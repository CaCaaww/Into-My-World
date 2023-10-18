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
    [SerializeField]
    private Sprite spriteNoWater;
    [SerializeField]
    private Sprite spriteWater;
    [SerializeField]
    private Image image;
    //private Image sprite;
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
        }
    }
    public void SpriteNoWater()
    {
        if (pipeButtonType != EPipeButtonType.Empty)
        {
            image.sprite = spriteNoWater;
        }
    }
    #endregion
}
