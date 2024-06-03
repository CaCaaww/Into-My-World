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
    private GameObject GameCompletePrefab;

    [Tooltip("The canvas")]
    [SerializeField]
    private Transform canvas;
    #endregion

    #region Private variables
    int[] winIndexes;
    private int winIndexesLength;
    //private int correctButtons;
    #endregion

    #region Unity Methods
    void Start()
    {
        winIndexes = (int[]) gamePatternsSO[patternIndex].WinIndexes.Clone();
        winIndexesLength = winIndexes.Count();

        AddButtons();
        AddListeners();
        CheckRotation();
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Add listeners to all the interactable buttons
    /// </summary>
    void AddListeners()
    {
        foreach (LVL4_PipesButtonController btn in buttons)
        {
            btn.Button.onClick.AddListener(() => OnClick(btn));
        }
    }
    
    /// <summary>
    /// Add the buttons to the grid
    /// </summary>
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
   
    /// <summary>
    /// Add or remove the water for a section
    /// </summary>
    /// <param name="buttonController">The button controller</param>
    /// <param name="water">true to add water, false to remove it</param>
    void AddOrRemoveWater(LVL4_PipesButtonController buttonController, bool water)
    {
        if (water)
        {
            buttonController.SpriteWater();
        }
        else
        {
            buttonController.SpriteNoWater();
        }
    }
    
    /// <summary>
    /// Check the rotation for all the buttons
    /// </summary>
    /// <param name="winIndexes">The array that contains all the button indexes for the current winning pattern</param>
    void CheckRotation()
    {
        for (int i = 0; i < winIndexesLength; i++)
        {
            // Debug.Log("["+ i + "] Current winIndex value: " + winIndexes[i]);

            if (i == 0)
            {
                if ((int)buttons[winIndexes[i]].Pivot.transform.eulerAngles.z == (int)gamePatternsSO[patternIndex].Pattern[i].rotation * 90)
                {
                    AddOrRemoveWater(buttons[winIndexes[i]], true);
                    buttons[winIndexes[i]].HasCorrectRotation = true;
                }
                else
                {
                    AddOrRemoveWater(buttons[winIndexes[i]], false);
                    buttons[winIndexes[i]].HasCorrectRotation = false;
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
                    if (buttons[winIndexes[previousWinIndex]].HasWater)
                    {
                        AddOrRemoveWater(buttons[winIndexes[i]], true);
                    }

                    buttons[winIndexes[i]].HasCorrectRotation = true;
                }
                else
                {
                    AddOrRemoveWater(buttons[winIndexes[i]], false);
                    buttons[winIndexes[i]].HasCorrectRotation = false;
                }
            }

            if (!buttons[winIndexes[i]].HasWater)
            {
                for (int j = i; j < winIndexes.Length; j++)
                {
                    if (buttons[winIndexes[j]].HasWater)
                    {
                        AddOrRemoveWater(buttons[winIndexes[j]], false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check if the game is finished
    /// </summary>
    void CheckIfTheGameIsFinished()
    {
        int withCorrectRotation = 0;
        bool gameFinished = false;

        for (int i = 0; i < winIndexesLength; i++)
        {
            if (buttons[winIndexes[i]].HasCorrectRotation)
                withCorrectRotation += 1;
        }

        if (withCorrectRotation == winIndexesLength)
            gameFinished = true;

        Debug.Log(gameFinished ? "Game Finished" : "Not Finished");

        if(gameFinished) {
            GameObject GameComplete = Instantiate(GameCompletePrefab);
            GameComplete.transform.SetParent(canvas, false);
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

        CheckRotation();
        CheckIfTheGameIsFinished();
    }
    #endregion
}