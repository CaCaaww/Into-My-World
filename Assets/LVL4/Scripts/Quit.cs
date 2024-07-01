using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    #region Inspector
    [Tooltip("The Red X in the top left corner")]
    [SerializeField]
    private GameObject RedButton;
    [Tooltip("Confirmation page")]
    [SerializeField]
    private GameObject AreYouSure;
    [Tooltip("Yes button")]
    [SerializeField]
    private GameObject YesButton;
    [Tooltip("No button")]
    [SerializeField]
    private GameObject NoButton;
    [Tooltip("The dimmed screen")]
    [SerializeField]
    private GameObject Backdrop;
    [Tooltip("Canvas the game is played on")]

    [Header("Listening Event Channels")]
    [SerializeField]
    private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    #endregion

    #region Public Methods

    /// <summary>
    /// Opens the quit menu and dims the screen
    /// </summary>
    public void RedXButtonClicked()
    {
        ToggleQuitScreen();
    }

    /// <summary>
    /// Closes the minigame and resumes first-person gameplay.
    /// </summary>
    public void YesButtonClicked()
    {
        ToggleQuitScreen();

        CloseGameEventChannel.RaiseEvent(new CloseGameEvent());
    }

    #endregion

    #region Helper Methods
    private void ToggleQuitScreen()
    {
        AreYouSure.SetActive(!AreYouSure.activeSelf);
        YesButton.SetActive(!YesButton.activeSelf);
        NoButton.SetActive(!NoButton.activeSelf);
        Backdrop.SetActive(!Backdrop.activeSelf);
    }
    #endregion
}
