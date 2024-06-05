using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class CardForestTrail : Card
{
    public CardForestTrail()
    {
        Name = "����С��";
        cost.Add(ResourceType.Enlight, 1);
        skills.Add(new ActiveSkill());
    }

    class ActiveSkill : Skill
    {
        public ActiveSkill()
        {
            activeSkills = true;
            tapCost = true;
            resourceCost.Add(ResourceType.AnyRes, 1);
            description = "����+1ͨ��->1��1��";
        }

        public async override Task Use(Card card)
        {
            await Actions.AddResource(new GameResource(ResourceType.Enlight, 1, ResourceType.Light, 1));
        }
    }
}
