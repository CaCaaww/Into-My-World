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
    [SerializeField]
    private Canvas gameCanvas;
    [SerializeField]
    private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    #endregion

    #region Public Methods

    /// <summary>
    /// Opens the quit menu and dims the screen
    /// </summary>
    public void RedXButtonClicked()
    {
        // Opening the quit screen
        AreYouSure.SetActive(!AreYouSure.activeSelf);
        YesButton.SetActive(!YesButton.activeSelf);
        NoButton.SetActive(!NoButton.activeSelf);
        Backdrop.SetActive(!Backdrop.activeSelf);
    }

    /// <summary>
    /// Closes the minigame and resumes first-person gameplay.
    /// </summary>
    public void YesButtonClicked()
    {
        AreYouSure.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        Backdrop.SetActive(false);

        CloseGameEventChannel.RaiseEvent(new CloseGameEvent(GetControllerInRoot()));
    }

    /// <summary>
    /// Closes the quit menu and resumes the minigame
    /// </summary>
    public void NoButtonClicked()
    {
        // Close the quit screen and continue playing the minigame
        AreYouSure.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        Backdrop.SetActive(false);
    }

    public CloseGameController GetControllerInRoot()
    {
        return this.gameObject.transform.root.GetComponent<CloseGameController>();
    }
    #endregion
}
