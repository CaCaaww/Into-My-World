using System;

/// <summary>
/// Base data class for building an event to send
/// </summary>
[Serializable]
public class EventData
{
    #region Public Variables
    /// <summary>
    /// The id of the current "session"
    /// </summary>
    public string token;
    /// <summary>
    /// The sent event type
    /// </summary>
    public int eventType;
    #endregion
}