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
    [SerializeField]
    private RectTransform pivot;
    public RectTransform Pivot
    {
        get => pivot;
        set => pivot = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener( () => OnClick());
        

    }
    private void OnDisable()
    {
        button.onClick.RemoveListener( OnClick );
    }
    public void OnClick()
    {
        Debug.Log("premuto");
        pivot.Rotate(0, 0, -90);
        //this.transform.parent.Rotate(0, 0, 90);
    }
}
