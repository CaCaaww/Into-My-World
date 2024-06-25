using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    public void ReturnToLevel()
    {
        CloseGameEventChannel.RaiseEvent(new CloseGameEvent());
    }

    public GameObject GetParent()
    {
        return this.gameObject.transform.parent.gameObject;
    }
}
