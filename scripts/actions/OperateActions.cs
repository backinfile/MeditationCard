using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public static class OperateActions
{
    private static object waitResult = null;

    public static async Task DiscardCardFromHand(int cnt = 1)
    {
        List<Card> handPile = Utils.GetPlayer().handPile;
        if (handPile.Count == 0) return;

        Card card = await OperateActions.SelectCard(handPile, $"弃置{cnt}张手牌");
        await Actions.DiscardCard(card);
    }

    private static int GetCardIndex(Card card)
    {
        {
            int index = Utils.GetBoard().playgound.IndexOf(card);
            if (index >= 0) return index;
        }
        {
            int index = Utils.GetPlayer().handPile.IndexOf(card);
            if (index >= 0) return index + 10;
        }
        return 0;
    }

    public static async Task<Card> SelectCard(List<Card> selectFrom, string tip = "", int cnt = 1, bool cancelable = false)
    {
        List<Card> Selected = new List<Card>();

        // setup interact
        foreach (Card card in selectFrom)
        {
            CardNode node = CardRenderManager.GetNode(card);
            node.ZIndex = Res.ZIndex_Card_To_Select + GetCardIndex(card);
            node.ClearAllInteract();
            node.SetGlow(Colors.White);
            node.SetOnClick(() =>
            {
                var node = CardRenderManager.GetNode(card);
                if (Selected.Contains(card))
                {
                    Selected.Remove(card);
                    node.SetSelected(false);
                }
                else
                {
                    Selected.Add(card);
                    node.SetSelected(true);
                    if (Selected.Count == cnt)
                    {
                        waitResult = Selected;
                    }
                }
            });
        }
        BoardRenderManager.SetTip(true, tip == "" ? $"选择{cnt}张牌" : tip);

        // TODO cancelable

        // wait for selecting
        List<Card> cards = await WaitOperateResult<List<Card>>();

        // clear interact
        BoardRenderManager.SetTip(false);
        await Actions.SetPlayState(HandState.None);


        if (cards.Count == 0)
        {
            return null;
        }
        return cards[0];
    }

    private static async Task<T> WaitOperateResult<T>()
    {
        waitResult = null;
        while (true)
        {
            await GameNode.WaitBoardUpdate();
            if (waitResult != null)
            {
                break;
            }
        }
        T result = (T)waitResult;
        waitResult = null;
        return result;
    }
}
