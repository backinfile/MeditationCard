using Godot;
using System;

public class HeroLight : Card 
{
    public HeroLight()
    {
        Name = "初始英雄";
        Type = CardType.Hero;
        skills.Add(new IninateSkill());
        skills.Add(new PrecipitationSkill(new GameResource(ResourceType.Light, 1, ResourceType.Enlight, 1)));
        skills.Add(new DiscardHandToGainSkill(new GameResource(ResourceType.Enlight, 1, ResourceType.Light, 1)));
    }



}
