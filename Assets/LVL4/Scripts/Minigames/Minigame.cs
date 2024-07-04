using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [Header("Listening Event Channels")]
    [SerializeField] protected MinigameCompleteEventChannel minigameCompleteEventChannel;
    [SerializeField] protected CloseMinigameEventChannel closeMinigameEventChannel;

    public abstract bool IsFinished();
    public abstract void Restart();

    public void ReturnToLevel()
    {
        closeMinigameEventChannel.RaiseEvent();

        /* ========================== SEND DATA TO SERVER HERE ==============================*/
    }
}
