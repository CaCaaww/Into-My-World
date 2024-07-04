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
    private CloseMinigameEventChannel closeGameEventChannel;
    [SerializeField]
    private AllPrisonersFreedEventChannel allPrisonersFreedEventChannel;
    [SerializeField]
    private GiveGuardItemEventChannel giveGuardItemEventChannel;
    [SerializeField]
    private MinigameOpenedEventChannel minigameOpenedEventChannel;
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

        toggleDebugEventChannel.OnEventRaised += OnToggleDebug;

        giveGuardItemEventChannel.OnEventRaised += OnGiveGuardItem;

        minigameOpenedEventChannel.OnEventRaised += OnMinigameOpened;

        closeGameEventChannel.OnEventRaised += OnMinigameClosed;

    }

    private void OnDisable()
    {
        interactEventChannel.OnEventRaised -= OnPlayerPressInteract;

        pickupItemEventChannel.OnEventRaised -= OnPickupItem;

        minigameCompleteEventChannel.OnEventRaised -= OnMinigameComplete;

        toggleDebugEventChannel.OnEventRaised -= OnToggleDebug;

        giveGuardItemEventChannel.OnEventRaised -= OnGiveGuardItem;

        minigameOpenedEventChannel.OnEventRaised -= OnMinigameOpened;

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
    /// Toggles the input action map
    /// </summary>
    public void InputEnabled(bool enabled)
    {
        inputsEnabled = enabled;
        Debug.Log("inputsEnabled = " + inputsEnabled);

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

    #region Listener Methods
    /// <summary>
    /// Increases minigame complete count on event trigger and possibly triggers event to send to next stage.
    /// </summary>
    private void OnMinigameComplete(MinigameCompleteEvent evt)
    {
        gamesCompleted++;

        if (evt.isDebug)
        {
            gamesCompleted = gamesNeededToComplete;
        }

        if (gamesCompleted >= gamesNeededToComplete)
        {
            allPrisonersFreedEventChannel.RaiseEvent();

            /* ========================== SEND DATA TO SERVER HERE ==============================*/

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnPlayerPressInteract()
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
                /* ========================== SEND DATA TO SERVER HERE ==============================*/
            }
            if (hit.collider.gameObject.CompareTag("JailCell"))
            {
                // We can probably use an event here to lower coupling
                hit.collider.gameObject.GetComponent<MinigameController>().LockClicked();
                /* ========================== SEND DATA TO SERVER HERE ==============================*/
            }

            if (hit.collider.gameObject.GetComponent<KeyItem>())
            {
                if (hit.collider.gameObject.GetComponent<KeyItem>().CanPickUp())
                {
                    pickupItemEventChannel.RaiseEvent(new PickupItemEvent(hit.collider.gameObject.GetComponent<KeyItem>()));
                    /* ========================== SEND DATA TO SERVER HERE ==============================*/
                }
            }
            else if (hit.collider.gameObject.GetComponentInParent<KeyItem>())
            {
                if (hit.collider.gameObject.GetComponentInParent<KeyItem>().CanPickUp())
                {
                    pickupItemEventChannel.RaiseEvent(new PickupItemEvent(hit.collider.gameObject.GetComponentInParent<KeyItem>()));
                    /* ========================== SEND DATA TO SERVER HERE ==============================*/
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

    private void OnMinigameClosed()
    {
        if (gamesCompleted < gamesNeededToComplete)
        {
            InputEnabled(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnToggleDebug()
    {
        InputEnabled(!inputsEnabled);
    }

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
    #endregion
}
