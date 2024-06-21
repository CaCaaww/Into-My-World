using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CellGuard : MonoBehaviour
{
    private enum CellGuardState
    {
        HasNotTalkedToPlayer,
        TalkedToPlayer,
        AskingForItems,
        Angry,
        Happy
    }

    #region Inspector
    [SerializeField, TextArea]
    private string introDialoge;
    [SerializeField, TextArea]
    private string itemCheckDialoge;
    [SerializeField, TextArea]
    private string completeDialoge;
    [SerializeField, TextArea]
    private string angryDialoge;
    [SerializeField]
    private List<GameObject> cellGuardModels;
    [SerializeField]
    TMP_Text guardText;
    [SerializeField, Tooltip("Face textures for the cell guard")]
    private Texture2D angryFace, neutralFace, happyFace;
    [SerializeField]
    private float baseInteractionCooldown;
    [SerializeField]
    private float angryInteractionCooldown;
    [SerializeField]
    private float textAlphaFalloffDistance;
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

        float distance = Vector3.Distance(this.transform.position, LVL4Manager.instance.playerCapsule.transform.position);
        float alpha = 1 - ((distance - textAlphaFalloffDistance / 2) / (textAlphaFalloffDistance / 2));
        alpha = Mathf.Clamp(alpha, 0, 1);
        guardText.alpha = alpha;

        #region State Machine
        switch (cellGuardState)
        {
            case CellGuardState.HasNotTalkedToPlayer:
                break;
            case CellGuardState.TalkedToPlayer:
                guardText.text = introDialoge;
                break;
            case CellGuardState.AskingForItems:
                facePlate.material.mainTexture = neutralFace;
                string fullItemText = itemCheckDialoge + "\n";
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
            case CellGuardState.Happy:
                facePlate.material.mainTexture = happyFace;
                guardText.text = completeDialoge;
                break;
            case CellGuardState.Angry:
                facePlate.material.mainTexture = angryFace;
                guardText.text = angryDialoge;
                if (interactionCooldownTimer >= angryInteractionCooldown)
                {
                    cellGuardState = CellGuardState.AskingForItems;
                }
                break;
        }
        #endregion
    }

    public void Interact()
    {
        Debug.Log("Cell Guard Interaction");

        if (interactionCooldownTimer >= baseInteractionCooldown)
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
                    if (LVL4Manager.instance.currentlyHeldItem)
                    {
                        bool isItemCorrect = false;
                        foreach (KeyItem i in items)
                        {
                            isItemCorrect = isItemCorrect || LVL4Manager.instance.currentlyHeldItem.CompareItemTags(i);
                        }

                        if (isItemCorrect)
                        {
                            for (int i = items.Count - 1; i >= 0; i--)
                            {
                                if (LVL4Manager.instance.currentlyHeldItem.CompareItemTags(items[i]))
                                {
                                    items.RemoveAt(i);
                                    LVL4Manager.instance.ItemWasCorrect();
                                    break;
                                }
                            }
                            if (items.Count == 0)
                            {
                                cellGuardState = CellGuardState.Happy;
                                GetComponentInParent<DoorController>().toggleDoor();
                            }
                        }
                        else
                        {
                            cellGuardState = CellGuardState.Angry;
                            LVL4Manager.instance.ItemWasIncorrect();
                        }
                    }
                    break;
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
