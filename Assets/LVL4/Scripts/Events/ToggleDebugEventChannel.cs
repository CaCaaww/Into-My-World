using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toggle Debug Event Channel", menuName = "Events/Toggle Debug Event Channel")]
public class ToggleDebugEventChannel : GenericEventChannelSO<ToggleDebugEvent> { }

[System.Serializable]
public struct ToggleDebugEvent
{
}
