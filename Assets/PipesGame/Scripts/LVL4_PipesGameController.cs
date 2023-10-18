using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LVL4_PipesGameController : MonoBehaviour
{
    #region Inspector
    [Header("Inspector settings")]
    [Tooltip("The patterns")]
    [SerializeField]
    private List<LVL4_PipesGamePatternSO> gamePatternsSO;
    [Tooltip("The pattern to display")]
    [SerializeField]
    private int patternIndex;
    [Tooltip("The panel")]
    [SerializeField]
    private Transform gameGrid;
    [Tooltip("The buttons")]
    [SerializeField]
    public List<LVL4_PipesButtonController> buttons = new();
    [Tooltip("The prefabs")]
    [SerializeField]
    public List<LVL4_PipesButtonController> prefabs;
    #endregion

    #region Unity Methods
    void Start()
    {
        AddButtons();
        AddListeners();
    }
    #endregion

    #region Helper Methods
    
    //Add the buttons to the grid
    void AddButtons()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject button = Instantiate(gamePatternsSO[patternIndex].Pattern[i].button, gameGrid);
            //button.transform.SetParent(gameGrid, false);

            buttons.Add(button.GetComponent<LVL4_PipesButtonController>());
            //buttons[i].Pivot.Rotate(0, 0, 0);
            
            //Add a rando rotation based on the type of pipe
            switch (buttons[i].PipeButtonType)
            {
                //Angled pipes have 4 possible rotations
                case EPipeButtonType.Angled:
                    {
                        int rotationIndex = Random.Range(0, 4);
                        buttons[i].Pivot.Rotate(0, 0, rotationIndex*90f);
                    }
                    break;
                //Staright pipes have 2 possible rotations
                case EPipeButtonType.Straight:
                    {
                        int rotationIndex = Random.Range(0, 2);
                        buttons[i].Pivot.Rotate(0, 0, rotationIndex * 90f);
                    }
                    break;
                //The empty button CANT rotate
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
            //Check if the current rotation of the pipe isnt matching the one in the SO pattern
            if ((int)button.Pivot.transform.eulerAngles.z != (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90) 
            {
                break;
            }
            i++;
            if (i == 16) 
            {
                Debug.Log("Game has finished");
            }
        }
        
    }
    public void AddWater(LVL4_PipesButtonController buttonController)
    {
        int i = 0;
        for(int j = 0; j < buttons.Count; j++)
        {
            //Check if the current rotation of the pipe isnt matching the one in the SO pattern
            if ((int)buttonController.Pivot.transform.eulerAngles.z == (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90)
            {
                switch (buttons[j].PipeButtonType)
                {
                    //Angled pipes have 4 possible rotations
                    case EPipeButtonType.Angled:
                        {
                            Debug.Log((int)buttonController.Pivot.transform.eulerAngles.z);
                            Debug.Log((int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90);
                            buttons[i] = prefabs[3];
                        }
                        break;
                    //Staright pipes have 2 possible rotations
                    case EPipeButtonType.Straight:
                        {
                            buttons[i] = prefabs[2];
                        }
                        break;
                    //The empty button CANT rotate
                    default:
                        {
                            //buttons[i].Pivot.Rotate(0, 0, 0);
                        }
                        break;
                }
                i++;
            }
            else
            {
                break;
            }
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
    #endregion

    #region Callbacks
    public void OnClick(LVL4_PipesButtonController buttonController)
    {
        switch (buttonController.PipeButtonType)
        {
            //Rotate the pipes based on their types and bring them back to 0 when the possible rotations are finished
            //Angled can have 0, 90, 180 and 270 deg rotation.
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
            //Straight can have 0 and 90 deg rotation.
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
            //Empty pipes do not rotate
            default:
                {
                    Debug.Log("Case Default");
                }
                break;
        }
        //CheckIfTheGameISFinished();
        AddWater(buttonController);
    }
    #endregion
}
