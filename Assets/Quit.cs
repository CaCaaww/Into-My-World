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
        // Sets the game canvas to inactivem the player will return to the level
        this.gameObject.transform.parent.gameObject.SetActive(false);
        AreYouSure.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        Backdrop.SetActive(false);

    }

    public void NoButtonClicked()
    {
        AreYouSure.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
        Backdrop.SetActive(false);
    }
}
