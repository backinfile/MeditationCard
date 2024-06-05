using Godot;
using System;
using System.Threading.Tasks;

public partial class DiscardHandToGainSkill : Skill
{
    private GameResource resource;
    public DiscardHandToGainSkill(GameResource resource)
    {
        this.resource = resource;
        activeSkills = true;
        tapCost = true;
        description = "����->����1�����ƣ����1��1��";
    }

    public override bool CanUse(Card card)
    {
        if (Utils.GetPlayer().handPile.Count == 0) return false;
        return base.CanUse(card);
    }

    public override async Task Use(Card card)
    {
        Card discard = await OperateActions.SelectCard(Utils.GetPlayer().handPile, "����1������");
        await Actions.DiscardCard(discard);
        await Actions.AddResource(new GameResource(ResourceType.Enlight, 1, ResourceType.Light, 1));
    }
}
