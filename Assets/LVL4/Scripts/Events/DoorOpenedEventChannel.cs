using UnityEngine;

[CreateAssetMenu(fileName = "Door Opened Event Channel", menuName = "Events/Door Opened Event Channel")]
public class DoorOpenedEventChannel : GenericEventChannelSO<DoorOpenedEvent> { }

[System.Serializable]
public struct DoorOpenedEvent
{
    public DoorController controller;
    public bool debugCall;

    public DoorOpenedEvent(DoorController controller, bool isDebug = false)
    {
        this.controller = controller;
        debugCall = isDebug;
    }
}