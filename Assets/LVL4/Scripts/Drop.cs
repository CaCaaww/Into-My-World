using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler {
    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<FoundDropEvent> foundDropEventChannel;
    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            foundDropEventChannel.RaiseEvent(new FoundDropEvent(eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition));
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
