using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minigame Complete Event Channel", menuName = "Events/Minigame Complete Event Channel")]
public class MinigameCompleteEventChannel : GenericEventChannelSO<MinigameCompleteEvent> {

}
[System.Serializable]
public struct MinigameCompleteEvent {
    public MiniGameController controller;
    public MinigameCompleteEvent(MiniGameController controller) {
        this.controller = controller;
    }
}

