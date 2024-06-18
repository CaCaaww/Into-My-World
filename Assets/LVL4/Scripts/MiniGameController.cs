using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    [SerializeField] private List<Canvas> miniGames;
    //[SerializeField] private GameObject pipeMiniGamePrefab;
    //[SerializeField] private GameObject matchingMiniGamePrefab;
    private Canvas currentGame;
    private int randNum;
    private bool firstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        randNum = RandomNumberGenerator.GetInt32(0, miniGames.Count)
        currentGame = Instantiate(miniGames[randNum]);
        //currentGame.gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void lockClicked() {
        //Debug.Log("Loading");
        currentGame.gameObject.GetComponent<Canvas>().enabled = true;
        if (!firstTime) {
            switch (randNum) {
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
        } else {
            firstTime = false;
        }
        
        LVL4Manager.instance.TogglePlayerInput();
        //Debug.Log(currentGame.gameObject.active);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    
}
