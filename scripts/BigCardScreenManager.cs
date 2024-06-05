using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class BigCardScreenManager
{
    private static CanvasLayer BigCardScreen;
    private static Card curCard;

    public static void Init()
    {
        BigCardScreen = GameNode.Instance.GetNode<CanvasLayer>("BigCardScreen");
        BigCardScreen.Visible = false;
        BigCardScreen.GetNode("Control").GetNode<Control>("CenterContainer").GuiInput += ev =>
        {
            if (ev is InputEventMouseButton e && e.Pressed)
            {
                BigCardScreen.Visible = false;
                CardRenderManager.GetNode(curCard)?.SetFocus(false);
            }
        };
    }

    public static void Show(Card card)
    {
        curCard = card;
        BigCardScreen.Visible = true;
        Control infoControl = BigCardScreen.GetChild<Control>(0).GetNode<Control>("CenterContainer").GetChild<Control>(0);
        infoControl.GetNode<Label>("Name").Text = card.Name;
        infoControl.GetNode<Label>("Cost").Text = "cost: " + card.cost.ToString();

        CardRenderManager.GetNode(card)?.SetFocus(true);
    }


}
