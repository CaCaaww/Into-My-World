using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Close Game Event Channel", menuName = "Events/Close Game Event Channel")]
public class CloseGameEventChannel : GenericEventChannelSO<CloseGameEvent>
{
}

[System.Serializable]
public struct CloseGameEvent {
    
}
