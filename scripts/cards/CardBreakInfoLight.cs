using Godot;
using System;
using System.Threading.Tasks;

public partial class CardBreakInfoLight : Card
{
    public CardBreakInfoLight()
    {
        Name = "灵光乍现";
        cost.Add(ResourceType.Enlight, 1);
        cost.Add(ResourceType.Light, 2);
        skills.Add(new ActiveSkill());
    }

    class ActiveSkill : Skill
    {
        public ActiveSkill()
        {
            activeSkills = true;
            tapCost = false;
            description = "摧毁自身，将所有光转化为启";
        }

        public override bool CanUse(Card card)
        {
            return base.CanUse(card);
        }

        public override async Task Use(Card card)
        {
            await Actions.DiscardCard(card);
            int num = Utils.GetPlayer().resource.Get(ResourceType.Light);
            if (num > 0)
            {
                GameResource resource = new GameResource();
                resource.Add(ResourceType.Light, -num);
                resource.Add(ResourceType.Enlight, num);
                await Actions.AddResource(resource);
            }
        }
    }
}
