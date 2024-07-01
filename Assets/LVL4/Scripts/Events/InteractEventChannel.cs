using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interact Event Channel", menuName = "Events/Interact Event Channel")]
public class InteractEventChannel : GenericEventChannelSO<InteractEvent> { }

[System.Serializable]
public struct InteractEvent
{
}
