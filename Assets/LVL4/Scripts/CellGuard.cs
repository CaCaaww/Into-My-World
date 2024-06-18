using System.Collections;
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
        Angry
    }

    #region Inspector
    [SerializeField, TextArea]
    private string introDialoge;
    [SerializeField, TextArea]
    private string itemCheckDialoge;
    [SerializeField]
    private List<GameObject> cellGuardModels;
    [SerializeField]
    TMP_Text guardText;
    [SerializeField, Tooltip("Face textures for the cell guard")]
    private Texture2D angryFace, neutralFace, happyFace;
    [SerializeField]
    private float baseInteractionCooldown;
    #endregion

    #region Private Variables
    private CellGuardState cellGuardState;
    private MeshRenderer facePlate;
    private int itemsFound;
    private float interactionCooldownTimer;
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

        itemsFound = -1;

        GetComponent<Animator>().Rebind();
    }

    // Update is called once per frame
    void Update()
    {
        interactionCooldownTimer += Time.deltaTime;

        #region State Machine
        switch (cellGuardState)
        {
            case CellGuardState.HasNotTalkedToPlayer:
                break;
            case CellGuardState.TalkedToPlayer:
                guardText.text = introDialoge;
                break;
            case CellGuardState.AskingForItems:
                guardText.text = itemCheckDialoge;
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

            if (cellGuardState == CellGuardState.HasNotTalkedToPlayer)
            {
                cellGuardState = CellGuardState.TalkedToPlayer;
            }
            else if (cellGuardState == CellGuardState.TalkedToPlayer)
            {
                cellGuardState = CellGuardState.AskingForItems;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (SceneView.currentDrawingSceneView)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawMesh(cellGuardModels[0].GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh, this.transform.position, this.transform.rotation, this.transform.localScale);
        }
    }

}
