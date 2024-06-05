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

    public virtual bool CanPlay()
    {
        // ��λ���ɳ���7��
        if (Utils.GetBoard().playgound.Count >= Res.PlaygoundSize)
        {
            return false;
        }

        return Utils.GetPlayer().ConvertCardCost(this, out _);
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


    public async virtual Task OnTurnStart(Card card)
    {

        await Actions.DoNothing();
    }
    public async virtual Task OnTurnStartAfter(Card card)
    {
        await Actions.DoNothing();

    }
    public async virtual Task OnTurnEnd(Card card)
    {

        await Actions.DoNothing();
    }
    public async virtual Task OnTurnEndAfter(Card card)
    {

        await Actions.DoNothing();
    }

    public virtual void OnTrigger(Card card, Card target)
    {

    }
}
