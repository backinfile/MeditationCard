using Godot;
using System;

public partial class CardDreamBarrier : Card
{
    public CardDreamBarrier()
    {
        Name = "√Œæ≥∆¡’œ";
        cost.Add(ResourceType.Enlight, 1);
        cost.Add(ResourceType.Heart, 1);
        cost.Add(ResourceType.AnyRes, 2);
        skills.Add(new GainResOnBuildSkill(new GameResource(ResourceType.Soul, 1)));
        skills.Add(new PrecipitationSkill(new GameResource(ResourceType.Light, 1, ResourceType.Heart, 1)));
    }
}
