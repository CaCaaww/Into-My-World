using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;
    private bool foundDrop = false;
    private GameObject matchingPair;
    private GameObject currentlyAttached;
    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<FoundDropEvent> foundDropEventChannel;
    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        foundDrop = false;
    }
    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts=true;
        if (!foundDrop) {
            rectTransform.anchoredPosition = startPosition;
            GetComponentInParent<finalStageManager>().removeFromTakenList(currentlyAttached);
            currentlyAttached = null;
        } 
    }
    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
        foundDropEventChannel.OnEventRaised += OnFoundDrop;
    }
    public void OnFoundDrop(FoundDropEvent evt) {
        if (evt.current == rectTransform.anchoredPosition) {
            if (GetComponentInParent<finalStageManager>().checkNotInTakenList(evt.pair)) {
                foundDrop = true;
                if (currentlyAttached != null) {
                    GetComponentInParent<finalStageManager>().removeFromTakenList(currentlyAttached);
                }
                currentlyAttached = evt.pair;
                GetComponentInParent<finalStageManager>().addToTakenList(evt.pair);
            }        
        }    
    }
    public bool checkMatchesWithPair() {
        if (currentlyAttached != null && matchingPair != null) {
            return matchingPair == currentlyAttached;
        } else {
            return false;
        }
    }
    public void setMatchingPair(GameObject pair) {
        matchingPair = pair;
    }
}
