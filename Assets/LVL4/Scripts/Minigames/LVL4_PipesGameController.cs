using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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

    [Tooltip("Backdrop")]
    [SerializeField]
    private GameObject backdrop;

    [SerializeField]
    private GameObject continueButton;

    [Tooltip("The canvas")]
    [SerializeField]
    private Transform canvas;
    [SerializeField] private GameObject angledPrefab;
    [SerializeField] private GameObject straightPrefab;
    [SerializeField] private GameObject emptyPrefab;
    [SerializeField]
    private Button quitButton;
    #endregion

    #region Private variables
    int[] winIndexes;
    private int winIndexesLength;
    List<LVL4_PipesGameSpawnableItem> pattern;

    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<MinigameCompleteEvent> MinigameCompleteEventChannel;
    [SerializeField] private GenericEventChannelSO<MinigameOpenedEvent> MinigameOpenedEventChannel;
    [SerializeField] private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;

    private MiniGameController currentController;
    #endregion

    #region constructor
    public class LVL4_PipesGameSpawnableItem {
        public GameObject button;
        public EPipesButtonRotation rotation;
        public EPipeButtonType type;
    }
    #endregion
   
    #region Unity Methods
    void Start()
    {
        //winIndexes = (int[]) gamePatternsSO[patternIndex].WinIndexes.Clone();
        pattern = new List<LVL4_PipesGameSpawnableItem>(); //instantiate the list for the pattern
        for (int i = 0; i < 16; i++) { // filling it with blank objects
            LVL4_PipesGameSpawnableItem adding = new LVL4_PipesGameSpawnableItem();
            pattern.Add(adding);
        }
        MazeGenerator pipePattern = new MazeGenerator(); // generating a random pattern
        winIndexes = pipePattern.getWinIndices(); // getting the winIndexes for that.
        for (int i = 0; i < pattern.Count; i++) { // go through each item in the pattern list and grab the information for that.
            pattern[i].rotation = pipePattern.getButtonRotation(i); // getting the rotation
            EPipeButtonType buttonType = pipePattern.getButton(i); // getting the button type
            switch (buttonType) { // setting the right prefab for that.
                case EPipeButtonType.Angled:
                    pattern[i].button = angledPrefab;
                    pattern[i].type = EPipeButtonType.Angled;
                    break;
                case EPipeButtonType.Straight:
                    pattern[i].button = straightPrefab;
                    pattern[i].type = EPipeButtonType.Straight;
                    break;
                case EPipeButtonType.Empty:
                    pattern[i].button = emptyPrefab;
                    pattern[i].type = EPipeButtonType.Empty;
                    break;
                default:
                    pattern[i].button = emptyPrefab;
                    break;
            }
        }
        winIndexesLength = winIndexes.Count();
        AddButtons();
        AddListeners();
        CheckRotation();
        CheckRotation2();
        
    }
    private void OnEnable() {
        MinigameOpenedEventChannel.OnEventRaised += OnMinigameOpened;
        CloseGameEventChannel.OnEventRaised += OnMinigameClosed;
    }
    #endregion

    #region Helper Methods
    private Side[] getSides(int index) { // returns a list of the sides that the pipe touches
        // used to determine the input and output of the pipes
        if (pattern[index].type == EPipeButtonType.Empty) {
            return null;
        }
        Side[] sides = new Side[2];
        int rotation = (int)buttons[index].Pivot.transform.eulerAngles.z;
        if (pattern[index].type == EPipeButtonType.Angled) {
            switch (rotation) {
                case 0:
                    sides[0] = Side.Bottom;
                    sides[1] = Side.Left;
                    return sides;
                case 90:
                    sides[0] = Side.Right;
                    sides[1] = Side.Bottom;
                    return sides;
                case 180:
                    sides[0] = Side.Top;
                    sides[1] = Side.Right;
                    return sides;
                case 270:
                    sides[0] = Side.Left;
                    sides[1] = Side.Top;
                    return sides;
            }
        } else {
            switch (rotation) {
                case 0:
                    sides[0] = Side.Bottom;
                    sides[1] = Side.Top;
                    return sides;
                case 90:
                    sides[0] = Side.Right;
                    sides[1] = Side.Left;
                    return sides;
            }
        }
        Debug.Log("None???");
        return null;

    }
    private int getIndexFromInput(int index, Side side) { // take an index and side of a pipe to get the index of the pipe attached to that
        // side. Return -10 if out of bounds or other error.
        switch (side) {
            case Side.Left:
                index--;
                if (index == 3 || index == 7 || index == 11 || index == 15) {
                    return -10;
                } else {
                    return index;
                }
            case Side.Right:
                index++;
                if (index == 4 || index == 8 || index == 12 || index == 16) {
                    return -10;
                } else {
                    return index;
                }
            case Side.Top:
                index -= 4;
                if (index < 0) {
                    return -10;
                } else {
                    return index;
                }
            case Side.Bottom:
                index += 4;
                if (index > 15) {
                    return -10;
                } else {
                    return index;
                }
            case Side.None:
                return -10;
        }
        return -10;
    }
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
            //Debug.Log(pattern[i].rotation);
            GameObject button = Instantiate(pattern[i].button, gameGrid);
            
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
                            buttons[i].Pivot.Rotate(0, 0, (int)pattern[i].rotation * 90f);
                            continue;
                        }
                        #endif

                        // Assign to rotationIndex a random value[0,4) 
                        int rotationIndex = UnityEngine.Random.Range(0, 4);
                        // Keep doing it until the generated value it's different from the correct pattern rotation
                        while (rotationIndex == (int)pattern[i].rotation)
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
                            buttons[i].Pivot.Rotate(0, 0, (int)pattern[i].rotation * 90f);
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
                if ((int)buttons[winIndexes[i]].Pivot.transform.eulerAngles.z == (int)pattern[i].rotation * 90)
                {
                    //AddOrRemoveWater(buttons[winIndexes[i]], true);
                    buttons[winIndexes[i]].HasCorrectRotation = true;
                }
                else
                {
                    //AddOrRemoveWater(buttons[winIndexes[i]], false);
                    buttons[winIndexes[i]].HasCorrectRotation = false;
                }
            }
            else if (i > 0)
            {
                int previousWinIndex = i - 1;
                // Debug.Log("["+ i + "] Previous winIndex: " + previousWinIndex);
                // Debug.Log("["+ i + "] Previous winIndex value: " + winIndexes[previousWinIndex]);
                // Debug.Log("["+ i + "] " + (int)buttons[winIndexes[i]].Pivot.transform.eulerAngles.z);
                // Debug.Log("["+ i + "] " + (int)gamePatternsSO[patternIndex]gamePatternsSO[patternIndex].Pattern[i].rotation * 90);
                if ((int)buttons[winIndexes[i]].Pivot.transform.eulerAngles.z == (int)pattern[winIndexes[i]].rotation * 90)
                {
                    
                    /*if (buttons[winIndexes[previousWinIndex]].HasWater)
                    {
                        AddOrRemoveWater(buttons[winIndexes[i]], true);
                    }*/
                    
                    buttons[winIndexes[i]].HasCorrectRotation = true;
                }
                else
                {
                    //AddOrRemoveWater(buttons[winIndexes[i]], false);
                    buttons[winIndexes[i]].HasCorrectRotation = false;
                }
            }
            /*
            if (!buttons[winIndexes[i]].HasWater)
            {
                for (int j = i; j < winIndexes.Length; j++)
                {
                    if (buttons[winIndexes[j]].HasWater)
                    {
                        AddOrRemoveWater(buttons[winIndexes[j]], false);
                    }
                }
            }*/
            
        }
    }
    private void removeRestOfWater(int index) { // removes water from winIndexes List past a certin index. Exclusive
        for (int i = 0; i < winIndexes.Length; i++) {
            if (index == winIndexes[i]) {
                index = i;
                break;
            }
        }
        for (int j = index; j < winIndexes.Length-1; j++) {
            if (buttons[winIndexes[j+1]].HasWater) {
                AddOrRemoveWater(buttons[winIndexes[j+1]], false);
            }
        }
    }
    void CheckRotation2() { //Adds the Water For the pipes
        List<int> visitedIndexes = new List<int>();
        int index = 0;
        int indexLast = 0;
        while (index > -10) { //uses index of -10 if the index is out of bounds, null, or an empty tile
            // Essentially, this program sneaks through the pipes using their input and outputs to add water to
            // all the pipes that are connected to the water source.
            if (index == 0) {
                Side[] sides = getSides(index);
                if (sides[0] == Side.Left) {
                    visitedIndexes.Add(index);
                    AddOrRemoveWater(buttons[index], true);
                    indexLast = index;
                    index = getIndexFromInput(index, sides[1]);
                    if (visitedIndexes.Contains(index)) {
                        index = -10;
                    }
                } else if (sides[1] == Side.Left) {
                    visitedIndexes.Add(index);
                    AddOrRemoveWater(buttons[index], true);
                    indexLast = index;
                    index = getIndexFromInput(index, sides[0]);
                    if (visitedIndexes.Contains(index)) {
                        index = -10;
                    }
                } else {
                    AddOrRemoveWater(buttons[index], false);
                    removeRestOfWater(index);
                    index = -10;
                }
            } else {
                Side[] sides = getSides(index);
                if (sides != null) {
                    if (getIndexFromInput(index, sides[0]) == visitedIndexes[visitedIndexes.Count -1]) {
                        visitedIndexes.Add(index);
                        AddOrRemoveWater(buttons[index], true);
                        indexLast = index;
                        index = getIndexFromInput(index, sides[1]);
                        if (visitedIndexes.Contains(index)) {
                            index = -10;
                        }
                    } else if (getIndexFromInput(index, sides[1]) == visitedIndexes[visitedIndexes.Count - 1]) {
                        visitedIndexes.Add(index);
                        AddOrRemoveWater(buttons[index], true);
                        indexLast = index;
                        index = getIndexFromInput(index, sides[0]);
                        if (visitedIndexes.Contains(index)) {
                            index = -10;
                        }
                    } else {
                        AddOrRemoveWater(buttons[index], false);
                        removeRestOfWater(index);
                        index = -10;
                    }
                } else {
                    index = -10;
                }
            }
        }
        removeRestOfWater(indexLast);
    }
    /// <summary>
    /// Check if the game is finished
    /// </summary>
    public bool CheckIfTheGameIsFinished()
    {
        int withCorrectRotation = 0;

        for (int i = 0; i < winIndexesLength; i++)
        {
            if (buttons[winIndexes[i]].HasCorrectRotation)
                withCorrectRotation += 1;
        }

        if (withCorrectRotation == winIndexesLength) {   
            GameObject gameComplete = Instantiate(GameCompletePrefab);
            gameComplete.transform.SetParent(canvas,false);

            // Instantiate the continue button prefab
            // The index of continue button in the Unity hierarchy
            continueButton.SetActive(true);
            backdrop.SetActive(true);
            quitButton.enabled = false;
            MinigameCompleteEventChannel.RaiseEvent(new MinigameCompleteEvent(currentController));
            return true;
        }
        return false;
        //Debug.Log(withCorrectRotation == winIndexesLength ? "Game Finished" : "Not Finished");
    }
    public void restartPipeGame() { 
        // rotate all the pipes
        for (int i = 0; i < 16; i++) {
            // Add a random rotation based on the type of pipe
            switch (buttons[i].PipeButtonType) {
                // Angled pipes have 4 possible rotations
                case EPipeButtonType.Angled: {
                    // Assign to rotationIndex a random value[0,4) 
                    int rotationIndex = UnityEngine.Random.Range(0, 4);
                    // Keep doing it until the generated value it's different from the correct pattern rotation
                    while (rotationIndex == (int)pattern[i].rotation) {
                        rotationIndex = UnityEngine.Random.Range(0, 4);
                    }
                    buttons[i].Pivot.Rotate(0, 0, rotationIndex * 90f);
                    }
                    break;
                // Straight pipes have 2 possible rotations
                case EPipeButtonType.Straight: {
                        int rotationIndex = UnityEngine.Random.Range(0, 2);
                        buttons[i].Pivot.eulerAngles = Vector3.zero;
                    }
                    break;
                // The empty button CAN NOT rotate
                default: {
                        buttons[i].Pivot.Rotate(0, 0, 0);
                    }
                    break;
            }
        }
        // check rotation and add water
        CheckRotation();
        CheckRotation2();
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
        CheckRotation2();
        CheckIfTheGameIsFinished();
    }

    private void OnMinigameOpened(MinigameOpenedEvent evt)
    {
        if (evt.controller == null) return;
        currentController = evt.controller;
    }

    private void OnMinigameClosed(CloseGameEvent evt)
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;

        // Returning to the level, hide the cursor
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
}