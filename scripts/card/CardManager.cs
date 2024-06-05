using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

public class CardManager
{

    public static List<Card> CreateShop()
    {
        List<Card> list = new List<Card>();

        return list;
    }
    private static readonly List<Card> allCards = new();

    public static void Init()
    {
        foreach (Type type in Utils.GetAllSubclasses(typeof(Card)))
        {
            allCards.Add((Card)Activator.CreateInstance(type));
        }
    }

    public static List<Card> GetPlayerInitCards()
    {
        return allCards.Select(c => c.MakeCopy()).ToList();
    }
}
