using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class finalStageManager : MonoBehaviour
{
    [SerializeField] private List<string> stringsForTextBoxes;
    [SerializeField] private List<GameObject> dragableTextBoxes;
    [SerializeField] private List<GameObject> dropableBoxHolders;
    [Header("The indexes of the dragable text boxes in the order which they need to be placed in to be correct.\nIe: if the 3rd index dragable has to be the first box in the pattern, then the first number of this list is 3")]
    [SerializeField] private List<int> correctIndexOrder;
    
    private void Start() {
        int index = 0;
        foreach (GameObject textBox in dragableTextBoxes) {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = stringsForTextBoxes[index];
            index++;
        }
        setMatchingPairs();
    }
    private void setMatchingPairs() {
        for (int i = 0; i < correctIndexOrder.Count; i++) {
            dragableTextBoxes[correctIndexOrder[i]].GetComponent<Drag>().setMatchingPair(dragableTextBoxes[i]);
        }
    }
    public void checkOrder() {
        bool correctOrder = true;
        foreach(GameObject textBox in dragableTextBoxes) {
            if (!textBox.GetComponent<Drag>().checkMatchesWithPair()) {
                correctOrder = false; break;
            }
        }
        if (correctOrder) {
            Debug.Log("You won!!!");
        } else {
            Debug.Log("Not Correct");
        }
    }
}
