using Godot;
using System;

public partial class CardNode : Control
{
    private static PackedScene element_object = GD.Load<PackedScene>("res://nodes/card_node.tscn");

    [Export]
    public int CostElementSize = 20;
    [Export]
    public int CostElementGap = 0;

    private TextureRect CardFocus;



    private double mouseDownTimer = -1f;
    private const double TIME_TO_START_DRAG = 0.15f;

    private Action _OnClick;
    private Action _OnRightClick;
    private Action<bool> _OnMouseEnter;

    private Action<bool> _OnDrag;
    private Action _onDragCancel;
    private bool _draggable = false;
    private bool _dragging = false;
    private Card card;
    private const bool CLICK_DRAG_INFO_OPEN = false;


    public static CardNode CreateNode()
    {
        CardNode node = element_object.Instantiate<CardNode>();
        node.CardFocus = node.GetNode<Control>("Control").GetNode<TextureRect>("CardFocus");
        return node;
    }

    public void Init(Card card)
    {
        this.card = card;
        var control = GetNode<Control>("Control");
        control.GetNode<Label>("Name").Text = card.Name;

        {
            var costNode = control.GetNode("Cost");
            int curPositionY = 0;
            foreach (ResourceType type in Utils.GetAllResType())
            {
                if (card.cost.Has(type))
                {
                    int cnt = card.cost.Get(type);
                    TextureRect rect = new TextureRect
                    {
                        Texture = type.GetTexture(),
                        Position = new Vector2(0, curPositionY),
                        Scale = new Vector2(CostElementSize, CostElementSize) / type.GetTexture().GetSize()
                    };
                    Label label = new Label
                    {
                        Text = cnt.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Size = new Vector2(CostElementSize, CostElementSize),
                        Position = new Vector2(0, curPositionY),
                        PivotOffset = new Vector2(CostElementSize / 2f, CostElementSize / 2f),
                        Scale = new Vector2(CostElementSize / 40f, CostElementSize / 40f)
                    };

                    curPositionY += CostElementSize + CostElementGap;
                    costNode.AddChild(rect);
                    if (cnt > 1)
                    {
                        costNode.AddChild(label);
                    }
                }
            }
            if (curPositionY > 0) curPositionY -= CostElementGap;
            costNode.GetNode<ColorRect>("ColorRect").Size = new Vector2(CostElementSize, curPositionY);
        }

        var ClickHandler = control.GetNode<ColorRect>("ClickHandler");

        ClickHandler.MouseEntered += () => { _OnMouseEnter?.Invoke(true); };
        ClickHandler.MouseExited += () => { _OnMouseEnter?.Invoke(false); };
        ClickHandler.GuiInput += (ev) =>
        {
            if (ev is InputEventMouseButton e && e.ButtonIndex == MouseButton.Left)
            {
                if (e.Pressed)
                {
                    mouseDownTimer = 0f;
                    if (CLICK_DRAG_INFO_OPEN) GD.Print("mouseDownTimer start");
                }
                else
                {
                    if (_dragging)
                    {
                        _dragging = false;
                        _OnDrag?.Invoke(false);
                        if (CLICK_DRAG_INFO_OPEN) GD.Print("_OnDrag false");
                    }
                    else if (mouseDownTimer >= 0 && mouseDownTimer <= TIME_TO_START_DRAG)
                    {
                        _OnClick?.Invoke();
                        if (CLICK_DRAG_INFO_OPEN) GD.Print("_OnClick");
                    }
                    mouseDownTimer = -1f;
                    if (CLICK_DRAG_INFO_OPEN) GD.Print("mouseDownTimer = -1");
                }
            }
            else if (ev is InputEventMouseMotion)
            {
                if (_draggable && mouseDownTimer >= 0 && !_dragging)
                {
                    _dragging = true;
                    _OnDrag?.Invoke(true);
                    if (CLICK_DRAG_INFO_OPEN) GD.Print("_OnDrag true by motion");
                }
            }
            if (ev is InputEventMouseButton e1 && e1.ButtonIndex == MouseButton.Right)
            {
                if (e1.Pressed) _OnRightClick?.Invoke();
            }
        };



        { // for test
            _draggable = true;
            _OnClick = () =>
            {
                GD.Print("OnClick");
            };
            _OnDrag = (v) =>
            {
                GD.Print("_OnDrag " + v);
                if (v)
                {
                    Tween tween = this.CreateTween();
                    tween.Parallel().TweenProperty(this, "scale", new Vector2(1.2f, 1.2f), 0.1f);
                }
                else
                {
                    Tween tween = this.CreateTween();
                    tween.Parallel().TweenProperty(this, "scale", new Vector2(1f, 1f), 0.1f);
                }
            };
            _OnMouseEnter = (v) =>
            {
                GD.Print("_OnMouseEnter " + v);
                //var target = v ? 1.5f : 1f;
                //Tween tween = this.CreateTween();
                //tween.Parallel().TweenProperty(this, "scale", new Vector2(target, target), 0.2f);
                //tween.Parallel().TweenProperty(this, "position", new Vector2(target, target), 0.2f);
            };
        }

        ClearAllInteract();
    }


    public void SetGlow(Color color)
    {
        var control = GetNode<Control>("Control");
        control.GetNode<ColorRect>("Glow").Color = color;
    }

    public void ClearAllInteract()
    {
        SetOnClick(null);
        SetOnRightClick(() => BigCardScreenManager.Show(card));
        SetOnMouseHover(null);
        SetOnDrag(false, null, null);
    }

    public void SetOnClick(Action action)
    {
        _OnClick = action;
    }

    public void SetOnRightClick(Action action)
    {
        _OnRightClick = action;
    }

    public void SetOnMouseHover(Action<bool> onHover)
    {
        _OnMouseEnter = onHover;
    }

    public void SetOnDrag(bool draggable, Action<bool> onDrag, Action onDragCancel)
    {
        if (_dragging && !draggable)
        {
            _dragging = false;
            mouseDownTimer = -1f;
            _onDragCancel?.Invoke();
            if (CLICK_DRAG_INFO_OPEN) GD.Print("_onDragCancel");
        }

        _draggable = draggable;
        _OnDrag = onDrag;
        _onDragCancel = onDragCancel;
    }

    public void SetFocus(bool focus)
    {
        CardFocus.Visible = focus;
    }

    public void SetCostHide(bool hide = true)
    {
        GetNode<Control>("Control").GetNode<Control>("Cost").Visible = !hide;
    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        {
            if (!_dragging && mouseDownTimer >= 0f)
            {
                mouseDownTimer += delta;
                if (mouseDownTimer > TIME_TO_START_DRAG && _draggable)
                {
                    _dragging = true;
                    _OnDrag?.Invoke(true);
                    if (CLICK_DRAG_INFO_OPEN) GD.Print("_OnDrag true by hold time");
                }
            }
        }

        if (_dragging)
        {
            Vector2 position = GetViewport().GetMousePosition();
            this.Position = position;
        }

        CardFocus.RotationDegrees += (float)(90 * delta);
    }
}
