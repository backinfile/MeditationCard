using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;

public static class HandRenderManager
{
    private static Node2D HandAnchor;

    public static void Init()
    {
        HandAnchor = GameNode.Instance.GetNode<Node2D>("%HandAnchor");
    }

    public static async Task RefreshHand()
    {

        Vector2 position = HandAnchor.Position;
        var handPile = Utils.GetPlayer().handPile;
        int len = handPile.Count;
        int index = -1;

        float cardWidth = 140;
        float cardHeight = 196;
        float gap = cardWidth * 0.1f;
        float MOVE_INTERVAL = 0.13f;

        float centerX = position.X;
        float centerY = position.Y;
        float totalX = len * cardWidth + (len - 1) * gap;
        float startX = position.X - totalX / 2f + cardWidth / 2f;
        foreach (Card card in handPile)
        {
            index++;
            CardNode node = CardRenderManager.Show(card);

            float curX = startX + index * (cardWidth + gap);
            float curY = centerY;
            Tween tween = node.CreateTween();
            tween.Parallel().TweenProperty(node, "position", new Vector2(curX, curY), MOVE_INTERVAL);
            tween.Parallel().TweenProperty(node, "scale", new Vector2(1f, 1f), MOVE_INTERVAL);
            tween.Parallel().TweenProperty(node, "rotation", 0, MOVE_INTERVAL);
        }
        await Actions.Wait(MOVE_INTERVAL);
        GD.Print("wait finish");
    }
}
