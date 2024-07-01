using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private KeyItemTagsSO presetItemTags;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private int gamesNeededToComplete;
    [SerializeField]
    private PlayerTransformSO transformSO;

    [Header("Listening Event Channels")]
    [SerializeField]
    private InteractEventChannel interactEventChannel;
    [SerializeField]
    private PickupItemEventChannel pickupItemEventChannel;
    [SerializeField]
    private ToggleDebugEventChannel toggleDebugEventChannel;
    [SerializeField]
    private InteractWithGuardEventChannel interactWithGuardEventChannel;
    [SerializeField]
    private MinigameCompleteEventChannel minigameCompleteEventChannel;
    [SerializeField]
    private CloseGameEventChannel closeGameEventChannel;
    [SerializeField]
    private AllPrisonersFreedEventChannel allPrisonersFreedEventChannel;
    [SerializeField]
    private ToggleCursorEventChannel toggleCursorEventChannel;
    [SerializeField]
    private GenericEventChannelSO<GiveGuardItemEvent> GiveGuardItemEventChannel;
    [SerializeField]
    private GenericEventChannelSO<MinigameOpenedEvent> MinigameOpenedEventChannel;
    #endregion

    #region Private Variables
    private bool inputsEnabled;
    private KeyItem currentlyHeldItem;
    private int gamesCompleted = 0;
    #endregion

    #region Unity Methods
    void Start()
    {
        transformSO.Set(playerInput.gameObject.transform);

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;

        inputsEnabled = true;

        SetItemTags();
        AssignItemsToGuards();
    }

    private void OnEnable()
    {
        interactEventChannel.OnEventRaised += OnPlayerPressInteract;

        pickupItemEventChannel.OnEventRaised += OnPickupItem;

        minigameCompleteEventChannel.OnEventRaised += OnMinigameComplete;

        toggleCursorEventChannel.OnEventRaised += OnToggleCursor;

        toggleDebugEventChannel.OnEventRaised += (ToggleDebugEvent evt) => { InputEnabled(!inputsEnabled); };

        GiveGuardItemEventChannel.OnEventRaised += OnGiveGuardItem;

        MinigameOpenedEventChannel.OnEventRaised += OnMinigameOpened;

        closeGameEventChannel.OnEventRaised += OnMinigameClosed;

    }

    private void OnDisable()
    {
        interactEventChannel.OnEventRaised -= OnPlayerPressInteract;

        pickupItemEventChannel.OnEventRaised -= OnPickupItem;

        minigameCompleteEventChannel.OnEventRaised -= OnMinigameComplete;

        toggleCursorEventChannel.OnEventRaised -= OnToggleCursor;

        toggleDebugEventChannel.OnEventRaised -= (ToggleDebugEvent evt) => { InputEnabled(!inputsEnabled); };

        GiveGuardItemEventChannel.OnEventRaised -= OnGiveGuardItem;

        MinigameOpenedEventChannel.OnEventRaised -= OnMinigameOpened;

        closeGameEventChannel.OnEventRaised -= OnMinigameClosed;

    }
    private void Update()
    {
        transformSO.Set(playerInput.gameObject.transform);
    }
    #endregion

    #region Helper Methods
    private void SetItemTags()
    {
        presetItemTags.Init();
        List<KeyItem> allItems = new List<KeyItem>();
        allItems.AddRange(Object.FindObjectsOfType<KeyItem>());
        foreach (KeyItem item in allItems)
        {
            if (!presetItemTags.SetTagsOfItem(item))
            {
                Debug.LogWarning("Can not find item tags for item " + item.itemType.ToString().Replace("_", " "));
            }
        }
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
            for (int i = allItems.Count - 1; i >= 0; i--)
            {
                if (allItems[i].itemType == item1.itemType)
                {
                    allItems.RemoveAt(i);
                }
            }

            KeyItem item2 = temp[Random.Range(0, temp.Count)];
            temp.Remove(item2);
            for (int i = allItems.Count - 1; i >= 0; i--)
            {
                if (allItems[i].itemType == item2.itemType)
                {
                    allItems.RemoveAt(i);
                }
            }

            KeyItem item3 = temp[Random.Range(0, temp.Count)];
            temp.Remove(item3);
            for (int i = allItems.Count - 1; i >= 0; i--)
            {
                if (allItems[i].itemType == item3.itemType)
                {
                    allItems.RemoveAt(i);
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
    #endregion

    #region Public Methods

    /// <summary>
    /// Picks up an item and makes the item inactive
    /// </summary>
    public void OnPickupItem(PickupItemEvent evt)
    {
        KeyItem item = evt.item;
        if (currentlyHeldItem)
        {
            currentlyHeldItem.ShowItem(true);
        }
        currentlyHeldItem = item;
        item.ShowItem(false);
    }

    /// <summary>
    /// Opens all doors
    /// - Used for debugging purposes in the Unity Editor
    /// </summary>
    public void ToggleAllDoors()
    {
        List<DoorController> doors = new List<DoorController>();
        doors.AddRange(Object.FindObjectsOfType<DoorController>());
        foreach (DoorController i in doors)
        {
            i.toggleDoor();
        }
    }

    /// <summary>
    /// Toggles the input action map
    /// </summary>
    public void InputEnabled(bool enabled)
    {
        inputsEnabled = enabled;

        if (inputsEnabled)
        {
            playerInput.currentActionMap.Enable();
        }
        else
        {
            playerInput.currentActionMap.Disable();
        }
    }
    #endregion

    /// <summary>
    /// Increases minigame complete count on event trigger and possibly triggers event to send to next stage.
    /// </summary>
    private void OnMinigameComplete(MinigameCompleteEvent evt)
    {
        gamesCompleted++;
        if (gamesCompleted == gamesNeededToComplete)
        {
            allPrisonersFreedEventChannel.RaiseEvent(new AllPrisonersFreedEvent());
            InputEnabled(true);
            //toggleCursorEventChannel.RaiseEvent(new ToggleCursorEvent());
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void OnToggleCursor(ToggleCursorEvent evt)
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void OnPlayerPressInteract(InteractEvent evt)
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 forwardDirection = mainCamera.transform.forward;

        RaycastHit hit;
        // float maxDistance = 1.2F;
        if (Physics.SphereCast(cameraPosition, 0.3f, forwardDirection, out hit, 5.0F))
        {
            // If the ray hits something, you can access the hit information
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponent<CellGuard>())
            {
                interactWithGuardEventChannel.RaiseEvent(new InteractWithGuardEvent(hit.collider.gameObject.GetComponent<CellGuard>(), currentlyHeldItem));
            }
            if (hit.collider.gameObject.CompareTag("JailCell"))
            {
                hit.collider.gameObject.GetComponent<MiniGameController>().lockClicked();
            }

            if (hit.collider.gameObject.GetComponent<KeyItem>())
            {
                if (hit.collider.gameObject.GetComponent<KeyItem>().CanPickUp())
                {
                    pickupItemEventChannel.RaiseEvent(new PickupItemEvent(hit.collider.gameObject.GetComponent<KeyItem>()));
                }
            }
            else if (hit.collider.gameObject.GetComponentInParent<KeyItem>())
            {
                if (hit.collider.gameObject.GetComponentInParent<KeyItem>().CanPickUp())
                {
                    pickupItemEventChannel.RaiseEvent(new PickupItemEvent(hit.collider.gameObject.GetComponentInParent<KeyItem>()));
                }
            }
        }
    }

    public void OnGiveGuardItem(GiveGuardItemEvent evt)
    {
        if (!evt.isCorrectItem)
        {
            currentlyHeldItem.ShowItem(true);
        }
        currentlyHeldItem = null;
    }

    public void OnMinigameOpened(MinigameOpenedEvent evt)
    {
        InputEnabled(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnMinigameClosed(CloseGameEvent evt)
    {
        InputEnabled(true);
    }
}
