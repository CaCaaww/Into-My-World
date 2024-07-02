using UnityEngine;

[CreateAssetMenu(fileName = "Minigame Opened Event Channel", menuName = "Events/Minigame Opened Event Channel")]
public class MinigameOpenedEventChannel : GenericEventChannelSO<MinigameOpenedEvent> { }

[System.Serializable]
public struct MinigameOpenedEvent
{
    public MinigameController controller;

    public MinigameOpenedEvent(MinigameController controller)
    {
        this.controller = controller;
    }
}