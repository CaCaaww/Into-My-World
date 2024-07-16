using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base data class for building a data event to send to the server
/// </summary>
[Serializable]
public class LVL4_ServerData
{
    /// <summary>
    /// The id of the current "session"
    /// </summary>
    public string token;
    /// <summary>
    /// The sent event type
    /// </summary>
    public LVL4_EventType eventType;

    public  LVL4_ServerData(string token, LVL4_EventType eventType) 
    {
        this.token = token;
        this.eventType = eventType;
    }

    public virtual void Reset() { }
}
