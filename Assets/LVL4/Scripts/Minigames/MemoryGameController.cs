using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameController : Minigame
{
    #region Constants
    private const int numberOfGamePuzzles = 16;
    #endregion

    #region Inspector
    [Header("UI settings")]

    [Header("Prefabs")]
    [Tooltip("Thumb up")]
    [SerializeField]
    private GameObject thumbUpPrefab;
    [Tooltip("Thumb down")]
    [SerializeField]
    private GameObject thumbDownPrefab;
    [Tooltip("Game Completed Prefab")]
    [SerializeField]
    private GameObject GameCopletePrefab;
    [Tooltip("The prefab of the button")]
    [SerializeField]
    private GameObject buttonPrefab;

    [Header("UI References")]
    [Tooltip("The canvas")]
    [SerializeField]
    private Transform canvas;
    [Tooltip("The panel")]
    [SerializeField]
    private Transform puzzleField;

    [Header("Sprites")]
    [Tooltip("The back image of the card")]
    [SerializeField]
    private Sprite cardCover;
    [Tooltip("The sprites of the cards")]
    [SerializeField]
    private Sprite[] puzzleSprites;
    [Tooltip("The button list")]
    [SerializeField]
    private List<Button> buttons = new();
    [Tooltip("The relation between sprites and cards")]
    [SerializeField]
    private List<Sprite> relatedSprites = new();
    [SerializeField] private GenericEventChannelSO<QuitButtonEvent> quitButtonEventChanel;
    #endregion

    #region Private Variables
    // firstGuess and secondGuess are needed to differentiate between the first and the second pick
    private bool firstGuess;
    private bool secondGuess;
    // count the total guesses (corrects and incorrects)(the guess is added after 2 picks)
    private int countGuesses;
    // count the correct guesses
    private int countCorrectGuesses;
    // number of correct guesses needed to finish the game
    private int gameGuesses;
    // name parsed to int of the selected game objects for the first pick
    private int firstGuessIndex;
    // name parsed to int of the selected game objects for the second pick
    private int secondGuessIndex;
    //the sprite of the first card picked
    private string firstGuessPuzzle;
    //the sprite of the second card picked
    private string secondGuessPuzzle;
    //wait for half a second
    private WaitForSeconds waitForHalfSecond = new(.5f);
    //wait for one second
    private WaitForSeconds waitForOnefSecond = new(1f);
    #endregion

    #region Unity & listener Methods
    private void Start()
    {

        AddButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(relatedSprites);
        gameGuesses = numberOfGamePuzzles / 2;
    }

    #endregion

    #region Helper Methods
    public void QuitButtonClicked() {
        Debug.Log("Quit Button Clicked");
        quitButtonEventChanel.RaiseEvent(new QuitButtonEvent());
    }
    public override void Restart()
    {
        foreach (Button b in buttons)
        { // make all the buttons interactable again and turn them face down
            b.interactable = true;
            b.image.enabled = true;
            b.image.sprite = cardCover;
        }
        //reset the guesses
        countCorrectGuesses = countGuesses = 0;
        gameGuesses = numberOfGamePuzzles / 2;

        /* ========================== SEND DATA TO SERVER HERE ==============================*/
    }

    //Populate the panel whit the selected ammount of buttons 
    void AddButtons()
    {
        for (int i = 0; i < numberOfGamePuzzles; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            //gives the button a numerical name
            button.name = "" + i;
            //set the parent off the button
            button.transform.SetParent(puzzleField, false);
            buttons.Add(button.GetComponent<Button>());
            //add the sprite of the cardCover
            buttons[i].image.sprite = cardCover;
        }
    }

    //Add a sprite to 2 buttons
    void AddGamePuzzles()
    {
        // Get the number of buttons in the list
        int looper = buttons.Count;
        // Initialize the index for puzzle sprites
        int index = 0;

        // Loop through all the buttons
        for (int i = 0; i < looper; i++)
        {
            // Check if the index has reached half of the button count
            if (index == looper / 2)
            {
                // If so, reset the index to 0 to start over with the first sprite
                index = 0;
            }
            // Add the current puzzle sprite to the list of related sprites and increment the index
            relatedSprites.Add(puzzleSprites[index]);
            index++;
        }
    }

    //Add the listeners to the buttons
    void AddListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    //Remove the listeners from the buttons, probably will be called at a different time in the complete level 4
    void RemoveListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
        }
    }

    // Check if the game is finished checking if the number of guesses is equal to the number of guesses required
    public override bool IsFinished()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            GameObject GameCoplete = Instantiate(GameCopletePrefab);
            GameCoplete.transform.SetParent(canvas, false);
            minigameCompleteEventChannel.RaiseEvent(new MinigameCompleteEvent(this));
            //Debug.Log("It took you " + countGuesses + " many guesses to finish the game");

            ForwardMinigameCompleteData();
            return true;
        }
        return false;
    }

    // Shuffle the sprites in a random order
    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    #endregion

    #region Callbacks

    //Make a guess 
    public void PickAPuzzle()
    {
        //The name of the current selected object
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        //First pick
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(name);
            firstGuessPuzzle = relatedSprites[firstGuessIndex].name;
            buttons[firstGuessIndex].image.sprite = relatedSprites[firstGuessIndex];

        }
        //Second pick
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(name);
            if (secondGuessIndex == firstGuessIndex)
            {
                secondGuess = false;

            }
            else
            {
                secondGuessPuzzle = relatedSprites[secondGuessIndex].name;
                buttons[secondGuessIndex].image.sprite = relatedSprites[secondGuessIndex];

                countGuesses++;

                StartCoroutine(CheckIfThePuzzlesMatch());
            }
        }
    }
    #endregion

    #region Coroutines

    //Checks if the two pick match
    IEnumerator CheckIfThePuzzlesMatch()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return waitForHalfSecond;
        //Checks if the first pick and the second match and that is not the same button pressed twice
        if (firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex)
        {
            yield return waitForHalfSecond;

            //Set both the buttons as not interactable 
            buttons[firstGuessIndex].interactable = false;
            buttons[secondGuessIndex].interactable = false;

            //Disable both images components
            buttons[firstGuessIndex].image.enabled = false;
            buttons[secondGuessIndex].image.enabled = false;

            //ADD THUMBS UP
            GameObject thumbUp = Instantiate(thumbUpPrefab);
            thumbUp.transform.SetParent(canvas, false);
            yield return waitForOnefSecond;
            Destroy(thumbUp);


            IsFinished();

        }
        else
        {
            //Cover the cards back
            yield return waitForHalfSecond;
            buttons[firstGuessIndex].image.sprite = cardCover;
            buttons[secondGuessIndex].image.sprite = cardCover;

            //ADD THUMBS DOWN
            GameObject thumbDown = Instantiate(thumbDownPrefab);
            thumbDown.transform.SetParent(canvas, false);
            yield return waitForOnefSecond;
            Destroy(thumbDown);
        }
        
        firstGuess = secondGuess = false;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
}
