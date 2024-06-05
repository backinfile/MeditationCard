using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Board 
{
    public BoardState State { get; private set; } = BoardState.None;
    public Player Player { get; } = new Player();

    public readonly List<Card> playgound = new List<Card>();

    public int Turn = 0;

    public async Task Update()
    {
        if (State == BoardState.None)
        {
            await ChangeStageTo(BoardState.Init);
        }
    }

    public async Task ChangeStageTo(BoardState state)
    {
        State = state;
        switch (state)
        {
            case BoardState.Init:
                {
                    GD.Print("State = Init");
                    await Actions.Wait(1);
                    await Player.Init();
                    await ChangeStageTo(BoardState.GameStart);
                    break;
                }
            case BoardState.GameStart:
                {
                    GD.Print("State = GameStart");
                    await Player.OnGameStart();
                    await ChangeStageTo(BoardState.TurnStart);
                    break;
                }
            case BoardState.TurnStart:
                {
                    GD.Print("State = TurnStart");
                    BoardRenderManager.SetTip(true, "回合开始");
                    Turn++;

                    BoardRenderManager.SetTurnInfo(Turn);
                    await Player.OnTurnStart();
                    await ChangeStageTo(BoardState.Turn);
                    BoardRenderManager.SetTip(false);
                    break;
                }
            case BoardState.Turn:
                {
                    GD.Print("State = Turn");
                    await Actions.SetPlayState(HandState.Play);
                    //await ChangeStageTo(BoardState.TurnEnd);
                    break;
                }
            case BoardState.TurnEnd:
                {
                    GD.Print("State = TurnEnd");
                    await Actions.SetPlayState(HandState.None);
                    await Player.OnTurnEnd();
                    await ChangeStageTo(BoardState.TurnStart);
                    break;
                }
            case BoardState.Clear:
                {
                    GD.Print("State = Clear");
                    await ChangeStageTo(BoardState.Finish);
                    break;
                }
        }
    }

    public bool IsFull()
    {
        return playgound.Count >= Res.PlaygoundSize;
    }
}
public enum BoardState
{
    None,
    Init,
    GameStart,
    TurnStart,
    Turn,
    TurnEnd,
    Clear,
    Finish
}


