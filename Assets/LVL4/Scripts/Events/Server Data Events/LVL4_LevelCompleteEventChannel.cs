using UnityEngine;

[CreateAssetMenu(fileName = "Level Complete Event Channel", menuName = "Events / Level Complete Event")]
public class LVL4_LevelCompleteEvent : GenericEventChannelSO<LevelCompleteEvent> { }

public struct LevelCompleteEvent
{
    public string token;
    public int eventType;

    public LevelCompleteEvent(string token, int eventType)
    {
        this.token = token;
        this.eventType = eventType;
    }
}
