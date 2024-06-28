using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class LVL4UIManager : MonoBehaviour
{
    [SerializeField]
    private PickupItemEventChannel pickupItemEventChannel;
    [SerializeField]
    private ToggleDebugEventChannel toggleDebugEventChannel;
    [SerializeField]
    private DoorOpenedEventChannel doorOpenedEventChannel;
    [SerializeField]
    private AllPrisonersFreedEventChannel allPrisonersFreedEventChannel;
    [SerializeField]
    private TMP_Text itemText;
    [SerializeField]
    private TMP_Text pickupText;
    [SerializeField]
    private TMP_Text itemHeldText;
    [SerializeField]
    private CanvasRenderer itemPanel;
    [SerializeField]
    private CanvasRenderer DEBUG_PANEL;
    [SerializeField]
    private GameObject nextStageGameObject;

    private List<int> DOTweenIDs;
    private bool debugEnabled;

    private void Start()
    {
        pickupItemEventChannel.OnEventRaised += OnPickupItem;
        toggleDebugEventChannel.OnEventRaised += OnToggleDebug;
        allPrisonersFreedEventChannel.OnEventRaised += OnAllPrisonersFreed;
        itemPanel.SetAlpha(0);
        itemText.alpha = 0;
        pickupText.alpha = 0;

        itemHeldText.text = "Nothing";

        DOTweenIDs = new List<int>();
        DOTweenIDs.Add(-1);
        DOTweenIDs.Add(-1);
        DOTweenIDs.Add(-1);
    }

    private void OnAllPrisonersFreed(AllPrisonersFreedEvent evt) {
        Debug.Log("Moving to next stage");
        itemPanel.gameObject.SetActive(false);
        nextStageGameObject.SetActive(true);
    }
    private void OnPickupItem(PickupItemEvent evt)
    {
        KeyItem item = evt.item;
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

    private void OnToggleDebug(ToggleDebugEvent evt)
    {
        Debug.Log("Toggle Debug");
        debugEnabled = !debugEnabled;
        DEBUG_PANEL.gameObject.SetActive(!DEBUG_PANEL.gameObject.activeSelf);
        if (debugEnabled)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ExitDebugMenuButton() 
    {
        toggleDebugEventChannel.RaiseEvent(new ToggleDebugEvent());
    }

    public void OpenDoorsButton()
    {
        doorOpenedEventChannel.RaiseEvent(new DoorOpenedEvent(null, true));
    }
}
