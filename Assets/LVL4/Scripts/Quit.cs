using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{

    [SerializeField]
    private GameObject RedButton;
    [SerializeField]
    private GameObject AreYouSure;
    [SerializeField]
    private GameObject YesButton;
    [SerializeField]
    private GameObject NoButton;
    [SerializeField]
    private GameObject Backdrop;

    public void RedXButtonClicked()
    {
        AreYouSure.SetActive(!AreYouSure.activeSelf);
        YesButton.SetActive(!YesButton.activeSelf);
        NoButton.SetActive(!NoButton.activeSelf);
        Backdrop.SetActive(!Backdrop.activeSelf);
    }

    public void YesButtonClicked()
    {
        // Sets the game canvas to inactive, the player will return to the level
        AreYouSure.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        Backdrop.SetActive(false);
        this.gameObject.transform.parent.gameObject.SetActive(false);
        LVL4Manager.instance.TogglePlayerInput();

    }

    public void NoButtonClicked()
    {
        AreYouSure.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        Backdrop.SetActive(false);
    }
}
