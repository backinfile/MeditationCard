using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class BoardRenderManager
{
    private static Control PlaygoundAnchor;
    public static Action<ResourceType, string> SetResourceCnt;

    private static HandState playgoundState = HandState.None;


    public static void Init()
    {
        PlaygoundAnchor = GameNode.Instance.GetNode<Control>("%PlaygoundAnchor");

        Dictionary<ResourceType, Label> CntMap = new Dictionary<ResourceType, Label>();

        Node ResourceAnchor = GameNode.Instance.GetNode("%ResourceAnchor");
        int curPositionX = 0;
        int curPositionY = 0;
        int resourceIconSize = GameNode.Instance.ResourceIconSize;
        int resourceIconGap = GameNode.Instance.ResourceIconGap;
        foreach (var type in Utils.GetAllResType())
        {
            if (type == ResourceType.AnyRes)
            {
                continue;
            }

            int cnt = 0;
            TextureRect rect = new TextureRect
            {
                Texture = type.GetTexture(),
                Position = new Vector2(curPositionX, curPositionY),
                Scale = new Vector2(resourceIconSize, resourceIconSize) / type.GetTexture().GetSize()
            };
            Label label = new Label
            {
                Text = cnt.ToString(),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Size = new Vector2(resourceIconSize, resourceIconSize),
                Position = new Vector2(resourceIconSize, curPositionY),
                PivotOffset = new Vector2(resourceIconSize / 2f, resourceIconSize / 2f),
                //Scale = new Vector2(resourceIconSize / 40f, resourceIconSize / 40f)
            };

            curPositionY += resourceIconSize + resourceIconGap;
            ResourceAnchor.AddChild(rect);
            ResourceAnchor.AddChild(label);
            CntMap[type] = label;
        }
        ResourceAnchor.GetNode<ColorRect>("ColorRect").Size = new Vector2(resourceIconSize * 2, resourceIconSize * 6 + resourceIconGap * 5);
        SetResourceCnt = (type, cnt) =>
        {
            if (CntMap.ContainsKey(type))
            {
                CntMap[type].Text = cnt.ToString();
            }
        };
    }

    public static async Task SetPlaygoundState(HandState state)
    {
        playgoundState = state;
        await RefreshPlaygound();
    }

    public static async Task RefreshPlaygound()
    {
        var playgound = Utils.GetBoard().playgound;
        int index = -1;
        foreach(Card card in playgound)
        {
            index++;
            RefreshPlaygoundCard(card, index);
        }
        await Actions.Wait(Res.MOVE_INTERVAL);
    }

    private static void RefreshPlaygoundCard(Card card, int index)
    {
        GD.Print("RefreshCard");
        CardNode node = CardRenderManager.Show(card);
        Tween tween = node.CreateTween();
        tween.Parallel().TweenProperty(node, "position", CalcCardPosition(index), Res.MOVE_INTERVAL);
        tween.Parallel().TweenProperty(node, "scale", new Vector2(Res.SCALE_PLAYGROUNG, Res.SCALE_PLAYGROUNG), Res.MOVE_INTERVAL);
        tween.Parallel().TweenProperty(node, "rotation", 0, Res.MOVE_INTERVAL);
        node.ZIndex = Res.ZIndex_Board + index;

        node.SetCostHide(true);

        switch (playgoundState)
        {
            case HandState.None:
                {
                    node.SetGlow(Colors.Transparent);
                    node.ClearAllInteract();
                    break;
                }
            case HandState.Play:
                {
                    bool canPlay = card.GetPlayer().resource.Test(card.cost);
                    GD.Print("canPlay: " + canPlay);
                    node.SetGlow(canPlay ? Colors.Blue : Colors.Transparent);
                    
                    break;
                }
            case HandState.Select:
                {
                    break;

                }
        }

    }

    private static Vector2 CalcCardPosition(int index)
    {
        int handSize = Res.PlaygoundSize;
        float centerX = PlaygoundAnchor.Position.X;
        float centerY = PlaygoundAnchor.Position.Y;
        float scale = Res.SCALE_PLAYGROUNG;
        float totalX = handSize * Res.CardWidth * scale + (handSize - 1) * Res.PlaygoundCardX_Gap * scale;
        float startX = centerX - totalX / 2f + Res.CardWidth / 2f;
        float curX = startX + index * (Res.CardWidth + Res.PlaygoundCardX_Gap) * scale;
        float curY = centerY;
        return new Vector2(curX, curY);

    }
}


public enum PlaygoundState
{
    None,
    Use,
    Select,
}