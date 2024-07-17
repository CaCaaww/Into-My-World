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
    }

    private void OnDisable()
    {
        levelCompleteEventChannel.OnEventRaised -= ManageDataEvent;
        correctItemGivenEventChannel.OnEventRaised -= ManageDataEvent;
    }
    #endregion

    #region Helper methods
    /// <summary>
    /// Build and send a JSON for a game start event to the remote webapp server
    /// </summary> 
    /*private void ForwardLevelCompleteData(args)
    {
        // Build the JSON
        json = JsonUtility.ToJson();

        Debug.Log("JSON: " + json);

        // Set the remote host to the LVL3 action gate URL
        remoteHost = webAppConsoleData.LVL2ActionGate;

        // Start the coroutine to post the JSON to the webapp server
        StartCoroutine(PostRequestCO(remoteHost, json));
    }*/
    #endregion

    #region Event Listener Methods
    private void ManageDataEvent(LevelCompleteEvent evt)
    {
        if (evt.token == null) { return; }
        
        Debug.Log(evt.token);

        // Get the date and time
        string currentDate = DateTime.Now.ToString("yyyy-mm-dd");
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        // make a new object for json

        // LVL4_LevelCompleteEventData = new LVL4_LevelCompleteEventData()
        // LVL4_LevelCompleteEventData.activationTime = currentTime;

        // Build the JSON
        //json = JsonUtility.ToJson();

        //Debug.Log("JSON: " + json);
    }
    
    private void ManageDataEvent(CorrectItemGivenEvent evt)
    {
        if (evt.token == null) { return; }
        
        Debug.Log(evt.token);
            
    }
    #endregion
}
