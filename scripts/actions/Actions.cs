using Godot;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading.Tasks;

public static class Actions
{
    private static object waitResult = null;

    public static async Task SetPlayState(HandState state)
    {
        BoardRenderManager.SetTurnEndBtn(state == HandState.Play);

        await HandRenderManager.SetHandState(state);
        await BoardRenderManager.SetPlaygoundState(state);
    }

    public static async Task EndTurn()
    {
        if (GetBoard().State == BoardState.Turn)
        {
            await GetBoard().ChangeStageTo(BoardState.TurnEnd);
        }
        else
        {
            GD.PrintErr("End turn in state " + GetBoard().State);
        }
    }

    public static async Task AddResource(GameResource resource, bool remove = false, bool animation = true)
    {
        if (remove)
        {
            resource = resource.MakeCopy();
            resource.Inverse();
        }

        GameResource playerResource = GetPlayer().resource;
        if (animation)
        {
            BoardRenderManager.AddResourcePreview(resource);
            await Wait(0.3f);
        }
        playerResource.Add(resource);
        BoardRenderManager.ResetPlayerResourceView();
    }
    public static async Task AddResource(ResourceType resourceType, int cnt)
    {
        GameResource resource = new GameResource();
        resource.Add(resourceType, cnt);
        await AddResource(resource);
    }

    public static async Task ClearResource()
    {
        foreach (var type in Utils.GetAllResType())
        {
            GetPlayer().resource.Remove(type);
            BoardRenderManager.SetResourceCnt(type, "0");
        }
        await DoNothing();
    }

    public static async Task PlayCard(Card card)
    {
        GD.Print("playerCard");
        GetPlayer().ConvertCardCost(card, out var converted);
        await AddResource(converted, true, false);

        GetPlayer().handPile.Remove(card);
        GetBoard().playgound.Add(card);

        await BoardRenderManager.RefreshPlaygound();
        await HandRenderManager.RefreshHand();
    }

    public static async Task DrawCard(int cnt = 1)
    {
        GD.Print("Action:DrawCard " + cnt);
        Player player = GetPlayer();
        for (int i = 0; i < cnt; i++)
        {
            if (player.drawPile.Count == 0)
            {
                await ShuffleDiscardPileIntoDrawPile();
            }
            if (player.drawPile.Count == 0)
            {
                break;
            }
            Card card = player.drawPile[player.drawPile.Count - 1];
            player.drawPile.Remove(card);
            player.handPile.Add(card);
            await HandRenderManager.RefreshHand();
        }
    }
    public static async Task ShuffleDiscardPileIntoDrawPile()
    {
        GD.Print("Action:ShuffleDiscardPileIntoDrawPile");
        Player player = GetPlayer();
        player.drawPile.AddAll(player.discardPile);
        player.drawPile.Shuffle();
        await DoNothing();
    }

    public static async Task<Card> SelectCardOnBoard(List<Card> selectFrom, bool cancelable = false)
    {
        await Wait(1);
        return null;
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

    public static async Task Wait(double time)
    {
        await GameNode.Wait(time);
    }
    public static async Task DoNothing()
    {

    }

    public static Player GetPlayer()
    {
        return Utils.GetPlayer();
    }
    public static Board GetBoard()
    {
        return Utils.GetBoard();
    }
}
