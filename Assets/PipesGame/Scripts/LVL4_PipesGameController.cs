using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Tooltip("Game Completed Prefab")]
    [SerializeField]
    private GameObject GameCopletePrefab;
    [Tooltip("The canvas")]
    [SerializeField]
    private Transform canvas;
    #endregion

    #region Unity Methods
    void Start()
    {
        AddButtons();
        AddListeners();

        AddWater();
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
            button.name += i;
            //buttons[i].Pivot.Rotate(0, 0, 0);
            
            //Add a rando rotation based on the type of pipe
            switch (buttons[i].PipeButtonType)
            {
                //Angled pipes have 4 possible rotations
                case EPipeButtonType.Angled:
                    {
                        int rotationIndex = UnityEngine.Random.Range(0, 4);
                        buttons[i].Pivot.Rotate(0, 0, rotationIndex*90f);
                    }
                    break;
                //Staright pipes have 2 possible rotations
                case EPipeButtonType.Straight:
                    {
                        int rotationIndex = UnityEngine.Random.Range(0, 2);
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
    public void AddWater()
    {
        int correctButtons = 0;

        for (int i = 0; i < buttons.Count; i++)
        {
            Debug.Log("i: "+ i);
            if (buttons[i].PipeButtonType == EPipeButtonType.Empty)
                continue;

            int winIndex = Array.IndexOf(gamePatternsSO[patternIndex].WinIndexes, i);
            Debug.Log("winIndex: " + winIndex);
            if (i == 0)
            {
                if ((int)buttons[i].Pivot.transform.eulerAngles.z == (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90)
                {
                    switch (buttons[i].PipeButtonType)
                    {
                        //Angled pipes have 4 possible rotations
                        case EPipeButtonType.Angled:
                            {
                                buttons[i].SpriteWater();
                            }
                            break;
                        //Staright pipes have 2 possible rotations
                        case EPipeButtonType.Straight:
                            {
                                buttons[i].SpriteWater();
                            }
                            break;
                    }
                    //correctButtons++;
                }
                /*else
                {
                    if (correctButtons > 0)
                    {
                        correctButtons--;
                    }
                }*/
            }
            else if (i > 0)
            {
                int previousWinIndex = winIndex - 1;
                Debug.Log("previousWinIndex: " + previousWinIndex);
                Debug.Log("realPreviousWinIndex :" + gamePatternsSO[patternIndex].WinIndexes[previousWinIndex]);

                //Debug.Log("Rotation" + i + ": " + (int)buttons[i].Pivot.transform.eulerAngles.z + " should be equal to " + (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90);
                if ((int)buttons[i].Pivot.transform.eulerAngles.z == (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90)
                {
                    Debug.Log("HasWater " + buttons[gamePatternsSO[patternIndex].WinIndexes[previousWinIndex]] + ": " + buttons[gamePatternsSO[patternIndex].WinIndexes[previousWinIndex]].HasWater);
                    if (buttons[gamePatternsSO[patternIndex].WinIndexes[previousWinIndex]].HasWater)
                    {
                        switch (buttons[i].PipeButtonType)
                        {
                            //Angled pipes have 4 possible rotations
                            case EPipeButtonType.Angled:
                                {
                                    buttons[i].SpriteWater();
                                }
                                break;
                            //Staright pipes have 2 possible rotations
                            case EPipeButtonType.Straight:
                                {
                                    buttons[i].SpriteWater();
                                }
                                break;
                        }
                    }

                }
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
        AddWater();
    }
    #endregion
}
