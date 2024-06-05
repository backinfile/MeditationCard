using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class GameNode : Node
{
    public static GameNode Instance { get; private set; }

    [Signal]
    public delegate void BoardUpdateEventHandler();

    [Export]
    public Texture2D ResImage_Enlight;
    [Export]
    public Texture2D ResImage_Light;
    [Export]
    public Texture2D ResImage_Shadow;
    [Export]
    public Texture2D ResImage_Stone;
    [Export]
    public Texture2D ResImage_Heart;
    [Export]
    public Texture2D ResImage_Any;
    [Export]
    public Texture2D ResImage_Soul;


    [Export]
    public int ResourceIconSize = 40;
    [Export]
    public int ResourceIconGap = 0;

    public override void _Ready()
    {
        Instance = this;
        if (OS.GetName() == "Windows")
        {
            //DisplayServer.WindowSetSize(new Vector2I(480, 800));
        }
        BoardRenderManager.Init();
        HandRenderManager.Init();
        BigCardScreenManager.Init();
        GameManager.StartGame();

    }

    public void Test()
    {
        Card card = new TmpCard();
        card.cost.Add(ResourceType.AnyRes, 1);
        card.cost.Add(ResourceType.Enlight, 1);
        card.cost.Add(ResourceType.Soul, 1);
        card.cost.Add(ResourceType.Heart, 2);
        CardNode cardNode = CardRenderManager.Show(card);
        cardNode.Position = new Vector2(200, 200);

        var tween = cardNode.CreateTween();
        tween.Parallel();

        foreach (var type in Utils.GetAllSubclasses(typeof(Card)))
        {
            GD.Print(type.Name);
        }


        LinkedList<int> arr = new LinkedList<int>();
        arr.AddLast(0);
        arr.AddLast(1);
        arr.AddLast(2);
        arr.AddLast(3);
        arr.Shuffle();
        foreach (var i in arr)
        {
            GD.Print(i);
        }
    }

    public override async void _Process(double delta)
    {
        await GameManager.Update();
        EmitSignal(SignalName.BoardUpdate);
    }

    private async Task WaitPrivate(double time)
    {
        await ToSignal(GetTree().CreateTimer(time), "timeout");
    }

    public static async Task Wait(double time)
    {
        await Instance.WaitPrivate(time);
    }
    private async Task WaitBoardUpdatePrivate()
    {
        await ToSignal(this, SignalName.BoardUpdate);
    }

    public static async Task WaitBoardUpdate()
    {
        await Instance.WaitBoardUpdatePrivate();
    }

    public void AddCard(Control node)
    {
        AddChild(node);
    }
    public void RemoveCard(Control node)
    {
        RemoveChild(node);
        node.QueueFree();
    }
}
