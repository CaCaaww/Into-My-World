using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGameController : MonoBehaviour
{
    #region Inspector

    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    #endregion
}
