using UnityEngine;

[CreateAssetMenu(fileName = "Correct Item Given Event Channel", menuName = "Events / Correct Item Given Event")]
public class LVL4_CorrectItemGivenEventChannel : GenericEventChannelSO<CorrectItemGivenEvent> { }

public struct CorrectItemGivenEvent
{
    public string token;
    public int eventType;

    public CorrectItemGivenEvent(string token, int eventType)
    {
        this.token = token;
        this.eventType = eventType;
    }
}
