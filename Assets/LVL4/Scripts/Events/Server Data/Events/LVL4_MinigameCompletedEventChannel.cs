using UnityEngine;

[CreateAssetMenu(fileName = "Minigame Completed Event Channel", menuName = "Events / Minigame Completed Event Channel")]
public class LVL4_MinigameCompletedEventChannel : GenericEventChannelSO<MinigameCompletedEvent> { }

public struct MinigameCompletedEvent 
{
    public string token;
    public int eventType;

    public MinigameCompletedEvent(string token, int eventType)
    {
        this.token = token;
        this.eventType = eventType;
    }
}
