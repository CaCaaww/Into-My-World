using UnityEngine;

[CreateAssetMenu(fileName = "Pickup Item Event Channel", menuName = "Events/Pickup Item Event Channel")]
public class PickupItemEventChannel : GenericEventChannelSO<PickupItemEvent> { }

[System.Serializable]
public struct PickupItemEvent
{
    public KeyItem item;

    public PickupItemEvent(KeyItem item)
    {
        this.item = item;
    }
}