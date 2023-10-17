using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EPipeButtonType
{
    Empty, Angled, Straight
}
public class LVL4_PipesButtonController : MonoBehaviour
{
    
    [SerializeField]
    private EPipeButtonType pipeButtonType;
    public EPipeButtonType PipeButtonType
    {
        get => pipeButtonType;
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
    // Start is called before the first frame update
    void OnEnable()
    {
        button = GetComponent<Button>();
       
    }
}
