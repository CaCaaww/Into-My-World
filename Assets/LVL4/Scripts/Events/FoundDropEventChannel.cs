using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Found Drop Event Channel", menuName = "Events/Found Drop Event Channel")]
public class FoundDropEventChannel : GenericEventChannelSO<FoundDropEvent> {

}
[System.Serializable]
public struct FoundDropEvent {
    public Vector2 current;
    public GameObject pair;

    public FoundDropEvent(Vector3 current, GameObject pair) {
        this.current = current;
        this.pair = pair;
    }
}

