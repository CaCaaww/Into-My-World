using System;

/// <summary>
/// EventData class for a WrongItemGiven event
/// </summary>
[Serializable]
public class LVL4_WrongItemGivenEventData : EventData
{
    #region Public properties
    /// <summary>
    /// Activation date for a WrongItemGiven event
    /// </summary>
    public string activationDate;
    /// <summary>
    /// Activation time for a WrongItemGiven event
    /// </summary>
    public string activationTime;
    #endregion

    #region Constructor
    /// <summary>
    /// Static method to create a LVL4_WrongItemGivenEventData instance
    /// </summary>
    /// <param name="token"></param>
    /// <param name="eventType"></param>
    /// <param name="activationDate"></param>
    /// <param name="activationTime"></param>
    /// <returns></returns>
    public LVL4_WrongItemGivenEventData(string token, int eventType, string activationDate, string activationTime)
    {
        this.token = token;
        this.eventType = eventType;
        this.activationDate = activationDate;
        this.activationTime = activationTime;
    }
    #endregion
}