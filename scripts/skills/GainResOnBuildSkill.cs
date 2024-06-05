using Godot;
using System;
using System.Threading.Tasks;

public partial class GainResOnBuildSkill : Skill 
{
    private GameResource resource;

    public GainResOnBuildSkill(GameResource resource)
    {
        this.resource = resource;
        description = "建造时获得：" + resource.ToString();
    }

    public override async Task OnBuild(Card card)
    {
        await Actions.AddResource(resource);
    }
}
