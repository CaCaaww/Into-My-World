using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class continueStageButton : MonoBehaviour
{
    #region Inspector

    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<OpenNextStageEvent> OpenNextStageEventChannel; 
    #endregion

    public void openNextStage() {
        OpenNextStageEventChannel.RaiseEvent(new OpenNextStageEvent());
    }
}
