using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitScreen : MonoBehaviour
{
    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO CloseGameEventChannel;
    [SerializeField] private GenericEventChannelSO<QuitButtonEvent> quitButtonEventChannel;

    void Start(){
        quitButtonEventChannel.OnEventRaised += OnQuitButton;
    }
    public void OnQuitButton(QuitButtonEvent evt) {
        ToggleQuitScreen();
    }
    public void YesButtonClicked() {
        ToggleQuitScreen();
        CloseGameEventChannel.RaiseEvent();
    }
    public void NoButtonClicked() {
        ToggleQuitScreen();
    }
    public void ToggleQuitScreen() {
        //Debug.Log("Toggle Quit Screen");
        this.GetComponent<Canvas>().enabled = !this.GetComponent<Canvas>().enabled;
        //Debug.Log(this.GetComponent<Canvas>().enabled);
    }
}
