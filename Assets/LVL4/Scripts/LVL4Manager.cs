using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LVL4Manager : MonoBehaviour
{
    public static LVL4Manager instance;
    private KeyItem currentlyHeldItem;

    void Start()
    {
        instance = this;
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

}
