using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class Player 
{
    public readonly List<Card> handPile = new List<Card>();
    public readonly List<Card> drawPile = new List<Card>();
    public readonly List<Card> discardPile = new List<Card>();
    public readonly GameResource resource = new GameResource();

    public Player()
    {

    }

    public async Task Init()
    {
        { // init drawPile
            drawPile.Add(new CardBreakInfoLight());
            drawPile.Add(new CardDreamBarrier());
            drawPile.Add(new CardForestTrail());
            drawPile.Add(new CardInspiration());
            drawPile.Add(new CardIntoLight());
            drawPile.Add(new CardMirrorSpirit());
            drawPile.Add(new CardMirrorWorld());
            drawPile.Add(new CardSpiritWorldLibrary());
            drawPile.Add(new CardSummonMirrorSpirit());
            drawPile.Add(new CardWithGod());
            drawPile.Add(new HeroLight());

            List<Card> ininateCards = drawPile.Where(c => c.GetSkill<IninateSkill>() != null).ToList();
            foreach (var item in ininateCards) drawPile.Remove(item);
            drawPile.Shuffle();
            ininateCards.Shuffle();
            drawPile.AddAll(ininateCards);
        }
        GD.Print("drawPile size = " + drawPile.Count);

        GameResource initResource = new GameResource();
        initResource.Add(ResourceType.Enlight, 1);
        initResource.Add(ResourceType.Light, 1);
        initResource.Add(ResourceType.Shadow, 1);
        initResource.Add(ResourceType.Heart, 1);
        initResource.Add(ResourceType.Stone, 1);
        await Actions.AddResource(initResource);
    }

    public async Task OnGameStart()
    {
        await Actions.DrawCard(4);
    }

    public async Task OnTurnStart()
    {
        foreach(var card in Utils.GetBoard().playgound)
        {
            card.tapped = false;
        }
        await BoardRenderManager.RefreshPlaygound();
        await Actions.DrawCard(1);
    }

    public async Task OnTurnEnd()
    {
        foreach (var card in Utils.GetBoard().playgound)
        {
            foreach (var skill in card.skills)
            {
                await skill.OnTurnEnd(card);
            }
        }
        foreach (var card in Utils.GetBoard().playgound)
        {
            foreach (var skill in card.skills)
            {
                await skill.OnTurnEndAfter(card);
            }
        }
    }

    public bool CanFitCardCost(Card card)
    {
        return ConvertCardCost(card, out _);
    }

    public bool ConvertCardCost(Card card, out GameResource converted)
    {
        GameResource cost = card.cost.MakeCopy();
        // TODO: may have some reduce resource effect
        return ConvertCost(cost, out converted);
    }

    public bool ConvertSkillCost(Card card, Skill skill, out GameResource converted)
    {
        GameResource cost = skill.resourceCost.MakeCopy();
        // TODO: may have some reduce resource effect
        return ConvertCost(cost, out converted);
    }

    public bool ConvertCost(GameResource cost, out GameResource converted)
    {
        converted = new GameResource();
        cost = cost.MakeCopy();
        var own = resource.MakeCopy();

        // fit need for each type
        foreach (var type in Utils.GetAllResType())
        {
            if (type == ResourceType.AnyRes) continue;
            int need = cost.Get(type);
            int has = own.Get(type);
            int provide = Math.Min(has, need);
            if (provide > 0)
            {
                converted.Add(type, provide);
                own.Add(type, -provide);
                cost.Add(type, -provide);
            }
        }
        // use enlight to fit each type
        {
            int leftEnlight = own.Get(ResourceType.Enlight);
            foreach (var type in Utils.GetAllResType()) // 启可以用来代替光影石心
            {
                if (type == ResourceType.AnyRes || type == ResourceType.Enlight || type == ResourceType.Soul) continue;
                int need = cost.Get(type);
                int provide = Math.Min(need, leftEnlight);
                if (provide > 0)
                {
                    converted.Add(ResourceType.Enlight, provide);
                    leftEnlight -= provide;
                    own.Add(ResourceType.Enlight, -provide);
                    cost.Add(type, -provide);
                }
            }
        }

        // fit any type
        {
            int needAny = cost.Get(ResourceType.AnyRes);// 需求通用元素，可以用启光影石心
            foreach (var type in new ResourceType[]{
                ResourceType.Stone, ResourceType.Heart, ResourceType.Shadow, ResourceType.Light, ResourceType.Enlight
            })
            {
                int has = own.Get(type);
                int provide = Math.Min(has, needAny);
                if (provide > 0)
                {
                    converted.Add(type, provide);
                    needAny -= provide;
                    own.Add(type, -provide);
                    cost.Add(ResourceType.AnyRes, -provide);
                }
            }
        }

        // 还剩下了元素没有满足
        if (cost.Total() > 0)
        {
            return false;
        }
        return true;
    }
}
