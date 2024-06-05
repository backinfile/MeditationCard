using Godot;
using System;
using System.Threading.Tasks;

public partial class TransformSkill : Skill 
{
    private GameResource to;

    public TransformSkill(GameResource from, GameResource to)
    {
        this.resourceCost.Add(from);
        this.to = to;

        activeSkills = true;
        tapCost = true;

        description = "横置+" + resourceCost.ToString() + "->" + to.ToString();
    }

    public override async Task Use(Card card)
    {
        await Actions.AddResource(to);
    }
}
