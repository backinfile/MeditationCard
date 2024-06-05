using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class CardRenderManager
{
    private static readonly Dictionary<Card, CardNode> map = new Dictionary<Card, CardNode>();

    public static bool IsShow(Card card)
    {
        return map.ContainsKey(card);
    }

    // not create card node
    public static CardNode GetNode(Card card)
    {
        if (map.TryGetValue(card, out var node))
        {
            return node;
        }
        return null;
    }

    public static CardNode Show(Card card)
    {
        if (map.TryGetValue(card, out var node))
        {
            return node;
        }

        GD.Print("create card node " + card.Name);

        CardNode cardNode = CardNode.CreateNode();
        cardNode.Position = new Vector2(0, GameNode.Instance.GetViewport().GetVisibleRect().Size.Y);
        cardNode.Scale = new Vector2(0, 0);
        cardNode.Init(card);
        GameNode.Instance.AddCard(cardNode);
        map[card] = cardNode;
        return cardNode;
    }

    public static void Destroy(Card card)
    {
        GD.Print("remove card node " + card.Name);
        if (map.TryGetValue(card, out CardNode cardNode))
        {
            GameNode.Instance.RemoveCard(cardNode);
            map.Remove(card);
        }
    }

    public static async Task Flash(Card card)
    {
        CardNode node = GetNode(card);
        if (node == null) return;

        var curScale = node.Scale;
        var expandScale = curScale * 1.2f;
        Tween tween = node.CreateTween();
        tween.TweenProperty(node, "scale", expandScale, Res.MOVE_INTERVAL / 2f);
        tween.TweenProperty(node, "scale", curScale, Res.MOVE_INTERVAL / 2f);
        await Actions.Wait(Res.MOVE_INTERVAL);
    }
}
