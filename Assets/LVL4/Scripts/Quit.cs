using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{

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

    public void RedXButtonClicked()
    {
        // Opening the quit screen
        AreYouSure.SetActive(!AreYouSure.activeSelf);
        YesButton.SetActive(!YesButton.activeSelf);
        NoButton.SetActive(!NoButton.activeSelf);
        Backdrop.SetActive(!Backdrop.activeSelf);
    }

    public void YesButtonClicked()
    {
        AreYouSure.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        Backdrop.SetActive(false);
        
        // Using .enable and .disable instead of gameObject.SetActive because
        // of issues with reactiviting children when the parent gameObject is
        // activated
        if (gameCanvas != null) {
            gameCanvas.enabled = false;
        }

        // Enable
        LVL4Manager.instance.TogglePlayerInput();

        // Returning to the level, hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void NoButtonClicked()
    {
        // Close the quit screen and continue playing the minigame
        AreYouSure.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        Backdrop.SetActive(false);
    }
}
