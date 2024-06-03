using Godot;
using Godot.Collections;
using System;

public partial class BoardRenderManager
{
    public static Action<ResourceType, int> SetResourceCnt;

    public static void Init()
    {
        Dictionary<ResourceType, Label> CntMap = new Dictionary<ResourceType, Label>();

        Node ResourceAnchor = GameNode.Instance.GetNode("%ResourceAnchor");
        int curPositionX = 0;
        int curPositionY = 0;
        int resourceIconSize = GameNode.Instance.ResourceIconSize;
        int resourceIconGap = GameNode.Instance.ResourceIconGap;
        foreach (var type in Utils.GetAllResType())
        {
            if (type == ResourceType.Heart)
            {
                curPositionX += resourceIconSize + resourceIconGap;
                curPositionY = 0;
            } else if (type == ResourceType.AnyRes)
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
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Size = new Vector2(resourceIconSize, resourceIconSize),
                Position = new Vector2(curPositionX, curPositionY),
                PivotOffset = new Vector2(resourceIconSize / 2f, resourceIconSize / 2f),
                Scale = new Vector2(resourceIconSize / 40f, resourceIconSize / 40f)
            };

            curPositionY += resourceIconSize + resourceIconGap;
            ResourceAnchor.AddChild(rect);
            ResourceAnchor.AddChild(label);
            CntMap[type] = label;
        }
        ResourceAnchor.GetNode<ColorRect>("ColorRect").Size = new Vector2(resourceIconSize * 2 + resourceIconGap, resourceIconSize * 3 + resourceIconGap * 2);
        SetResourceCnt = (type, cnt) =>
        {
            if (CntMap.ContainsKey(type))
            {
                CntMap[type].Text = cnt.ToString();
            }
        };
    }
}
