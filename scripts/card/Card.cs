using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract class Card
{
    public string Name { get; protected set; } = "null";

    public CardType Type { get; protected set; } = CardType.Artifact;
    public readonly GameResource cost = new GameResource();
    public readonly List<Skill> skills = new List<Skill>();
    public readonly GameResource innerResource = new GameResource();
    public bool tapped = false;

    public Card()
    {
    }

    public virtual Card MakeCopy() // need to overwrite if contains complex constructor
    {
        return (Card)Activator.CreateInstance(this.GetType());
    }
}


public enum CardType
{
    Hero,
    Artifact
}

public abstract class Skill
{
    public bool activeSkills = false; // need active?

    public bool tapCost = false; // need tap for trigger skill
    public GameResource resourceCost = new GameResource();
    protected string description = "";

    public virtual bool CanUse(Card card)
    {
        if (!activeSkills) return false;
        if (!this.GetPlayer().resource.Test(resourceCost)) return false;
        return true;
    }
    public virtual async Task Use(Card card)
    {
        await Actions.DoNothing();
    }

    public virtual void UpdateDescription()
    {

    }


    public virtual void OnTurnStart(Card card)
    {

    }
    public virtual void OnTurnStartAfter(Card card)
    {

    }
    public virtual void OnTurnEnd(Card card)
    {

    }
    public virtual void OnTurnEndAfter(Card card)
    {

    }

    public virtual void OnTrigger(Card card, Card target)
    {

    }
}

public class HarvestSkill : Skill
{
    public GameResource resource;

    public HarvestSkill(GameResource resource)
    {
        this.activeSkills = false;
        this.tapCost = false;
        this.resource = resource;
        this.description = "Harvert: " + resource.ToString();
    }
}