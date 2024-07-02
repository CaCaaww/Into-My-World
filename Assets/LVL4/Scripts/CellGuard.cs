using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CellGuard : MonoBehaviour
{
    private enum CellGuardState
    {
        HasNotTalkedToPlayer,
        TalkedToPlayer,
        AskingForItems,
        IncorrectItem,
        AllItemsFound,
        CorrectItemFound
    }

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
    [SerializeField]
    private List<GameObject> cellGuardModels;
    [SerializeField]
    private PlayerTransformSO playerTransform;
    [SerializeField]
    private TMP_Text guardText;
    [SerializeField]
    private SpriteRenderer textBackground;
    [SerializeField, Tooltip("Face textures for the cell guard")]
    private Texture2D angryFace, neutralFace, happyFace;
    [SerializeField]
    private float baseInteractionCooldown;
    [SerializeField]
    private float angryInteractionCooldown;
    [SerializeField]
    private float correctItemTextTime;
    [SerializeField]
    private float textAlphaFalloffDistance;

    [Header("Listening Event Channels")]
    [SerializeField]
    private DoorOpenedEventChannel DoorOpenedEventChannel;
    [SerializeField]
    private InteractWithGuardEventChannel interactWithGuardEventChannel;
    [SerializeField]
    private GiveGuardItemEventChannel GiveGuardItemEventChannel;
    #endregion

    #region Private Variables
    private CellGuardState cellGuardState;
    private MeshRenderer facePlate;
    private float interactionCooldownTimer;
    private List<KeyItem> items;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        interactWithGuardEventChannel.OnEventRaised += OnInteract;

        cellGuardState = CellGuardState.HasNotTalkedToPlayer;

        GameObject model = Instantiate(cellGuardModels[Random.Range(0, cellGuardModels.Count)], this.transform);
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
        interactionCooldownTimer += Time.deltaTime;

        float distance = Vector3.Distance(this.transform.position, playerTransform.Position);
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
                break;
            case CellGuardState.TalkedToPlayer:
                guardText.text = introDialogue;
                break;
            case CellGuardState.AskingForItems:
                facePlate.material.mainTexture = neutralFace;
                string fullItemText = itemCheckDialogue + "\n";
                for (int i = 0; i < items.Count; i++)
                {
                    fullItemText += "something " + items[i].itemTags[0] + ", " + items[i].itemTags[1] + ", " + items[i].itemTags[2];
                    if (i != items.Count - 1)
                    {
                        fullItemText += "\n";
                    }
                }
                guardText.text = fullItemText;
                break;
            case CellGuardState.AllItemsFound:
                facePlate.material.mainTexture = happyFace;
                guardText.text = completeDialogue;
                break;
            case CellGuardState.IncorrectItem:
                facePlate.material.mainTexture = angryFace;
                guardText.text = angryDialogue;
                if (interactionCooldownTimer >= angryInteractionCooldown)
                {
                    cellGuardState = CellGuardState.AskingForItems;
                }
                break;
            case CellGuardState.CorrectItemFound:
                facePlate.material.mainTexture = happyFace;
                guardText.text = correctItemDialogue;
                if (interactionCooldownTimer >= correctItemTextTime)
                {
                    cellGuardState = CellGuardState.AskingForItems;
                }
                break;
        }
        #endregion
    }

    public void OnInteract(InteractWithGuardEvent evt)
    {
        Debug.Log("Cell Guard Interaction");
        if (evt.cellGuard == this)
        {
            if (interactionCooldownTimer >= baseInteractionCooldown && cellGuardState != CellGuardState.IncorrectItem && cellGuardState != CellGuardState.CorrectItemFound)
            {
                interactionCooldownTimer = 0;

                switch (cellGuardState)
                {
                    case CellGuardState.HasNotTalkedToPlayer:
                        cellGuardState = CellGuardState.TalkedToPlayer;
                        break;
                    case CellGuardState.TalkedToPlayer:
                        cellGuardState = CellGuardState.AskingForItems;
                        break;
                    case CellGuardState.AskingForItems:
                        if (evt.heldItem)
                        {
                            bool isItemCorrect = false;
                            foreach (KeyItem i in items)
                            {
                                isItemCorrect = isItemCorrect || evt.heldItem.CompareItemTags(i);
                            }

                            if (isItemCorrect)
                            {

                                for (int i = items.Count - 1; i >= 0; i--)
                                {
                                    if (evt.heldItem.CompareItemTags(items[i]))
                                    {
                                        items.RemoveAt(i);
                                        cellGuardState = CellGuardState.CorrectItemFound;
                                        break;
                                    }
                                }
                                if (items.Count == 0)
                                {
                                    cellGuardState = CellGuardState.AllItemsFound;
                                    DoorOpenedEventChannel.RaiseEvent(new DoorOpenedEvent(GetComponentInParent<DoorController>()));
                                    //GetComponentInParent<DoorController>().toggleDoor();
                                }
                            }
                            else
                            {
                                cellGuardState = CellGuardState.IncorrectItem;
                            }
                            GiveGuardItemEventChannel.RaiseEvent(new GiveGuardItemEvent(isItemCorrect));
                        }
                        break;
                }
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
}
