using Godot;
using System;
using System.Threading.Tasks;

public partial class BoardRenderManager
{
    private static Control PlaygoundAnchor;
    private static Control TipAnchor;
    private static Button TurnEndButton;
    private static Label GameTurnInfo;

    public static Action<ResourceType, string> SetResourceCnt;

    private static HandState playgoundState = HandState.None;


    public static void Init()
    {
        PlaygoundAnchor = GameNode.Instance.GetNode<Control>("%PlaygoundAnchor");
        TipAnchor = GameNode.Instance.GetNode<Control>("%TipAnchor");
        TipAnchor.Visible = false;

        // 资源图标
        {
            var CntMap = new System.Collections.Generic.Dictionary<ResourceType, Label>();

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


        // 回合结束按钮
        {
            TurnEndButton = GameNode.Instance.GetNode<Button>("%TurnEndButton");
            TurnEndButton.Pressed += async () =>
            {
                ResetPlayerResourceView();
                await Actions.EndTurn();
            };
            TurnEndButton.MouseEntered += () =>
            {
                GameResource resource = new GameResource();
                foreach (var card in Utils.GetBoard().playgound)
                {
                    foreach (var skill in card.skills)
                    {
                        if (skill is PrecipitationSkill s)
                        {
                            resource.Add(s.resource);
                        }
                    }
                }
                AddResourcePreview(resource);
            };
            TurnEndButton.MouseExited += () =>
            {
                ResetPlayerResourceView();
            };
        }

        {
            GameTurnInfo = GameNode.Instance.GetNode<Label>("%GameTurnInfo");
        }
    }

    public static void SetTurnEndBtn(bool canTurnEnd)
    {
        TurnEndButton.Disabled = !canTurnEnd;
    }

    public static void SetTurnInfo(int turn)
    {
        GameTurnInfo.Text = $"现在是第{turn}回合";
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
        foreach (Card card in playgound)
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
        float rotation = card.tapped ? 90f : 0;
        Tween tween = node.CreateTween();
        tween.Parallel().TweenProperty(node, "position", CalcCardPosition(index), Res.MOVE_INTERVAL);
        tween.Parallel().TweenProperty(node, "scale", new Vector2(Res.SCALE_PLAYGROUNG, Res.SCALE_PLAYGROUNG), Res.MOVE_INTERVAL);
        tween.Parallel().TweenProperty(node, "rotation_degrees", rotation, Res.MOVE_INTERVAL);
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
                    node.SetGlow(Colors.Transparent);
                    node.ClearAllInteract();
                    Skill skill = card.GetActivableSkill();
                    if (skill != null && skill.CanUse(card))
                    {
                        GD.Print("skill can use");
                        Utils.GetPlayer().ConvertSkillCost(card, skill, out var converted);
                        node.SetGlow(Colors.White);
                        node.SetOnMouseHover(v =>
                        {
                            if (v)
                            {
                                BoardRenderManager.AddResourcePreview(converted, true);
                            }
                            else
                            {
                                BoardRenderManager.ResetPlayerResourceView();
                            }
                        });
                        node.SetOnClick(async () =>
                        {
                            BoardRenderManager.ResetPlayerResourceView();
                            await Actions.UseSkill(card, skill);
                        });
                    }
                    else
                    {
                        GD.Print("skill can not use");
                    }
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

    public static void AddResourcePreview(GameResource resource, bool remove = false)
    {
        if (remove)
        {
            resource = resource.MakeCopy();
            resource.Inverse();
        }
        GameResource playerResource = Utils.GetPlayer().resource;
        foreach (var type in Utils.GetAllResType())
        {
            int curResource = playerResource.Get(type);
            if (resource.Has(type))
            {
                int cnt = resource.Get(type);
                BoardRenderManager.SetResourceCnt(type, curResource + (cnt > 0 ? "+" : "") + cnt);
            }
        }
    }
    public static void ResetPlayerResourceView()
    {
        GameResource playerResource = Utils.GetPlayer().resource;
        foreach (var type in Utils.GetAllResType())
        {
            int curResource = playerResource.Get(type);
            BoardRenderManager.SetResourceCnt(type, curResource.ToString());
        }
    }

    public static void SetTip(bool show,  string text = "")
    {
        TipAnchor.Visible = show;
        TipAnchor.GetNode<Label>("Label").Text = text;
    }
}


public enum PlaygoundState
{
    None,
    Use,
    Select,
}