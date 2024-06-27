using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Minigame Opened Event Channel", menuName = "Events/Minigame Opened Event Channel")]
public class MinigameOpenedEventChannel : GenericEventChannelSO<MinigameOpenedEvent>
{
   
}
[System.Serializable]
public struct MinigameOpenedEvent {
    public MiniGameController controller;

    public MinigameOpenedEvent(MiniGameController controller) {
        this.controller = controller;
    }

}