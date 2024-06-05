using Godot;
using System;

public partial class CardSpiritWorldLibrary : Card
{
    public CardSpiritWorldLibrary()
    {
        Name = "¡ÈΩÁÕº Èπ›";
        cost.Add(ResourceType.Enlight, 2);
        cost.Add(ResourceType.Light, 2);
        cost.Add(ResourceType.Stone, 1);
        cost.Add(ResourceType.AnyRes, 2);

        skills.Add(new PrecipitationSkill(new GameResource(ResourceType.Soul, 1)));
        skills.Add(new TransformSkill(new GameResource(ResourceType.Enlight, 5), new GameResource(ResourceType.Soul, 3)));
    }
}
