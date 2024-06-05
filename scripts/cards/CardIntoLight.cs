using Godot;
using System;

public partial class CardIntoLight : Card
{
    public CardIntoLight()
    {
        Name = "步入光明";
        cost.Add(ResourceType.Light, 1);
        cost.Add(ResourceType.Heart, 1);

        skills.Add(new GainResOnBuildSkill(new GameResource(ResourceType.Soul, 1)));
        skills.Add(new PrecipitationSkill(new GameResource(ResourceType.Light, 1)));
    }
}
