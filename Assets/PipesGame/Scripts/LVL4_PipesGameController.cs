using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LVL4_PipesGameController : MonoBehaviour
{
    [SerializeField]
    private List<LVL4_PipesGamePatternSO> gamePatternsSO;
    [Tooltip("The panel")]
    [SerializeField]
    private Transform gameGrid;
    [SerializeField]
    public List<LVL4_PipesButtonController> buttons = new();
    
    // Start is called before the first frame update
    void Start()
    {
        AddButtons();
        AddListeners();
    }


    void AddButtons()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject button = Instantiate(gamePatternsSO[0].Pattern[i].button, gameGrid);
            //button.transform.SetParent(gameGrid, false);

            buttons.Add(button.GetComponent<LVL4_PipesButtonController>());
            //buttons[i].Pivot.Rotate(0, 0, 0);
            
            
            switch (buttons[i].PipeButtonType)
            {
                case EPipeButtonType.Angled:
                    {
                        int rotationIndex = Random.Range(0, 4);
                        buttons[i].Pivot.Rotate(0, 0, rotationIndex*90f);
                    }
                    break;
                case EPipeButtonType.Straight:
                    {
                        int rotationIndex = Random.Range(0, 2);
                        buttons[i].Pivot.Rotate(0, 0, rotationIndex * 90f);
                    }
                    break;
                default:
                    {
                        buttons[i].Pivot.Rotate(0, 0, 0);
                    }
                    break;
            }
            
            //Debug.Log((float)gamePatternsSO[0].Pattern[i].rotation);
            //Debug.Log(i);
            //Debug.Break();
            //buttons[i].Pivot.Rotate(0, 0, 90);
        }
    }

    public void CheckIfTheGameISFinished()
    {
        int i = 0;
        foreach (LVL4_PipesButtonController button in buttons)
        {
           
            if ((int)button.Pivot.transform.eulerAngles.z != (int)gamePatternsSO[0].Pattern[i].rotation * 90) 
            {
                //Debug.Log(button);
                //Debug.Log("ok");
                break;
            }
            i++;
            if (i == 16) 
            {
                Debug.Log("Game has finished");
            }
        }
        
    }
    void AddListeners()
    {

        foreach (LVL4_PipesButtonController btn in buttons)
        {
            /*
            if (btn.Button == null) {
                Debug.Log("button is null");
            }
            else
            {
                Debug.Log("button exist");

            }*/
            btn.Button.onClick.AddListener(() => OnClick(btn));
        }
    }
    public void OnClick(LVL4_PipesButtonController buttonController)
    {
        switch (buttonController.PipeButtonType)
        {
            case EPipeButtonType.Angled:
                {
                    if (buttonController.Pivot.transform.eulerAngles.z >= 270)
                    {
                        //Debug.Log("if");
                        //Debug.Log(pivot.transform.eulerAngles.z);
                        buttonController.Pivot.Rotate(0, 0, -270);
                        //Debug.Log(pivot.transform.eulerAngles.z);
                    }
                    else
                    {
                        //Debug.Log("else");
                        //Debug.Log(pivot.transform.eulerAngles.z);
                        buttonController.Pivot.Rotate(0, 0, 90);
                        //Debug.Log(pivot.transform.eulerAngles.z);
                    }
                }
                break;
            case EPipeButtonType.Straight:
                {
                    if (buttonController.Pivot.transform.eulerAngles.z >= 90)
                    {
                        buttonController.Pivot.Rotate(0, 0, -buttonController.Pivot.transform.eulerAngles.z);
                    }
                    else
                    {
                        buttonController.Pivot.Rotate(0, 0, 90);
                    }
                }
                break;
            default:
                {
                    Debug.Log("Case Default");
                }
                break;
        }
        CheckIfTheGameISFinished();
    }
}
