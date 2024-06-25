using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    #region Helper methods
    /// <summary>
    /// Brings the player back to first-person gameplay
    /// </summary>
    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    public void ReturnToLevel()
    {
        CloseGameEventChannel.RaiseEvent(new CloseGameEvent());
    }

    /// <summary>
    /// Returns the parent GameObject
    /// </summary>
    public GameObject GetParent()
    {
        return this.gameObject.transform.parent.gameObject;
    }
    #endregion
}
