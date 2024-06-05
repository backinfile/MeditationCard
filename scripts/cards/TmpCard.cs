using Godot;
using System;

public partial class TmpCard :Card
{

    // 统觉
    public TmpCard()
    {
        cost.Add(ResourceType.Enlight, 1);
        cost.Add(ResourceType.AnyRes, 1);
        Name = "统觉";

        skills.Add(new PrecipitationSkill(new GameResource(ResourceType.Light, 1)));
    }
}
