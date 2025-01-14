using UnityEngine;

[CreateAssetMenu(fileName = "Minigame Complete Event Channel", menuName = "Events/Minigame Complete Event Channel")]
public class MinigameCompleteEventChannel : GenericEventChannelSO<MinigameCompleteEvent> { }

[System.Serializable]
public struct MinigameCompleteEvent
{
    public Minigame game;
    public bool isDebug;

    public MinigameCompleteEvent(Minigame game, bool isDebug = false)
    {
        this.game = game;
        this.isDebug = isDebug;
    }
}

