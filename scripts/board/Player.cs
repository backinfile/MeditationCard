using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Player
{
    public readonly List<Card> handPile = new List<Card>();
    public readonly List<Card> drawPile = new List<Card>();
    public readonly List<Card> discardPile = new List<Card>();
    public readonly GameResource resource = new GameResource();

    public Player()
    {

    }

    public async Task Init()
    {
        for (int i = 1; i <= 8; i++)
        {
            drawPile.Add(new TmpCard());
        }
        GD.Print("drawPile size = " + drawPile.Count);
        await Actions.AddResource(ResourceType.Enlight, 1);
        await Actions.AddResource(ResourceType.Light, 1);
        await Actions.AddResource(ResourceType.Shadow, 1);
        await Actions.AddResource(ResourceType.Heart, 1);
        await Actions.AddResource(ResourceType.Stone, 1);
    }

    public async Task OnGameStart()
    {
        await Actions.DrawCard(4);
    }

    public async Task OnTurnStart()
    {
        await Actions.DrawCard(1);
    }
}
