using Godot;
using System;

public partial class CardNode : Node2D
{
    private static PackedScene element_object = GD.Load<PackedScene>("res://nodes/card_node.tscn");

    [Export]
    public int CostElementSize = 20;
    [Export]
    public int CostElementGap = 0;

    public static CardNode CreateNode()
    {
        CardNode node = element_object.Instantiate<CardNode>();
        return node;
    }

    public void Init(Card card)
    {
        Node2D control = GetNode<Node2D>("Control");
        control.GetNode<Label>("Name").Text = card.Name;

        {
            var costNode = control.GetNode("Cost");
            int curPositionY = 0;
            foreach(ResourceType type in Utils.GetAllResType())
            {
                if (card.cost.Has(type))
                {
                    int cnt = card.cost.Get(type);
                    TextureRect rect = new TextureRect
                    {
                        Texture = type.GetTexture(),
                        Position = new Vector2(0, curPositionY),
                        Scale =  new Vector2(CostElementSize, CostElementSize) / type.GetTexture().GetSize()
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


    }


}
