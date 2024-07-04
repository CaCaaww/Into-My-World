using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LVL4UIManager : MonoBehaviour
{
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
    private CanvasRenderer DEBUG_PANEL;
    [SerializeField]
    private GameObject nextStageGameObject;
    [SerializeField]
    private GameObject finalStageGameObject;
    [SerializeField]
    private GameObject gameOverPanel;

    [Header("Listening Event Channels")]
    [SerializeField]
    private PickupItemEventChannel pickupItemEventChannel;
    [SerializeField]
    private ToggleDebugEventChannel toggleDebugEventChannel;
    [SerializeField]
    private DoorOpenedEventChannel doorOpenedEventChannel;
    [SerializeField]
    private AllPrisonersFreedEventChannel allPrisonersFreedEventChannel;
    [SerializeField]
    private GameOverEventChannel gameOverEventChannel;
    [SerializeField]
    private GiveGuardItemEventChannel giveGuardItemEventChannel;
    [SerializeField]
    private OpenNextStageEventChannel openNextStageEventChannel;
    [SerializeField]
    private MinigameCompleteEventChannel minigameCompleteEventChannel;
    [SerializeField]
    private GenericEventChannelSO<FinalStageCompleteEvent> finalStageCompleteEventChannel;
    #endregion

    #region Private Variables
    private List<int> DOTweenIDs;
    private bool debugEnabled;
    #endregion

    private void Start()
    {
        itemPanel.SetAlpha(0);
        itemText.alpha = 0;
        pickupText.alpha = 0;

        itemHeldText.text = "Nothing";

        DOTweenIDs = new List<int>();
        DOTweenIDs.Add(-1);
        DOTweenIDs.Add(-1);
        DOTweenIDs.Add(-1);
    }

    private void OnEnable()
    {
        openNextStageEventChannel.OnEventRaised += OnOpenNextStage;
        pickupItemEventChannel.OnEventRaised += OnPickupItem;
        toggleDebugEventChannel.OnEventRaised += OnToggleDebug;
        allPrisonersFreedEventChannel.OnEventRaised += OnAllPrisonersFreed;
        gameOverEventChannel.OnEventRaised += OnGameOver;
        giveGuardItemEventChannel.OnEventRaised += OnGiveGuardItem;
        finalStageCompleteEventChannel.OnEventRaised += OnFinalStageComplete;
    }

    private void OnDisable()
    {
        openNextStageEventChannel.OnEventRaised -= OnOpenNextStage;
        pickupItemEventChannel.OnEventRaised -= OnPickupItem;
        toggleDebugEventChannel.OnEventRaised -= OnToggleDebug;
        allPrisonersFreedEventChannel.OnEventRaised -= OnAllPrisonersFreed;
        gameOverEventChannel.OnEventRaised -= OnGameOver;
        giveGuardItemEventChannel.OnEventRaised -= OnGiveGuardItem;
    }

    private void OnOpenNextStage()
    {
        finalStageGameObject.SetActive(true);
    }

    private void OnAllPrisonersFreed()
    {
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

    private void OnToggleDebug()
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

    /// <summary>
    /// Restarts level 4
    /// </summary>
    public void RetryButton()
    {
        SceneManager.LoadSceneAsync("LVL4");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitDebugMenuButton()
    {
        toggleDebugEventChannel.RaiseEvent();
    }

    public void OpenDoorsButton()
    {
        doorOpenedEventChannel.RaiseEvent(new DoorOpenedEvent(null, true));

        /* ========================== SEND DATA TO SERVER HERE ==============================*/
    }

    public void CompleteMinigamesButton()
    {
        minigameCompleteEventChannel.RaiseEvent(new MinigameCompleteEvent(null, true));
    }

    public void OnGiveGuardItem(GiveGuardItemEvent evt)
    {
        itemHeldText.text = "Nothing";
    }

    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnFinalStageComplete(FinalStageCompleteEvent evt) { 
    
    }

    public void OpenNextStageButton()
    {
        openNextStageEventChannel.RaiseEvent();

        /* ========================== SEND DATA TO SERVER HERE ==============================*/
    }

}

