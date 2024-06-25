using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    #region variables
    [SerializeField] private List<Canvas> miniGames; // list containing the miniGames
    private Canvas currentGame; // Canvas for the current game
    private int randNum; // the random number which is the index of which game of the list we are using
    private bool firstTime = true; // true if this is the first time the lock on the cell is being clicked

    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<MinigameCompleteEvent> MinigameCompleteEventChannel;
    [SerializeField] private GenericEventChannelSO<MinigameOpenedEvent> MinigameOpenedEventChannel;
    [SerializeField] private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    #endregion
    #region general functions
    private void OnEnable() {
        MinigameCompleteEventChannel.OnEventRaised += OnMinigameComplete;
        CloseGameEventChannel.OnEventRaised += OnGameClose;
    }
    void Start()
    {
        // picks a random minigame and makes it the current game for the lock
        randNum = RandomNumberGenerator.GetInt32(0, miniGames.Count);
        currentGame = Instantiate(miniGames[randNum]);
    }
    #endregion
    #region helper functions
    public void lockClicked() {
        currentGame.gameObject.GetComponent<Canvas>().enabled = true; // sets the canvas to be visible so the game can be seen
        MinigameOpenedEventChannel.RaiseEvent(new MinigameOpenedEvent(this));
        if (!firstTime) { // if this is not the first time playing the game, the game is reset to its original state
            switch (randNum) { // makes sure the game is not already finished so it doesn't reset a completed game
                case 0:
                    if (!currentGame.gameObject.GetComponent<LVL4_PipesGameController>().CheckIfTheGameIsFinished()) {
                        currentGame.gameObject.GetComponent<LVL4_PipesGameController>().restartPipeGame();
                    }
                    break;
                case 1:
                    if (!currentGame.gameObject.GetComponent<MemoryGameController>().CheckIfTheGameISFinished()) {
                        currentGame.gameObject.GetComponent<MemoryGameController>().restartMemoryGame();
                    }
                    break;
                case 2:
                    if (!currentGame.gameObject.GetComponent<MathWarGameController>().checkIfGameWon()) {
                        currentGame.gameObject.GetComponent<MathWarGameController>().restartMathsGame();
                    }
                    break;
            }
        } else { // else it is no longer the first time
            firstTime = false;
        }
        LVL4Manager.instance.TogglePlayerInput(); // locking player input
        Cursor.visible = true; // freeing the cursor and making it visible
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnMinigameComplete(MinigameCompleteEvent evt) {
        if (evt.controller == null) return;
        if (evt.controller == this) {
            gameObject.SetActive(false);
        }  
    }
    private void OnGameClose(CloseGameEvent evt) {
        currentGame.gameObject.GetComponent<Canvas>().enabled = false;
        LVL4Manager.instance.TogglePlayerInput(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
}
