using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class LVL4_DataSender : MonoBehaviour
{
    #region Inspector
    [Header("Listening Event Channels")]
    /// <summary>
    /// LVL 4 Data Event Channel Scriptable Object
    /// </summary>
    [SerializeField]
    private GenericEventChannelSO<LevelCompleteEvent> levelCompleteEventChannel;
    [SerializeField]
    private GenericEventChannelSO<CorrectItemGivenEvent> correctItemGivenEventChannel;
    [SerializeField]
    private GenericEventChannelSO<WrongItemGivenEvent> wrongItemGivenEventChannel;
    [SerializeField]
    private GenericEventChannelSO<MinigameStartedEvent> minigameStartedEventChannel;
    [SerializeField]
    private GenericEventChannelSO<MinigameCompletedEvent> minigameCompletedEventChannel;
    [SerializeField]
    private GenericEventChannelSO<AllMinigamesCompletedEvent> allMinigamesCompletedEventChannel;
    #endregion

    #region Private Variables
    /// <summary>
    /// JSON to send
    /// </summary> 
    private string json;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        levelCompleteEventChannel.OnEventRaised += ManageDataEvent;
        correctItemGivenEventChannel.OnEventRaised += ManageDataEvent;
        wrongItemGivenEventChannel.OnEventRaised += ManageDataEvent;
        minigameStartedEventChannel.OnEventRaised += ManageDataEvent;
        minigameCompletedEventChannel.OnEventRaised += ManageDataEvent;
        allMinigamesCompletedEventChannel.OnEventRaised += ManageDataEvent;
    }

    private void OnDisable()
    {
        levelCompleteEventChannel.OnEventRaised -= ManageDataEvent;
        correctItemGivenEventChannel.OnEventRaised -= ManageDataEvent;
        wrongItemGivenEventChannel.OnEventRaised -= ManageDataEvent;
        minigameStartedEventChannel.OnEventRaised -= ManageDataEvent;
        minigameCompletedEventChannel.OnEventRaised -= ManageDataEvent;
        allMinigamesCompletedEventChannel.OnEventRaised -= ManageDataEvent;
    }
    #endregion

    #region Event Listener Methods
    private void ManageDataEvent(LevelCompleteEvent evt)
    {
        if (evt.token == null) { return; }

        // Get the date and time
        string currentDate = DateTime.Now.ToString("yyyy-mm-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        // make a new object for json

        LVL4_LevelCompleteEventData eventData = new LVL4_LevelCompleteEventData(
            evt.token,
            evt.eventType,
            currentDate,
            currentTime
        );

        // Build the JSON
        json = JsonUtility.ToJson(eventData);

        Debug.Log("JSON: " + json);
    }
    
    private void ManageDataEvent(CorrectItemGivenEvent evt)
    {

        if (evt.token == null) { return; }

        // Get the date and time
        string currentDate = DateTime.Now.ToString("yyyy-mm-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        // make a new object for json
        LVL4_CorrectItemGivenEventData eventData = new LVL4_CorrectItemGivenEventData(
            evt.token,
            evt.eventType,
            currentDate,
            currentTime
        );

        // Build the JSON
        json = JsonUtility.ToJson(eventData);

        Debug.Log("JSON: " + json);

    }
    
    private void ManageDataEvent(WrongItemGivenEvent evt)
    {
        if (evt.token == null) { return; }

        // Get the date and time
        string currentDate = DateTime.Now.ToString("yyyy-mm-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        // make a new object for json
        LVL4_WrongItemGivenEventData eventData = new LVL4_WrongItemGivenEventData(
            evt.token,
            evt.eventType,
            currentDate,
            currentTime
        );

        // Build the JSON
        json = JsonUtility.ToJson(eventData);

        Debug.Log("JSON: " + json);

    }

    private void ManageDataEvent(MinigameStartedEvent evt)
    {
        if (evt.token == null) { return; }

        // Get the date and time
        string currentDate = DateTime.Now.ToString("yyyy-mm-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        // make a new object for json
        LVL4_MinigameStartedEventData eventData = new LVL4_MinigameStartedEventData(
            evt.token,
            evt.eventType,
            currentDate,
            currentTime
        );

        // Build the JSON
        json = JsonUtility.ToJson(eventData);

        Debug.Log("JSON: " + json);

    }

    private void ManageDataEvent(MinigameCompletedEvent evt)
    {
        if (evt.token == null) { return; }

        // Get the date and time
        string currentDate = DateTime.Now.ToString("yyyy-mm-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        // make a new object for json
        LVL4_MinigameCompletedEventData eventData = new LVL4_MinigameCompletedEventData(
            evt.token,
            evt.eventType,
            currentDate,
            currentTime
        );

        // Build the JSON
        json = JsonUtility.ToJson(eventData);

        Debug.Log("JSON: " + json);

    }

    private void ManageDataEvent(AllMinigamesCompletedEvent evt)
    {
        if (evt.token == null) { return; }

        // Get the date and time
        string currentDate = DateTime.Now.ToString("yyyy-mm-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        // make a new object for json
        LVL4_AllMinigamesCompletedEventData eventData = new LVL4_AllMinigamesCompletedEventData(
            evt.token,
            evt.eventType,
            currentDate,
            currentTime
        );

        // Build the JSON
        json = JsonUtility.ToJson(eventData);

        Debug.Log("JSON: " + json);

    }
    #endregion
}
