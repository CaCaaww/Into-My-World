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
    private TMP_Text pickupText;
    [SerializeField]
    private TMP_Text itemHeldText;
    [SerializeField]
    private CanvasRenderer itemPanel;
    [SerializeField]
    private PlayerInput playerInput;
    #endregion

    #region Private Variables
    private bool inputsEnabled;
    private List<int> DOTweenIDs;
    private List<KeyItem> allKeyItems;
    #endregion

    #region Public Variables
    public KeyItem currentlyHeldItem;
    #endregion


    void Start()
    {
        instance = this;
        inputsEnabled = true;
        DOTweenIDs = new List<int>();
        DOTweenIDs.Add(-1);
        DOTweenIDs.Add(-1);
        DOTweenIDs.Add(-1);

        itemPanel.SetAlpha(0);
        itemText.alpha = 0;
        pickupText.alpha = 0;

        itemHeldText.text = "Nothing";

        
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
        itemText.text = item.itemType.ToString().Replace("_", " ");
        itemHeldText.text = itemText.text;

        for (int i = 0; i < DOTweenIDs.Count; i++)
        {
            if (DOTweenIDs[i] != -1)
                DOTween.Kill(DOTweenIDs[i]);
        }

        itemPanel.SetAlpha(1);
        DOTweenIDs[0] = DOTween.To(() => itemPanel.GetAlpha(), x => itemPanel.SetAlpha(x), 0, 4).intId;

        itemText.alpha = 1;
        DOTweenIDs[1] = DOTween.To(() => itemText.alpha, x => itemText.alpha = x, 0, 4).intId;

        pickupText.alpha = 1;
        DOTweenIDs[2] = DOTween.To(() => pickupText.alpha, x => pickupText.alpha = x, 0, 4).intId;
    }

    public void TogglePlayerInput()
    {
        inputsEnabled = !inputsEnabled;
        //Debug.Log("Toggling Input: " + inputsEnabled);
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
