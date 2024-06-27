using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Door Opened Event Channel", menuName = "Events/Door Opened Event Channel")]
public class DoorOpenedEventChannel : GenericEventChannelSO<DoorOpenedEvent> {
}
[System.Serializable]
public struct DoorOpenedEvent {
    public DoorController controller;
    public DoorOpenedEvent(DoorController controller) {
        this.controller = controller;
    }
}