using Godot;
using System;
using System.Threading.Tasks;

public partial class PrecipitationSkill : Skill
{
    public GameResource resource;

    public PrecipitationSkill(GameResource resource)
    {
        this.activeSkills = false;
        this.tapCost = false;
        this.resource = resource;
        this.description = "³Áµí: " + resource.ToString();
    }

    public async override Task OnTurnEnd(Card card)
    {
        await CardRenderManager.Flash(card);
        await Actions.AddResource(resource);
    }
}
