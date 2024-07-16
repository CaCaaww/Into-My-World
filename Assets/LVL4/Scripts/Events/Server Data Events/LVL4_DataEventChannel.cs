using UnityEngine;

[CreateAssetMenu(fileName = "LVL4_DataEventChannel", menuName = "Events / Data Event Channel")]
public class LVL4_DataEventChannel : GenericEventChannelSO<DataEvent> { }

[System.Serializable]
public struct DataEvent
{
    public string token;
    public LVL4_EventType eventType;
    public DataEvent(LVL4_ServerData serverData)
    {
        this.token = serverData.token;
        this.eventType = serverData.eventType;
    }
}
