using Godot;
using System;
using System.Threading.Tasks;

public partial class GainResOnBuildSkill : Skill
{
    private GameResource resource;

    public GainResOnBuildSkill(GameResource resource)
    {
        this.resource = resource;
        description = "����ʱ��ã�" + resource.ToString();
    }

    public override async Task OnBuild(Card card)
    {
        await Actions.AddResource(resource);
    }
}
