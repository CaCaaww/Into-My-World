using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [Header("Listening Event Channels")]
    [SerializeField] protected MinigameCompleteEventChannel minigameCompleteEventChannel;
    [SerializeField] protected CloseMinigameEventChannel closeMinigameEventChannel;
    [SerializeField] protected GenericEventChannelSO<MinigameCompletedEvent> minigameCompletedEventChannel;
    //[SerializeField] protected LVL4_MinigameQuitEventChannel minigameQuitEventChannel;

    public abstract bool IsFinished();
    public abstract void Restart();

    protected void ReturnToLevel()
    {
        closeMinigameEventChannel.RaiseEvent();

    }

    protected void ForwardMinigameCompleteData()
    {
        minigameCompletedEventChannel.RaiseEvent(
            new MinigameCompletedEvent(
                "send minigame completed event data to server",
                (int)LVL4_EventType.MinigameCompletedEvent
        ));
    }
}
