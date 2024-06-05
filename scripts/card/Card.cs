using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
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
        // 单位不可超过7个
        if (Utils.GetBoard().playgound.Count >= Res.PlaygoundSize)
        {
            return false;
        }

        return Utils.GetPlayer().ConvertCardCost(this, out _);
    }

    public Skill GetActivableSkill()
    {
        foreach(var skill in skills)
        {
            if (skill.activeSkills) return skill;
        }
        return null;
    }

    public T GetSkill<T>() where T : Skill
    {
        foreach(var skill in skills)
        {
            if (skill is T)
            {
                return skill as T;
            }
        }
        return null;
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
    public bool isKeyword = false;

    public bool tapCost = false; // need tap for trigger skill
    public GameResource resourceCost = new GameResource();
    public string description = "";

    public virtual bool CanUse(Card card)
    {
        if (!activeSkills) return false;
        if (tapCost && card.tapped) return false;
        if (!Utils.GetPlayer().ConvertSkillCost(card, this, out _)) return false;
        return true;
    }
    public virtual async Task Use(Card card)
    {
    }

    public virtual void UpdateDescription()
    {

    }

    public async virtual Task OnBuild(Card card)
    {
        await Actions.DoNothing();
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
