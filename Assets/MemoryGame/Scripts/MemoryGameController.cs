using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameController : MonoBehaviour
{
    #region Constants
    private const int numberOfGamePuzzles = 16;
    #endregion
    #region Inspector
    [Header("UI settings")]
    /// <summary>
    /// the backgroud image
    /// </summary>
    [Tooltip("the backgroud image")]
    [SerializeField]
    private Sprite cardCover;
    [SerializeField]
    private Sprite[] puzzleSprites;
    [SerializeField]
    private Transform puzzleField;
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private List<Button> buttons = new();
    [SerializeField]
    private List<Sprite> relatedSprites = new();
    #endregion

    #region Private Variables
    private bool firstGuess;
    private bool secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;
    private int firstGuessIndex;
    private int secondGuessIndex;
    private string firstGuessPuzzle;
    private string secondGuessPuzzle;
    private WaitForSeconds waitForHalfSecond = new(.5f);
    #endregion

    #region Unity Methods
    private void OnDisable()
    {
        RemoveListeners();
    }
    
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
    void AddButtons()
    {
        for (int i = 0; i < numberOfGamePuzzles; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.name = "" + i;
            button.transform.SetParent(puzzleField, false);
            buttons.Add(button.GetComponent<Button>());
            buttons[i].image.sprite = cardCover;
        }
    }

    void AddGamePuzzles()
    {
        int looper = buttons.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }
            relatedSprites.Add(puzzleSprites[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }
    void RemoveListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
        }
    }

    void CheckIfTheGameISFinished()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Game Finished!");
            Debug.Log("It took you " + countGuesses + " many guesses to finish the game");
        }
    }

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
    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        //Debug.Log("premuto " + name);
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(name);
            firstGuessPuzzle = relatedSprites[firstGuessIndex].name;
            buttons[firstGuessIndex].image.sprite = relatedSprites[firstGuessIndex];

        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(name);
            secondGuessPuzzle = relatedSprites[secondGuessIndex].name;
            buttons[secondGuessIndex].image.sprite = relatedSprites[secondGuessIndex];

            countGuesses++;

            StartCoroutine(CheckIfThePuzzlesMatch());
        }
    }
    #endregion

    #region Coroutines
    IEnumerator CheckIfThePuzzlesMatch()
    {
        yield return waitForHalfSecond;
        if (firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex)
        {
            yield return waitForHalfSecond;

            buttons[firstGuessIndex].interactable = false;
            buttons[secondGuessIndex].interactable = false;

            buttons[firstGuessIndex].image.enabled = false;
            buttons[secondGuessIndex].image.enabled = false;

            CheckIfTheGameISFinished();

        }
        else
        {
            yield return waitForHalfSecond;
            buttons[firstGuessIndex].image.sprite = cardCover;
            buttons[secondGuessIndex].image.sprite = cardCover;
        }
        yield return waitForHalfSecond;

        firstGuess = secondGuess = false;
    }
    #endregion




    
}
