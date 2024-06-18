using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using DG.Tweening;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

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
    #endregion

    #region Public Variables
    [HideInInspector]
    public KeyItem currentlyHeldItem;
    public GameObject playerCapsule;
    #endregion


    void Start()
    {
        instance = this;
        inputsEnabled = true;

        AssignItemsToGuards();

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

    private void AssignItemsToGuards()
    {
        List<KeyItem> allItems = new List<KeyItem>();
        allItems.AddRange(Object.FindObjectsOfType<KeyItem>());
        List<CellGuard> allCellGuards = new List<CellGuard>();
        allCellGuards.AddRange(Object.FindObjectsOfType<CellGuard>());

        foreach (CellGuard guard in allCellGuards)
        {
            List<KeyItem> temp = new List<KeyItem>();
            temp.AddRange(allItems);

            KeyItem item1 = temp[Random.Range(0, temp.Count)];
            temp.Remove(item1);
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                if (temp[i].itemType == item1.itemType)
                {
                    temp.RemoveAt(i);
                }
            }

            KeyItem item2 = temp[Random.Range(0, temp.Count)];
            temp.Remove(item2);
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                if (temp[i].itemType == item2.itemType)
                {
                    temp.RemoveAt(i);
                }
            }

            KeyItem item3 = temp[Random.Range(0, temp.Count)];
            temp.Remove(item3);
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                if (temp[i].itemType == item3.itemType)
                {
                    temp.RemoveAt(i);
                }
            }

            guard.SetItems(item1, item2, item3);
            Debug.Log("Set guard with items: "
            + item1.itemType.ToString().Replace("_", " ") + ", "
            + item2.itemType.ToString().Replace("_", " ") + ", "
            + item3.itemType.ToString().Replace("_", " ")
            );
        }
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

    public void ItemWasCorrect()
    {
        currentlyHeldItem = null;
        itemHeldText.text = "Nothing";
    }

    public void ItemWasIncorrect()
    {

    }

    public void ToggleDebug()
    {
        Debug.Log("Opening Debug Menu");
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
