using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class CardWithGod : Card
{
    public CardWithGod()
    {
        Name = "�������";

        cost.Add(ResourceType.Enlight, 1);
        cost.Add(ResourceType.Light, 1);
        cost.Add(ResourceType.Shadow, 1);
        cost.Add(ResourceType.Heart, 1);
        cost.Add(ResourceType.Stone, 1);

        skills.Add(new PrecipitationSkill(new GameResource(ResourceType.Light, 1)));
        skills.Add(new ActiveSkill());
    }

    class ActiveSkill :Skill
    {
        public ActiveSkill()
        {
            activeSkills = true;
            tapCost = true;
            description = "����->�ݻ�һ����˼�������7��";
        }

        public override bool CanUse(Card card)
        {
            if (!Utils.GetBoard().playgound.Any(c => c != card && c.cost.Get(ResourceType.Light) > 0)) return false;
            return base.CanUse(card);
        }

        public override async Task Use(Card card)
        {
            var toDestroy = Utils.GetBoard().playgound.Where(c => c != card && c.cost.Get(ResourceType.Light) > 0).ToList();
            Card destoryCard = await OperateActions.SelectCard(toDestroy, "�ݻ�һ����˼��");
            await Actions.DiscardCard(destoryCard);
            await Actions.AddResource(new GameResource(ResourceType.Light, 7));
        }
    }
}
