using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class Utils
{
    private static Random random = new Random();
    private static readonly ResourceType[] resourceTypes = { ResourceType.Enlight, ResourceType.Light, ResourceType.Shadow, ResourceType.Heart, ResourceType.Stone, ResourceType.AnyRes, ResourceType.Soul };
    public static ResourceType[] GetAllResType()
    {
        return resourceTypes;
    }

    public static List<Type> GetAllSubclasses(Type baseType)
    {
        var subclasses = new List<Type>();
        var assembly = baseType.GetTypeInfo().Assembly;

        foreach (var type in assembly.DefinedTypes)
        {
            if (type.IsSubclassOf(baseType) && !type.IsAbstract)
            {
                subclasses.Add(type.AsType());
            }
        }
        return subclasses;
    }


    public static void AddAll<T>(this LinkedList<T> linkedList, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            linkedList.AddLast(item);
        }
    }
    public static void AddAll<T>(this List<T> list, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            list.Add(item);
        }
    }

    public static void Shuffle<T>(this LinkedList<T> list)
    {
        if (list == null || list.Count < 2)
            return;
        List<T> list2 = new(list);
        list2.Shuffle();
        list.Clear();
        list.AddAll(list2);
    }


    public static void Shuffle<T>(this IList<T> arr)
    {
        var n = arr.Count();
        while (n > 1)
        {
            n--;
            var k = random.Next(0, n + 1);
            var val = arr[k];
            arr[k] = arr[n];
            arr[n] = val;
        }
    }

    public static Board GetBoard()
    {
        return GameManager.board;
    }
    public static Player GetPlayer()
    {
        return GameManager.board.Player;
    }
}

public static class BoardUtil
{
    public static Board GetBoard(this Player obj)
    {
        return GameManager.board;
    }
    public static Board GetBoard(this Card obj)
    {
        return GameManager.board;
    }
    public static Board GetBoard(this Skill obj)
    {
        return GameManager.board;
    }
    public static Player GetPlayer(this Card obj)
    {
        return GameManager.board.Player;
    }
    public static Player GetPlayer(this Skill obj)
    {
        return GameManager.board.Player;
    }


}