using System.Collections;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEvent
{

}

public class Events
{
    private static Events eventsInstance = null;
    public static Events instance
    {
        get
        {
            if (eventsInstance == null)
            {
                eventsInstance = new Events();
            }

            return eventsInstance;
        }
    }

    public delegate void EventDelegate<T>(T e) where T : GameEvent;

    private Dictionary<System.Type, System.Delegate> delegates = new Dictionary<System.Type, System.Delegate>();

    public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        if (delegates.ContainsKey(typeof(T)))
        {
            System.Delegate tempDel = delegates[typeof(T)];

            delegates[typeof(T)] = System.Delegate.Combine(tempDel, del);
        }
        else
        {
            delegates[typeof(T)] = del;
        }
    }

    public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        if (delegates.ContainsKey(typeof(T)))
        {
            var currentDel = System.Delegate.Remove(delegates[typeof(T)], del);

            if (currentDel == null)
            {
                delegates.Remove(typeof(T));
            }
            else
            {
                delegates[typeof(T)] = currentDel;
            }
        }
    }

    public void Raise(GameEvent e)
    {
        if (e == null)
        {
            Debug.Log("Invalid event argument: " + e.GetType().ToString());
            return;
        }

        if (delegates.ContainsKey(e.GetType()))
        {
            delegates[e.GetType()].DynamicInvoke(e);
        }
    }
}
