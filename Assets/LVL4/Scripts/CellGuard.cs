using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CellGuard : MonoBehaviour
{
    private enum CellGuardState
    {
        HasNotTalkedToPlayer,
        TalkedToPlayer,
        AskingForItems,
        IncorrectItem,
        AllItemsFound,
        CorrectItemFound,
        PlayerIsBusy
    }
    [SerializeField] private GameObject player;

    #region Inspector
    [SerializeField, TextArea]
    private string introDialogue;
    [SerializeField, TextArea]
    private string itemCheckDialogue;
    [SerializeField, TextArea]
    private string completeDialogue;
    [SerializeField, TextArea]
    private string angryDialogue;
    [SerializeField, TextArea]
    private string correctItemDialogue;
    [SerializeField, TextArea]
    private string playerIsBusyDialogue;
    [SerializeField]
    private List<GameObject> cellGuardModels;
    [SerializeField]
    private PlayerDataSO playerData;
    [SerializeField]
    private TMP_Text guardText;
    [SerializeField]
    private SpriteRenderer textBackground;
    [SerializeField, Tooltip("Face textures for the cell guard")]
    private Texture2D angryFace, neutralFace, happyFace;
    [SerializeField, Tooltip("Icons to go above the guard's head")]
    private GameObject iconX, iconExclamation, iconCheck;
    [SerializeField]
    private float baseInteractionCooldown;
    [SerializeField]
    private float angryInteractionCooldown;
    [SerializeField]
    private float correctItemTextTime;
    [SerializeField]
    private float textAlphaFalloffDistance;
    [SerializeField, Tooltip("The guard looks at the player if the player is in this range")]
    private float aggroRange;

    [Header("Listening Event Channels")]
    [SerializeField]
    private DoorOpenedEventChannel DoorOpenedEventChannel;
    [SerializeField]
    private InteractWithGuardEventChannel interactWithGuardEventChannel;
    [SerializeField]
    private GiveGuardItemEventChannel GiveGuardItemEventChannel;
    [SerializeField]
    private GenericEventChannelSO<CorrectItemGivenEvent> correctItemGivenEventChannel;
    [SerializeField]
    private GenericEventChannelSO<WrongItemGivenEvent> wrongItemGivenEventChannel;
    [SerializeField]
    private GenericEventChannelSO<QuestGivenEvent> questGivenEventChannel;
    #endregion

    #region Private Variables
    private CellGuardState cellGuardState;
    private MeshRenderer facePlate;
    private float interactionCooldownTimer;
    private List<KeyItem> items;
    private string[] questItems = new string[3];
    private GameObject model;
    //private static bool questAccepted;
    private bool thisGuardIsQuest;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //questAccepted = false;
        thisGuardIsQuest = false;
        interactWithGuardEventChannel.OnEventRaised += OnInteract;

        cellGuardState = CellGuardState.HasNotTalkedToPlayer;

        model = Instantiate(cellGuardModels[Random.Range(0, cellGuardModels.Count)], this.transform);
        foreach (MeshRenderer i in model.GetComponentsInChildren<MeshRenderer>())
        {
            if (i.gameObject.name.Contains("Face_Plate"))
            {
                facePlate = i;
                break;
            }
        }
        facePlate.material.mainTexture = neutralFace;

        guardText.text = "";

        GetComponent<Animator>().Rebind();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(model.transform.position, playerData.Transform.position) < aggroRange)
        {
            GetComponentInChildren<LVL4_GuardLookAt>().lookAt = true;
        }
        else
        {
            GetComponentInChildren<LVL4_GuardLookAt>().lookAt = false;
        }

        interactionCooldownTimer += Time.deltaTime;

        float distance = Vector3.Distance(this.transform.position, playerData.Transform.position);
        float alpha = 1 - ((distance - textAlphaFalloffDistance / 2) / (textAlphaFalloffDistance / 2));
        alpha = Mathf.Clamp(alpha, 0, 1);
        guardText.alpha = alpha;
        if (guardText.text == "")
        {
            textBackground.color = new Color(1, 1, 1, 0);
        }
        else
        {
            textBackground.color = new Color(1, 1, 1, alpha);
        }


        #region State Machine
        switch (cellGuardState)
        {
            case CellGuardState.HasNotTalkedToPlayer:
                if (!player.GetComponent<PlayerManager>().isDoingQuest)
                {
                    iconExclamation.SetActive(true);
                }
                else
                {
                    iconExclamation.SetActive(false);
                }
                iconX.SetActive(false);
                iconCheck.SetActive(false);
                break;
            case CellGuardState.TalkedToPlayer:
                iconExclamation.SetActive(false);
                iconX.SetActive(false);
                iconCheck.SetActive(false);
                guardText.text = introDialogue;
                break;
            case CellGuardState.AskingForItems:
                iconExclamation.SetActive(false);
                iconX.SetActive(false);
                iconCheck.SetActive(false);
                facePlate.material.mainTexture = neutralFace;
                string fullItemText = itemCheckDialogue + "\n";
                for (int i = 0; i < items.Count; i++)
                {
                    fullItemText += "something <color=red>" + items[i].itemTags[0] + ", " + items[i].itemTags[1] + ", " + items[i].itemTags[2] + "</color>";
                    if (i != items.Count - 1)
                    {
                        fullItemText += "\n";
                    }
                }
                guardText.richText = true;
                guardText.text = fullItemText;
                break;
            case CellGuardState.AllItemsFound:
                iconExclamation.SetActive(false);
                iconX.SetActive(false);
                iconCheck.SetActive(true);
                facePlate.material.mainTexture = happyFace;
                guardText.text = completeDialogue;
                break;
            case CellGuardState.IncorrectItem:
                iconExclamation.SetActive(false);
                iconCheck.SetActive(false);
                facePlate.material.mainTexture = angryFace;
                guardText.text = angryDialogue;
                if (interactionCooldownTimer >= 4.0f)
                {
                    iconX.SetActive(true);
                    guardText.text = "";
                }
                else
                {
                    iconX.SetActive(false);
                }

                if (interactionCooldownTimer >= angryInteractionCooldown)
                {
                    cellGuardState = CellGuardState.AskingForItems;
                }
                break;
            case CellGuardState.CorrectItemFound:
                iconExclamation.SetActive(false);
                iconX.SetActive(false);
                iconCheck.SetActive(false);
                facePlate.material.mainTexture = happyFace;
                guardText.text = correctItemDialogue;
                if (interactionCooldownTimer >= correctItemTextTime)
                {
                    cellGuardState = CellGuardState.AskingForItems;
                }
                break;
            case CellGuardState.PlayerIsBusy:
                iconExclamation.SetActive(false);
                iconCheck.SetActive(false);
                iconX.SetActive(false);
                guardText.text = playerIsBusyDialogue;
                if (interactionCooldownTimer >= 4.0f) {
                    iconExclamation.SetActive(true);
                    guardText.text = "";
                } 

                if (interactionCooldownTimer >= angryInteractionCooldown) {
                    cellGuardState = CellGuardState.HasNotTalkedToPlayer;
                   
                }
                break;
        }
        #endregion
    }

    public void OnInteract(InteractWithGuardEvent evt)
    {
        //Debug.Log("Cell Guard Interaction");
        if (evt.cellGuard == this )
        {
            if ((!player.GetComponent<PlayerManager>().isDoingQuest || thisGuardIsQuest))
            {
                if (interactionCooldownTimer >= baseInteractionCooldown && cellGuardState != CellGuardState.IncorrectItem && cellGuardState != CellGuardState.CorrectItemFound) {
                    interactionCooldownTimer = 0;

                    switch (cellGuardState) {
                        case CellGuardState.HasNotTalkedToPlayer:
                            player.GetComponent<PlayerManager>().isDoingQuest = true;
                            thisGuardIsQuest = true;
                            cellGuardState = CellGuardState.TalkedToPlayer;
                            break;
                        case CellGuardState.TalkedToPlayer:
                            cellGuardState = CellGuardState.AskingForItems;
                            questGivenEventChannel.RaiseEvent(new QuestGivenEvent(questItems));
                            break;
                        case CellGuardState.AskingForItems:
                            if (evt.heldItem) {
                                bool isItemCorrect = false;
                                foreach (KeyItem i in items) {
                                    isItemCorrect = isItemCorrect || evt.heldItem.CompareItemTags(i);
                                }

                                ForwardData(isItemCorrect);

                                if (isItemCorrect) {
                                    for (int i = items.Count - 1; i >= 0; i--) {
                                        if (evt.heldItem.CompareItemTags(items[i])) {
                                            items.RemoveAt(i);
                                            int j;
                                            for (j = 0; j < items.Count; j++) {
                                                if (items[j] != null) {
                                                    questItems[j] = "something " + items[j].itemTags[0] + ", " + items[j].itemTags[1] + ", " + items[j].itemTags[2] + "";
                                                }
                                            }
                                            for (; j < 3; j++) {
                                                questItems[j] = null;
                                            }
                                            questGivenEventChannel.RaiseEvent(new QuestGivenEvent(questItems));
                                            cellGuardState = CellGuardState.CorrectItemFound;
                                            break;
                                        }
                                    }
                                    if (items.Count == 0) {
                                        cellGuardState = CellGuardState.AllItemsFound;
                                        player.GetComponent<PlayerManager>().isDoingQuest = false;
                                        thisGuardIsQuest = false;
                                        DoorOpenedEventChannel.RaiseEvent(new DoorOpenedEvent(GetComponentInParent<DoorController>()));

                                        //GetComponentInParent<DoorController>().toggleDoor();
                                    }
                                } else {
                                    cellGuardState = CellGuardState.IncorrectItem;
                                }
                                GiveGuardItemEventChannel.RaiseEvent(new GiveGuardItemEvent(isItemCorrect));
                            }
                            break;
                    }
                }
            } else {
                interactionCooldownTimer = 0f;
                cellGuardState = CellGuardState.PlayerIsBusy;
                Debug.Log("You already have a quest!");
            }
        }
    }

    public void SetItems(KeyItem item1, KeyItem item2, KeyItem item3)
    {
        items = new List<KeyItem>
        {
            item1,
            item2,
            item3
        };
        for (int i = 0; i < items.Count; i++)
        {
            questItems[i] = ("something " + items[i].itemTags[0] + ", " + items[i].itemTags[1] + ", " + items[i].itemTags[2]);
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (SceneView.currentDrawingSceneView)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawMesh(cellGuardModels[0].GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh, this.transform.position, this.transform.rotation, this.transform.localScale);
        }
    }
#endif

    private void ForwardData(bool isItemCorrect)
    {
        if (isItemCorrect)
        {
            correctItemGivenEventChannel.RaiseEvent(
                new CorrectItemGivenEvent(
                    "send correct item given data to server",
                    (int)LVL4_EventType.CorrectItemGivenEvent
                ));
        }
        else
        {
            wrongItemGivenEventChannel.RaiseEvent(
                new WrongItemGivenEvent(
                    "send wrong item given data to server",
                    (int)LVL4_EventType.WrongItemGivenEvent
                ));
        }
    }
}
