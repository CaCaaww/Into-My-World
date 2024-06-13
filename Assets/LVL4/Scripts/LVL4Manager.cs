using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LVL4Manager : MonoBehaviour
{
    public static LVL4Manager instance;
    private KeyItem currentlyHeldItem;
    private bool inputsEnabled;

    [SerializeField]
    private PlayerInput playerInput;

    void Start()
    {
        instance = this;
        inputsEnabled = true;
    }

    void Update()
    {

    }

    public void RetryButton()
    {
        SceneManager.LoadSceneAsync("LVL4");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void PickUpItem(KeyItem item)
    {
        if (currentlyHeldItem)
        {
            currentlyHeldItem.ShowItem(true);
        }
        currentlyHeldItem = item;
        item.ShowItem(false);
    }

    public void TogglePlayerInput()
    {
        inputsEnabled = !inputsEnabled;
        if (inputsEnabled)
        {
            playerInput.currentActionMap.Enable();
        }
        else
        {
            playerInput.currentActionMap.Disable();
        }
    }

}
