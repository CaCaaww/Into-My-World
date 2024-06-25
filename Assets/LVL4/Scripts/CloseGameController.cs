using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGameController : MonoBehaviour
{
    #region Inspector

    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        CloseGameEventChannel.OnEventRaised += OnConfirmation;
    }
    #endregion

    #region Callbacks
    /// <summary>
    /// Brings the player back to first-person gameplay
    /// </summary>
    public void OnConfirmation(CloseGameEvent evt)
    {
        if (evt.controller == null) return;
        if (evt.controller == this) { CloseMinigame(); }
    }
    #endregion

    #region Helper Methods

    public void CloseMinigame()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;

        // Enable player inputs
        LVL4Manager.instance.TogglePlayerInput(true);

        // Returning to the level, hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    #endregion
}
