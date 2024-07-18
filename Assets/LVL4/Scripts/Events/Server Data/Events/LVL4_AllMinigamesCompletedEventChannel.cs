using UnityEngine;

[CreateAssetMenu(fileName = "All Minigames Completed Event Channel", menuName = "Events / All Minigames Completed Event Channel")]
public class LVL4_AllMinigamesCompletedEventChannel : GenericEventChannelSO<AllMinigamesCompletedEvent> { }

public struct AllMinigamesCompletedEvent
{
    public string token;
    public int eventType;

    public AllMinigamesCompletedEvent(string token, int eventType)
    {
        this.token = token;
        this.eventType = eventType;
    }
}

