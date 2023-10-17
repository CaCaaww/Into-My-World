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
        switch (PipeButtonType)
        {
            case EPipeButtonType.Angled:
                {
                    if (pivot.transform.eulerAngles.z >= 270)
                    {
                        //Debug.Log("if");
                        //Debug.Log(pivot.transform.eulerAngles.z);
                        pivot.Rotate(0, 0, -270);
                        //Debug.Log(pivot.transform.eulerAngles.z);
                    }
                    else
                    {
                        //Debug.Log("else");
                        //Debug.Log(pivot.transform.eulerAngles.z);
                        pivot.Rotate(0, 0, 90);
                        //Debug.Log(pivot.transform.eulerAngles.z);
                    }
                }
                break;
            case EPipeButtonType.Straight:
                {
                    if (pivot.transform.eulerAngles.z >= 90)
                    {
                        pivot.Rotate(0, 0, -pivot.transform.eulerAngles.z);
                    }
                    else
                    {
                        pivot.Rotate(0, 0, 90);
                    }
                }
                break;
            default:
                {
                    Debug.Log("Case Default");
                }
                break;
        }

    }
}
