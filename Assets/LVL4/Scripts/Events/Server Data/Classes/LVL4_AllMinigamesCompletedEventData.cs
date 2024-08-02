using System;

/// <summary>
/// EventData class for a AllMinigamesCompletedEvent
/// </summary>
[Serializable]
public class LVL4_AllMinigamesCompletedEventData : EventData
{
    #region Public properties
    /// <summary>
    /// Activation date for a AllMinigamesCompletedEvent event
    /// </summary>
    public string activationDate;
    /// <summary>
    /// Activation time for a AllMinigamesCompletedEvent event
    /// </summary>
    public string activationTime;
    #endregion

    #region Constructor
    /// <summary>
    /// Static method to create a LVL4_AllMinigamesCompletedEventData instance
    /// </summary>
    /// <param name="token"></param>
    /// <param name="eventType"></param>
    /// <param name="activationDate"></param>
    /// <param name="activationTime"></param>
    /// <returns></returns>
    public LVL4_AllMinigamesCompletedEventData(string token, int eventType, string activationDate, string activationTime)
    {
        this.token = token;
        this.eventType = eventType;
        this.activationDate = activationDate;
        this.activationTime = activationTime;
    }
    #endregion
}