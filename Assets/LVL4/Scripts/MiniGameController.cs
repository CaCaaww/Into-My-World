using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    #region Inspector
    [SerializeField] private GameObject completeCageObject;
    [SerializeField] private List<Minigame> miniGames; // list containing the miniGames

    [SerializeField, Tooltip("Face textures for the prisoner")]
    private Texture2D sadFace, happyFace;
    [SerializeField]
    private List<GameObject> prisonerModels;

    [Header("Listening Event Channels")]
    [SerializeField] protected MinigameCompleteEventChannel minigameCompleteEventChannel;
    [SerializeField] protected MinigameOpenedEventChannel minigameOpenedEventChannel;
    [SerializeField] protected CloseMinigameEventChannel closeMinigameEventChannel;
    #endregion

    #region Private Variables
    private Minigame game; // Canvas for the current game
    private int randNum; // the random number which is the index of which game of the list we are using
    private bool firstTime = true; // true if this is the first time the lock on the cell is being clicked
    private MeshRenderer facePlate;
    private GameObject prisonerModel;
    #endregion

    #region Unity Methods
    void Start()
    {
        prisonerModel = Instantiate(prisonerModels[Random.Range(0, prisonerModels.Count)], this.transform);
        foreach (MeshRenderer i in prisonerModel.GetComponentsInChildren<MeshRenderer>()) {
            if (i.gameObject.name.Contains("Face_Plate") || i.gameObject.name.Contains("FacePlate") ) {
                facePlate = i;
                break;
            }
        }
        facePlate.material.mainTexture = sadFace;
        GetComponent<Animator>().Rebind();

        // picks a random minigame and makes it the current game for the lock
        randNum = RandomNumberGenerator.GetInt32(0, miniGames.Count);
        game = Instantiate(miniGames[randNum]);
    }

    private void OnEnable()
    {
        minigameCompleteEventChannel.OnEventRaised += OnMinigameComplete;
        closeMinigameEventChannel.OnEventRaised += OnMinigameClosed;
    }

    private void OnDisable()
    {
        minigameCompleteEventChannel.OnEventRaised -= OnMinigameComplete;
        closeMinigameEventChannel.OnEventRaised -= OnMinigameClosed;
    }
    #endregion

    #region Public Methods
    public void LockClicked()
    {
        game.gameObject.GetComponent<Canvas>().enabled = true; // sets the canvas to be visible so the game can be seen
        minigameOpenedEventChannel.RaiseEvent(new MinigameOpenedEvent(this));

        /* ========================== SEND DATA TO SERVER HERE ==============================*/

        if (!firstTime)
        { // if this is not the first time playing the game, the game is reset to its original state
            if (!game.IsFinished())
            {
                game.Restart();
            }
        }
        else
        { // else it is no longer the first time
            firstTime = false;
        }
    }
    #endregion

    #region Callbacks
    private void OnMinigameComplete(MinigameCompleteEvent evt)
    {
        if (evt.game == game)
        {
            prisonerModel.transform.localPosition -= new Vector3(2f, 0f, 0f);
            facePlate.material.mainTexture = happyFace;
            GetComponent<Animator>().SetBool("prisonerFreed", true);
        }
    }

    private void OnMinigameClosed()
    {
        game.gameObject.GetComponent<Canvas>().enabled = false;
    }
    #endregion
}
