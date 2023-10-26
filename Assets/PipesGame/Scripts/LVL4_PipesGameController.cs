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
    [Header("Debug")]
    [SerializeField]
    private bool enableDebug;
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

    #region Private variables
    int[] winIndexes;
    #endregion

    #region Unity Methods
    void Start()
    {
        winIndexes = (int[]) gamePatternsSO[patternIndex].WinIndexes.Clone();

        AddButtons();
        AddListeners();
        CheckRotation(winIndexes);
    }
    #endregion

    #region Helper Methods
    
    //Add the buttons to the grid
    void AddButtons()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject button = Instantiate(gamePatternsSO[patternIndex].Pattern[i].button, gameGrid);
            
            buttons.Add(button.GetComponent<LVL4_PipesButtonController>());
            button.name += i;
            
            // Add a random rotation based on the type of pipe
            switch (buttons[i].PipeButtonType)
            {
                // Angled pipes have 4 possible rotations
                case EPipeButtonType.Angled:
                    {
                        #if UNITY_EDITOR
                        if (enableDebug)
                        {
                            buttons[i].Pivot.Rotate(0, 0, (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90f);
                            continue;
                        }
                        #endif

                        // Assign to rotationIndex a random value[0,4) 
                        int rotationIndex = UnityEngine.Random.Range(0, 4);
                        // Keep doing it until the generated value it's different from the correct pattern rotation
                        while (rotationIndex == (int)gamePatternsSO[patternIndex].Pattern[i].rotation)
                        {
                            rotationIndex = UnityEngine.Random.Range(0, 4);
                        }
                        buttons[i].Pivot.Rotate(0, 0, rotationIndex*90f);
                    }
                    break;
                // Straight pipes have 2 possible rotations
                case EPipeButtonType.Straight:
                    {
                        #if UNITY_EDITOR
                        if (enableDebug)
                        {
                            buttons[i].Pivot.Rotate(0, 0, (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90f);
                            continue;
                        }
                        #endif

                        int rotationIndex = UnityEngine.Random.Range(0, 2);
                        buttons[i].Pivot.eulerAngles = Vector3.zero;
                    }
                    break;
                // The empty button CAN NOT rotate
                default:
                    {
                        buttons[i].Pivot.Rotate(0, 0, 0);
                    }
                    break;
            }
        }
    }

    public void CheckIfTheGameIsFinished()
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
    public void CheckRotation(int[] winIndexes)
    {
        for (int i = 0; i < winIndexes.Length; i++)
        {
            // Debug.Log("["+ i + "] Current winIndex value: " + winIndexes[i]);

            if (i == 0)
            {
                if ((int)buttons[winIndexes[i]].Pivot.transform.eulerAngles.z == (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90)
                {
                    switch (buttons[winIndexes[i]].PipeButtonType)
                    {
                        //Angled pipes have 4 possible rotations
                        case EPipeButtonType.Angled:
                            {
                                buttons[winIndexes[i]].SpriteWater();
                            }
                            break;
                        //Staright pipes have 2 possible rotations
                        case EPipeButtonType.Straight:
                            {
                                buttons[winIndexes[i]].SpriteWater();
                            }
                            break;
                    }
                }
                else
                {
                    switch (buttons[winIndexes[i]].PipeButtonType)
                    {
                        //Angled pipes have 4 possible rotations
                        case EPipeButtonType.Angled:
                            {
                                buttons[winIndexes[i]].SpriteNoWater();
                            }
                            break;
                        //Staright pipes have 2 possible rotations
                        case EPipeButtonType.Straight:
                            {
                                buttons[winIndexes[i]].SpriteNoWater();
                            }
                            break;
                    }
                }
            }
            else if (i > 0)
            {
                int previousWinIndex = i - 1;
                // Debug.Log("["+ i + "] Previous winIndex: " + previousWinIndex);
                // Debug.Log("["+ i + "] Previous winIndex value: " + winIndexes[previousWinIndex]);
                // Debug.Log("["+ i + "] " + (int)buttons[winIndexes[i]].Pivot.transform.eulerAngles.z);
                // Debug.Log("["+ i + "] " + (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90);
                if ((int)buttons[winIndexes[i]].Pivot.transform.eulerAngles.z == (int)gamePatternsSO[patternIndex].Pattern[winIndexes[i]].rotation * 90)
                {
                    // Debug.Log("["+ i + "] Previous winIndex HasWater " + buttons[winIndexes[previousWinIndex]] + ": " + buttons[winIndexes[previousWinIndex]].HasWater);
                    if (buttons[winIndexes[previousWinIndex]].HasWater)
                    {
                        switch (buttons[winIndexes[i]].PipeButtonType)
                        {
                            //Angled pipes have 4 possible rotations
                            case EPipeButtonType.Angled:
                                {
                                    buttons[winIndexes[i]].SpriteWater();
                                }
                                break;
                            //Staright pipes have 2 possible rotations
                            case EPipeButtonType.Straight:
                                {
                                    buttons[winIndexes[i]].SpriteWater();
                                }
                                break;
                        }
                    }
                }
                else
                {
                    switch (buttons[winIndexes[i]].PipeButtonType)
                    {
                        //Angled pipes have 4 possible rotations
                        case EPipeButtonType.Angled:
                            {
                                buttons[winIndexes[i]].SpriteNoWater();
                            }
                            break;
                        //Staright pipes have 2 possible rotations
                        case EPipeButtonType.Straight:
                            {
                                buttons[winIndexes[i]].SpriteNoWater();
                            }
                            break;
                    }
                }
            }

            if (!buttons[winIndexes[i]].HasWater)
            {
                for (int j = i; j < winIndexes.Length; j++)
                {
                    if (buttons[winIndexes[j]].HasWater)
                    {
                        switch (buttons[winIndexes[j]].PipeButtonType)
                        {
                            //Angled pipes have 4 possible rotations
                            case EPipeButtonType.Angled:
                                {
                                    buttons[winIndexes[j]].SpriteNoWater();
                                }
                                break;
                            //Staright pipes have 2 possible rotations
                            case EPipeButtonType.Straight:
                                {
                                    buttons[winIndexes[j]].SpriteNoWater();
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
                    //Debug.Log(buttonController.Pivot.transform.rotation.z);
                    if (buttonController.Pivot.transform.eulerAngles.z >= 270)
                    {
                        buttonController.Pivot.rotation = Quaternion.Euler(new Vector3(0,0,0));
                    }
                    else
                    {
                        buttonController.Pivot.Rotate(0, 0, 90);
                    }
                    //Debug.Log(buttonController.Pivot.transform.eulerAngles.z);
                }
                break;
            //Straight can have 0 and 90 deg rotation.
            case EPipeButtonType.Straight:
                {
                    //Debug.Log(buttonController.Pivot.transform.rotation.z);
                    if (buttonController.Pivot.transform.eulerAngles.z >= 90)
                    {
                        buttonController.Pivot.rotation = Quaternion.Euler(new Vector3(0,0,0));
                    }
                    else
                    {
                        buttonController.Pivot.Rotate(0, 0, 90);
                    }
                    //Debug.Log(buttonController.Pivot.transform.eulerAngles.z);
                }
                break;
            //Empty pipes do not rotate
            default:
                {
                    //Debug.Log("Case Default");
                }
                break;
        }

        //CheckIfTheGameISFinished();
        CheckRotation(winIndexes);
    }
    #endregion
}