using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using DG.Tweening;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LVL4Manager : MonoBehaviour
{
    [HideInInspector]
    public static LVL4Manager instance;

    #region Inspector
    [SerializeField]
    private TMP_Text itemText;
    [SerializeField]
    private CanvasRenderer itemPanel;
    [SerializeField]
    private PlayerInput playerInput;
    #endregion

    #region Private Variables
    private KeyItem currentlyHeldItem;
    private bool inputsEnabled;
    #endregion


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
        itemPanel.gameObject.SetActive(true);
        itemText.text = item.itemType.ToString();
        itemPanel.SetAlpha(1);
        DOTween.To(()=>itemPanel.GetAlpha(), x=>itemPanel.SetAlpha(x), 0, 4);
        // foreach(TMP_Text i in itemPanel.GetComponentsInChildren<TMP_Text>())
        // {
        //     DOTween.To(()=>i.color.a, x=>i.color.a, 0, 4);
        // }
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
