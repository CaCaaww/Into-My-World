using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Given Event Channel", menuName = "Events/Quest Given Event Channel")]
public class QuestGivenEventChannel : GenericEventChannelSO<QuestGivenEvent>
{
}
public struct QuestGivenEvent {
    public string[] questItems;
    public QuestGivenEvent(string[] questItems) {
        this.questItems = questItems;
    }
}
