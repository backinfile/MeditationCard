using Godot;
using System;
using System.Threading.Tasks;

public partial class CardInspiration : Card
{
    public CardInspiration()
    {
        Name = "Áé¸Ð";
        cost.Add(ResourceType.Enlight, 1);
        cost.Add(ResourceType.Heart, 1);
        cost.Add(ResourceType.AnyRes, 2);
        skills.Add(new ActiveSkill());
    }

    class ActiveSkill:Skill
    {
        public ActiveSkill()
        {
            description = "ºáÖÃ->³é2Æú1";
            activeSkills = true;
            tapCost = true;
        }
        public override async Task Use(Card card)
        {
            await Actions.DrawCard(2);
            await OperateActions.DiscardCardFromHand(1);
        }
    }
}
