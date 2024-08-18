using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public const string OnDraggableHover = "draggable.hover";

    public static void SubscribeTo<T>(string eventName, Action<T> action)
    {
        GameEvents<T>.SubscribeTo(eventName, action);
    }

    public static void Raise<T>(string eventName, T parameter)
    {
        GameEvents<T>.Raise(eventName, parameter);
    }
}

public static class GameEvents<T>
{

    private static Dictionary<string, List<Action<T>>> Subscribers = new Dictionary<string, List<Action<T>>>();

    internal static void Raise(string eventName, T status)
    {
        var list = GetSubcribersList(eventName);
        foreach (var action in list)
        {
            try
            {
                action(status);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    internal static void SubscribeTo(string eventName, Action<T> action)
    {
        var list = GetSubcribersList(eventName);
        list.Add(action);
    }

    private static List<Action<T>> GetSubcribersList(string eventName)
    {
        if (!Subscribers.ContainsKey(eventName))
        {
            Subscribers[eventName] = new List<Action<T>>();
        }

        return Subscribers[eventName];
    }
}
