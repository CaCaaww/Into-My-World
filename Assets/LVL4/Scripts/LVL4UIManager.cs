using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject itemDisplays;
    [SerializeField]
    private CanvasRenderer DEBUG_PANEL;
    [SerializeField]
    private GameObject nextStageGameObject;
    [SerializeField]
    private GameObject finalStageGameObject;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject VictoryScreen;
    [SerializeField]
    private Image neutralCrosshair, interactCrosshair, talkCrosshair;
    [SerializeField]
    private GameObject inventoryGameObject;
    [SerializeField] private List<TextMeshProUGUI> questItems;

    [SerializeField]
    private PlayerDataSO playerData;

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
    [SerializeField]
    private GenericEventChannelSO<FinalStageFailedEvent> finalStageFailedEventChannel;
    [SerializeField]
    private GenericEventChannelSO<ToggleInventoryEvent> toggleInventoryEventChannel;
    [SerializeField]
    private GenericEventChannelSO<QuestGivenEvent> questGivenEventChannel;
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
        questItems[0].text = "";
        questItems[1].text = "";
        questItems[2].text = "";
    }

    private void Update()
    {
        // Updating the crosshair
        neutralCrosshair.enabled = true;
        interactCrosshair.enabled = false;
        talkCrosshair.enabled = false;

        if (playerData.LookingAt)
        {
            if (playerData.LookingAt.GetComponent<CellGuard>())
            {
                talkCrosshair.enabled = true;
                neutralCrosshair.enabled = false;
            }

            if (playerData.LookingAt.CompareTag("JailCell"))
            {
                interactCrosshair.enabled = true;
                neutralCrosshair.enabled = false;
            }

            if (playerData.LookingAt.GetComponent<KeyItem>())
            {
                if (playerData.LookingAt.GetComponent<KeyItem>().CanPickUp())
                {
                    interactCrosshair.enabled = true;
                    neutralCrosshair.enabled = false;
                }
            }
            else if (playerData.LookingAt.GetComponentInParent<KeyItem>())
            {
                if (playerData.LookingAt.GetComponentInParent<KeyItem>().CanPickUp())
                {
                    interactCrosshair.enabled = true;
                    neutralCrosshair.enabled = false;
                }
            }
        }
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
        finalStageFailedEventChannel.OnEventRaised += OnFinalStageFailed;
        toggleInventoryEventChannel.OnEventRaised += OnToggleInventory;
        questGivenEventChannel.OnEventRaised += OnQuestGiven;
    }

    private void OnDisable()
    {
        openNextStageEventChannel.OnEventRaised -= OnOpenNextStage;
        pickupItemEventChannel.OnEventRaised -= OnPickupItem;
        toggleDebugEventChannel.OnEventRaised -= OnToggleDebug;
        allPrisonersFreedEventChannel.OnEventRaised -= OnAllPrisonersFreed;
        gameOverEventChannel.OnEventRaised -= OnGameOver;
        giveGuardItemEventChannel.OnEventRaised -= OnGiveGuardItem;
        finalStageCompleteEventChannel.OnEventRaised -= OnFinalStageComplete;
        finalStageFailedEventChannel.OnEventRaised -= OnFinalStageFailed;
        toggleInventoryEventChannel.OnEventRaised -= OnToggleInventory;
        questGivenEventChannel.OnEventRaised -= OnQuestGiven;
    }

    public void OnQuestGiven(QuestGivenEvent evt) {
        for (int i = 0; i < 3; i++) {
            if (evt.questItems[i] != null) {
                questItems[i].enabled = true;
                questItems[i].SetText(evt.questItems[i]);
            } else {
                questItems[i].enabled = false;
            }
        }
    }
    private void OnToggleInventory(ToggleInventoryEvent evt) {
        inventoryGameObject.SetActive(evt.isOpen);
    }
    private void OnOpenNextStage()
    {
        nextStageGameObject.SetActive(false);
        finalStageGameObject.SetActive(true);
    }

    private void OnAllPrisonersFreed()
    {
        Debug.Log("Moving to next stage");
        itemDisplays.gameObject.SetActive(false);
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
    /// Exits level 4
    /// </summary>
    public void ExitButton()
    {
        // Needs to be implemented to return player to start of the game
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
        itemDisplays.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnFinalStageComplete(FinalStageCompleteEvent evt)
    {
        finalStageGameObject.SetActive(false);
        VictoryScreen.SetActive(true);
    }
    private void OnFinalStageFailed(FinalStageFailedEvent evt) 
    {
        gameOverPanel.SetActive(true);
        itemDisplays.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenNextStageButton()
    {
        openNextStageEventChannel.RaiseEvent();

        /* ========================== SEND DATA TO SERVER HERE ==============================*/
    }

}

