using UnityEngine;

[CreateAssetMenu(fileName = "Interact With Guard Event Channel", menuName = "Events/Interact With Guard Event Channel")]
public class InteractWithGuardEventChannel : GenericEventChannelSO<InteractWithGuardEvent> { }

[System.Serializable]
public struct InteractWithGuardEvent
{
    public CellGuard cellGuard;
    public KeyItem heldItem;

    public InteractWithGuardEvent(CellGuard cellGuard, KeyItem heldItem)
    {
        this.cellGuard = cellGuard;
        this.heldItem = heldItem;
    }
}
