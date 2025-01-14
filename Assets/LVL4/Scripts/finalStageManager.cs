using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class finalStageManager : MonoBehaviour
{
    private List<GameObject> takenList = new List<GameObject>();
    [SerializeField] private List<string> stringsForTextBoxes;
    [SerializeField] private List<GameObject> dragableTextBoxes;
    [SerializeField] private List<GameObject> dropableBoxHolders;

    [Header("The indexes of the dragable text boxes in the order which they need to be placed in to be correct.\nIe: if the 3rd index dragable has to be the first box in the pattern, then the first number of this list is 3")]
    [SerializeField] private List<int> correctIndexOrder;

    [Header("Event Channels")]
    [SerializeField] private GenericEventChannelSO<FinalStageCompleteEvent> finalStageCompleteEventChannel;
    [SerializeField] private GenericEventChannelSO<LevelCompleteEvent> levelCompleteEventChannel;
    [SerializeField] private GenericEventChannelSO<FinalStageFailedEvent> finalStageFailedEventChannel;
    
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
            dragableTextBoxes[correctIndexOrder[i]].GetComponent<Drag>().setMatchingPair(dropableBoxHolders[i]);
        }
    }
    public void checkOrder() {
        bool correctOrder = true;
        foreach(GameObject textBox in dragableTextBoxes) {
            if (!(textBox.GetComponent<Drag>().checkMatchesWithPair())) {
                correctOrder = false; break;
            }
        }
        if (correctOrder) {
            Debug.Log("You won!!!");
            finalStageCompleteEventChannel.RaiseEvent(new FinalStageCompleteEvent());

            ForwardData();

        }
        else {
            Debug.Log("Not Correct");
            finalStageFailedEventChannel.RaiseEvent(new FinalStageFailedEvent());
        }
    }
    public bool checkNotInTakenList(GameObject checkAgainst) {
        int check = takenList.IndexOf(checkAgainst);
        if (check == -1) {
            return true;
        } else {
            return false;
        }
    }
    public void addToTakenList(GameObject add) {
        takenList.Add(add);
    }
    public void removeFromTakenList(GameObject remove) {
        takenList.Remove(remove);
    }

    #region Helper Methods
    private void ForwardData() {
        levelCompleteEventChannel.RaiseEvent(
            new LevelCompleteEvent(
                "Send level complete data to server",
                (int)LVL4_EventType.LevelCompleteEvent
        ));
    }
    #endregion
}
