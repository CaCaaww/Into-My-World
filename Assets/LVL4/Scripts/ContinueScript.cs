using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContinueScript : MonoBehaviour
{
    [SerializeField] private GenericEventChannelSO<MinigameCompleteEvent> minigameCompleteEventChannel;
    [SerializeField] public CloseMinigameEventChannel closeMinigameEventChannel;
    
    private void OnEnable()
    {
        minigameCompleteEventChannel.OnEventRaised += onMinigameComplete;
    }
    private void OnDisable() {
        minigameCompleteEventChannel.OnEventRaised -= onMinigameComplete;
    }

    public void onMinigameComplete(MinigameCompleteEvent evt) {
        if (!evt.isDebug) {
            this.gameObject.GetComponent<Canvas>().enabled = true;
        }
        
    }
    public void continueButtonClicked() {
        closeMinigameEventChannel.RaiseEvent();
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }
}
