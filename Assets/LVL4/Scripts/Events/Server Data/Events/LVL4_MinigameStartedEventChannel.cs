using UnityEngine;

[CreateAssetMenu(fileName = "Minigame Started Event Channel", menuName = "Events / Minigame Started Event Channel")]
public class LVL4_MinigameStartedEventChannel : GenericEventChannelSO<MinigameStartedEvent> { }

public struct MinigameStartedEvent
{
    public string token;
    public int eventType;

    public MinigameStartedEvent(string token, int eventType)
    {
        this.token = token;
        this.eventType = eventType;
    }
}