using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toggle Inventory Event Channel", menuName = "Events/Toggle Inventory Event Channel")]
public class ToggleInventoryEventChannel : GenericEventChannelSO<ToggleInventoryEvent>
{
    
}
[System.Serializable]
public struct ToggleInventoryEvent {
    public bool isOpen;
    public ToggleInventoryEvent(bool isOpen) { this.isOpen = isOpen;}
}
