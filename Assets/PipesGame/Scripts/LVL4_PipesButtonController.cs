using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#region Enums
public enum EPipeButtonType
{
    Empty, Angled, Straight
}
#endregion
public class LVL4_PipesButtonController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private EPipeButtonType pipeButtonType;
    public EPipeButtonType PipeButtonType
    {
        get => pipeButtonType;
        set => pipeButtonType = value;
    }
    private Button button;
    public Button Button
    {
        get => button;
    }
    [SerializeField]
    private RectTransform pivot;
    public RectTransform Pivot
    {
        get => pivot;
        set => pivot = value;
    }
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        button = GetComponent<Button>();
    }
    #endregion
}
