using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class GameResource
{
    private readonly Dictionary<ResourceType, int> dict = new();

    public int Get(ResourceType type)
    {
        if (dict.TryGetValue(type, out var i)) return i;
        return 0;
    }

    public bool Has(ResourceType type)
    {
        return dict.ContainsKey(type);
    }

    public void Add(ResourceType type, int cnt)
    {
        int cur = Get(type) + cnt;
        if (cur > 0)
        {
            dict[type] = cur;
        }
        else
        {
            dict.Remove(type);
        }
    }

    public void Remove(ResourceType type)
    {
        dict.Remove(type);
    }

    public void Clear(ResourceType type)
    {
        dict.Clear();
    }
    public int Total()
    {
        return dict.Values.Sum();
    }
    public int TotalExceptSoul()
    {
        int cnt = 0;
        foreach (ResourceType type in Enum.GetValues<ResourceType>())
        {
            if (type != ResourceType.Soul) cnt += Get(type);
        }
        return cnt;
    }

    public void Add(GameResource resource)
    {
        if (resource.Get(ResourceType.AnyRes) != 0)
        {
            throw new Exception("add any resource");
        }
        foreach (ResourceType type in Enum.GetValues<ResourceType>())
        {
            Add(type, resource.Get(type));
        }
    }
    public void Remove(GameResource resource)
    {
        if (resource.Get(ResourceType.AnyRes) != 0)
        {
            throw new Exception("remove any resource");
        }
        foreach (ResourceType type in Enum.GetValues<ResourceType>())
        {
            int need = resource.Get(type);
            if (Get(type) < need)
            {
                throw new Exception($"reduce {need} from {Get(type)} by {type}");
            }
            Add(type, -need);
        }
    }

    public void Inverse()
    {
        foreach(var type in Utils.GetAllResType())
        {
            int cnt = Get(type);
            if (cnt != 0)
            {
                dict[type] = - cnt;
            }
        }
    }

    public bool Test(GameResource resNeed)
    {
        //GD.Print($"need:{resNeed}, own:{this}");
        var own = this.MakeCopy();
        foreach (ResourceType type in Enum.GetValues<ResourceType>())
        {
            if (type == ResourceType.AnyRes) continue;
            int need = resNeed.Get(type);
            if (need > 0 && own.Get(type) < need)
            {
                return false;
            }
            own.Add(type, -need);
        }
        int anyNeed = resNeed.Get(ResourceType.AnyRes);
        int leftRes = own.TotalExceptSoul();
        return leftRes >= anyNeed;
    }


    public GameResource MakeCopy()
    {
        var res = new GameResource();
        res.dict.Merge(dict);
        return res;

    }

    public override string ToString()
    {
        string res = "";
        foreach (ResourceType type in Enum.GetValues<ResourceType>())
        {
            int num = Get(type);
            if (num > 0)
            {
                if (res == "") res = num + " " + type.ToString();
                else res += ", " + num + " " + type.ToString();
            }
        }
        if (res == "") return "0";
        return res;
    }

}

public enum ResourceType
{
    Enlight,
    Light,
    Shadow,
    Stone,
    Heart,
    AnyRes,
    Soul
}

public static class ResourceUtil
{
    public static string GetName(this ResourceType type)
    {
        return type switch
        {
            ResourceType.Enlight => "启",
            ResourceType.Light => "光",
            ResourceType.Shadow => "影",
            ResourceType.Stone => "石",
            ResourceType.Heart => "心",
            ResourceType.AnyRes => "通用",
            ResourceType.Soul => "魂质",
            _ => "未知",
        };
    }
    public static Color GetColor(this ResourceType type)
    { 
        return type switch
        {
            ResourceType.Enlight => new Color(0.129f, 0.365f, 1),
            ResourceType.Light => new Color(0.827f, 0.949f, 0),
            ResourceType.Shadow => new Color(0, 0, 0),
            ResourceType.Stone => new Color(0.196f, 0.196f, 0.196f),
            ResourceType.Heart => new Color(1, 0.196f, 0.216f),
            ResourceType.AnyRes => new Color(0.349f, 0.412f, 0.682f),
            ResourceType.Soul => new Color(0.839f, 0.51f, 0),
            _ => Colors.White,
        };
    }

    public static Texture2D GetTexture(this ResourceType type)
    {
        return type switch
        {
            ResourceType.Enlight => GameNode.Instance.ResImage_Enlight,
            ResourceType.Light => GameNode.Instance.ResImage_Light,
            ResourceType.Shadow => GameNode.Instance.ResImage_Shadow,
            ResourceType.Stone => GameNode.Instance.ResImage_Stone,
            ResourceType.Heart => GameNode.Instance.ResImage_Heart,
            ResourceType.AnyRes => GameNode.Instance.ResImage_Any,
            ResourceType.Soul => GameNode.Instance.ResImage_Soul,
            _ => GameNode.Instance.ResImage_Any,
        };
    }
}