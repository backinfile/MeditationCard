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

        GameResource initResource = new GameResource();
        initResource.Add(ResourceType.Enlight, 7);
        initResource.Add(ResourceType.Light, 1);
        initResource.Add(ResourceType.Shadow, 1);
        initResource.Add(ResourceType.Heart, 1);
        initResource.Add(ResourceType.Stone, 1);
        await Actions.AddResource(initResource);
    }

    public async Task OnGameStart()
    {
        await Actions.DrawCard(7);
    }

    public async Task OnTurnStart()
    {
        await Actions.DrawCard(1);
    }
}
