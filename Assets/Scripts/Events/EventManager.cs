using System;
using System.Collections.Generic;

public enum EventTypes
{
    PauseMenuClosedExternally,
    PlayerDeath,
    Victory,
    SHROOLOO
}

// Meant to be used for globally-notified events that don't require data to be passed
public class EventManager : LLPersistentSingleton<EventManager>
{
    private static Dictionary<EventTypes, Action> subscriberDict = new Dictionary<EventTypes, Action>();



    public void Subscribe(EventTypes eventType, Action listener)
    {
        Action existingListeners = null;

        // If eventType has listeners already in subscriberDict
        if (subscriberDict.TryGetValue(eventType, out existingListeners))
        {
            // Add new listener to existingListeners
            existingListeners += listener;

            // Update subscriberDict
            subscriberDict[eventType] = existingListeners;
        }

        // If eventType has no listeners in subscriberDict
        else
        {
            // Add event to subscriberDict
            existingListeners += listener;
            subscriberDict.Add(eventType, existingListeners);
        }
    }

    public void Unsubscribe(EventTypes eventType, Action listener)
    {
        // If EventManager is already destroyed, no reason to unsubscribe
        if (instance == null) return;

        Action existingListeners = null;

        // If eventType has listeners already in subscriberDict
        if (subscriberDict.TryGetValue(eventType, out existingListeners))
        {
            // Remove listener from existingListeners
            existingListeners -= listener;

            // Update subscriberDict
            subscriberDict[eventType] = existingListeners;
        }
    }

    public void Notify(EventTypes eventType)
    {
        // If eventType is in the subscriberDict, invoke all listeners of that eventType
        Action existingListeners = null;
        if (subscriberDict.TryGetValue(eventType, out existingListeners) && existingListeners != null)
        {
            existingListeners.InvokeNullCheck();
        }
    }
}