using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Give Guard Item Event Channel", menuName = "Events/Give Guard Item Event Channel")]
public class GiveGuardItemEventChannel : GenericEventChannelSO<GiveGuardItemEvent> { }

[System.Serializable]
public struct GiveGuardItemEvent
{
    public bool isCorrectItem;

    public GiveGuardItemEvent(bool isCorrectItem)
    {
        this.isCorrectItem = isCorrectItem;
    }
}