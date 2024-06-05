using Godot;
using System;

public partial class CardMirrorWorld : Card
{
    public CardMirrorWorld()
    {
        Name = "æµ√Ê ¿ΩÁ";
        cost.Add(ResourceType.Light, 1);
        skills.Add(new PrecipitationSkill(new GameResource(ResourceType.Enlight, 1)));
    }
}
