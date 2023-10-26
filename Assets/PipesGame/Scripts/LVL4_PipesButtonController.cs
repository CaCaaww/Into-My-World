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
    #region Inspector
    [SerializeField]
    private EPipeButtonType pipeButtonType;
    [SerializeField]
    private RectTransform pivot;
    [SerializeField]
    private Sprite spriteNoWater;
    [SerializeField]
    private Sprite spriteWater;
    [SerializeField]
    private Image image;
    [SerializeField]
    private bool hasWater;
    #endregion

    #region Private Variables
    private Button button;
    [SerializeField]
    private bool hasCorrectRotation;
    #endregion

    #region Public Properties
    public EPipeButtonType PipeButtonType { get => pipeButtonType; set => pipeButtonType = value; }
    public Button Button { get => button; }
    public RectTransform Pivot { get => pivot; set => pivot = value; }
    public bool HasWater { get => hasWater; }
    public bool HasCorrectRotation { get => hasCorrectRotation; set => hasCorrectRotation = value; }
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        button = GetComponent<Button>();
    }
    #endregion
    #region Public Methods
    public void SpriteWater()
    {
        if (pipeButtonType != EPipeButtonType.Empty) 
        {
            image.sprite = spriteWater;
            hasWater = true;
        }
    }
    public void SpriteNoWater()
    {
        if (pipeButtonType != EPipeButtonType.Empty)
        {
            image.sprite = spriteNoWater;
            hasWater = false;
        }
    }
    #endregion
}
