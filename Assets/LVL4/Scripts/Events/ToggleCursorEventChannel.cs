using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toggle Cursor Event Channel", menuName = "Events/Toggle Cursor Event Channel")]

public class ToggleCursorEventChannel : GenericEventChannelSO<ToggleCursorEvent> {
}
[System.Serializable]
public struct ToggleCursorEvent {

}
