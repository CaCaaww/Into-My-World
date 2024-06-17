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
    // Start is called before the first frame update
    void Start()
    {
        randNum = RandomNumberGenerator.GetInt32(0, miniGames.Count);
        currentGame = Instantiate(miniGames[randNum]);
        //currentGame = Instantiate(miniGames[2]);
        //currentGame.gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void lockClicked() {
        Debug.Log("Loading");
        currentGame.gameObject.GetComponent<Canvas>().enabled = true;
        LVL4Manager.instance.TogglePlayerInput();
        Debug.Log(currentGame.gameObject.active);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    
}
