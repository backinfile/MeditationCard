using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;

public static class HandRenderManager
{
    private static Control HandAnchor;
    private static HandState handState = HandState.Play;

    public static void Init()
    {
        HandAnchor = GameNode.Instance.GetNode<Control>("%HandAnchor");
    }


    public static async Task SetHandState(HandState state)
    {
        handState = state;
        await RefreshHand();
    }

    public static async Task RefreshHand()
    {
        int index = -1;
        List<Card> handPile = Utils.GetPlayer().handPile;
        int handSize = handPile.Count;
        foreach (Card card in handPile)
        {
            index++;
            RefreshCard(card, handSize, index);
        }
        await Actions.Wait(Res.MOVE_INTERVAL);
    }

    private static void RefreshCard(Card card, int handSize, int index)
    {
        GD.Print("RefreshCard");
        CardNode node = CardRenderManager.Show(card);
        Tween tween = node.CreateTween();
        tween.Parallel().TweenProperty(node, "position", CalcHandCardPosition(handSize, index), Res.MOVE_INTERVAL);
        tween.Parallel().TweenProperty(node, "scale", new Vector2(Res.SCALE_HAND, Res.SCALE_HAND), Res.MOVE_INTERVAL);
        tween.Parallel().TweenProperty(node, "rotation_degrees", 0, Res.MOVE_INTERVAL);
        node.ZIndex = Res.ZIndex_Hand + index;

        switch (handState)
        {
            case HandState.None:
                {
                    node.SetGlow(Colors.Transparent);
                    node.ClearAllInteract();
                    break;
                }
            case HandState.Play:
                {
                    node.SetOnMouseHover(null);
                    bool canPlay = card.CanPlay();
                    Utils.GetPlayer().ConvertCardCost(card, out var converted);
                    //GD.Print("canPlay: " + canPlay);
                    node.SetGlow(canPlay ? Colors.White : Colors.Transparent);
                    node.SetOnDrag(canPlay, async v =>
                    {
                        if (v) // drag start
                        {
                            node.CreateTween().TweenProperty(node, "scale", new Vector2(Res.SCALE_DRAG, Res.SCALE_DRAG), Res.MOVE_INTERVAL);
                            BoardRenderManager.AddResourcePreview(converted, true);
                            node.ZIndex = Res.ZIndex_Drag;
                        } else // drag end
                        {
                            if (node.Position.Y < HandAnchor.Position.Y - Res.CardHeight / 2f)
                            {
                                node.ClearAllInteract();
                                await Actions.PlayCard(card);
                            } else
                            {
                                BoardRenderManager.ResetPlayerResourceView();
                                RefreshCard(card, handSize, index);
                            }
                        }

                    }, () => RefreshCard(card, handSize, index)
                    );
                    break;
                }
            case HandState.Select:
                {
                    break;

                }
        }

    }

    private static Vector2 CalcHandCardPosition(int handSize, int index)
    {
        float centerX = HandAnchor.Position.X;
        float centerY = HandAnchor.Position.Y;
        float totalX = handSize * Res.CardWidth + (handSize - 1) * Res.HandCardX_Gap;
        float startX = centerX - totalX / 2f + Res.CardWidth / 2f;
        float curX = startX + index * (Res.CardWidth + Res.HandCardX_Gap);
        float curY = centerY;
        return new Vector2(curX, curY);

    }
}


public enum HandState
{
    None,
    Play, // can play card from hand
    Select, // select card from hand
}