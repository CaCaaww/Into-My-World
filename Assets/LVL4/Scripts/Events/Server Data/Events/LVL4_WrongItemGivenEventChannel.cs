using UnityEngine;

[CreateAssetMenu(fileName = "Wrong Item Given Event Channel", menuName = "Events / Wrong Item Given Event Channel")]
public class LVL4_WrongItemGivenEventChannel : GenericEventChannelSO<WrongItemGivenEvent> { }

public struct WrongItemGivenEvent
{
    public string token;
    public int eventType;

    public WrongItemGivenEvent(string token, int eventType)
    {
        this.token = token;
        this.eventType = eventType;
    }
}
