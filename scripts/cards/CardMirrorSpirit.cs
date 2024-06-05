using Godot;
using System;

public partial class CardMirrorSpirit : Card
{
    public CardMirrorSpirit()
    {
        Name = "æµ¡È";
        cost.Add(ResourceType.Light, 2);
        cost.Add(ResourceType.Heart, 1);
        cost.Add(ResourceType.AnyRes, 1);

        skills.Add(new PrecipitationSkill(new GameResource(ResourceType.Light, 2)));
        skills.Add(new TransformSkill(new GameResource(ResourceType.AnyRes, 1), new GameResource(ResourceType.Enlight, 1)));
    }
}
