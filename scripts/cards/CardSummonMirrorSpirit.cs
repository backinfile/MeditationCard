using Godot;
using System;
using System.Threading.Tasks;

public partial class CardSummonMirrorSpirit : Card 
{
    public CardSummonMirrorSpirit()
    {
        Name = "召唤：镜灵";

        cost.Add(ResourceType.Enlight, 1);
        cost.Add(ResourceType.Stone, 1);

        skills.Add(new ActiveSkill());
    }

    class ActiveSkill: Skill
    {
        public ActiveSkill()
        {
            activeSkills = true;
            tapCost = true;
            resourceCost.Add(ResourceType.Light, 4);
            description = "横置+4光->召唤镜灵";
        }

        public override bool CanUse(Card card)
        {
            if (Utils.GetBoard().IsFull()) return false;
            return base.CanUse(card);
        }

        public override async Task Use(Card card)
        {
            Utils.GetBoard().playgound.Add(new CardMirrorSpirit());
            await BoardRenderManager.RefreshPlaygound();
        }
    }
}
