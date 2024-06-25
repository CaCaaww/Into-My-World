using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    #region Inspector

    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    #endregion

    public void ReturnToLevel()
    {
        CloseGameEventChannel.RaiseEvent(new CloseGameEvent(GetControllerInRoot()));
    }

    public CloseGameController GetControllerInRoot()
    {
        return this.gameObject.transform.root.GetComponent<CloseGameController>();
    }

}
