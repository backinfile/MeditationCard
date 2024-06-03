using Godot;
using System;
using System.Collections.Generic;


public class CardConfig
{

}

public class CardCost
{
    public CardCostType Type { get; set; }
    public List<int> numbers = new(); 
    public int sum;
}

public enum CardCostType
{
    None, 
    Normal, 
    Sum,
    Tuple, 
    Triple, 
}

public class CardEffectActiveCost
{
    public bool rotation; 
    public int many; 
    public List<int> numbers = new(); 
}
