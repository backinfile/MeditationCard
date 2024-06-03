using Godot;
using System;
using System.Collections.Generic;

public partial class CardRenderManager
{
    private static readonly Dictionary<Card, CardNode> map = new Dictionary<Card, CardNode>();

    public static bool IsShow(Card card)
    {
        return map.ContainsKey(card);
    }

    public static CardNode Show(Card card)
    {
        if (map.TryGetValue(card, out var node))
        {
            return node;
        }

        GD.Print("create card node " + card.Name);

        CardNode cardNode = CardNode.CreateNode();
        cardNode.Init(card);
        GameNode.Instance.AddChild(cardNode);
        map[card] = cardNode;
        return cardNode;
    }

    public static void Hide(Card card)
    {

        GD.Print("remove card node " + card.Name);
        if (map.TryGetValue(card, out CardNode cardNode))
        {
            GameNode.Instance.RemoveCard(cardNode);
            map.Remove(card);
        }
    }
}
