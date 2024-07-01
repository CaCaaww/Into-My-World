using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MathWarGameController : MonoBehaviour
{
    #region Inspector
    [SerializeField, Tooltip("Scriptable Object patterns that hold numbers for the Math Game")]
    private List<LVL4_MathWarGameFloorSO> gameFloorsSO;
    [SerializeField]
    private Transform gameGrid;
    [SerializeField, Tooltip("Floor Object")]
    private GameObject floor;
    [SerializeField, Tooltip("Floor Object 1")]
    private GameObject floor1;
    [SerializeField, Tooltip("Floor Object 2")]
    private GameObject floor2;
    [SerializeField, Tooltip("Floor Object 3")]
    private GameObject floor3;
    [SerializeField, Tooltip("Floor Object 4")]
    private GameObject floor4;
    [SerializeField, Tooltip("Floor Object 5")]
    private GameObject floor5;
    [SerializeField, Tooltip("Floor Object 6")]
    private GameObject floor6;
    [SerializeField, Tooltip("Floor Object 7")]
    private GameObject floor7;
    [SerializeField, Tooltip("Floor Object 8")]
    private GameObject floor8;
    [SerializeField]
    private List<MathWarGameButtonController> buttons;
[   SerializeField]
    private TextMeshProUGUI enemyScoreRef;
    private List<GameObject> floors = new();

    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private GameObject backdrop;
    [SerializeField]
    private GameObject thumb;
    [SerializeField]
    private GameObject continueButton;

    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<MinigameCompleteEvent> MinigameCompleteEventChannel;
    [SerializeField] private GenericEventChannelSO<MinigameOpenedEvent> MinigameOpenedEventChannel;
    [SerializeField] private GenericEventChannelSO<CloseGameEvent> CloseGameEventChannel;
    #endregion

    #region Public & Private Variables
    private MiniGameController currentController;

    public string enemyText;
    public HashSet<int> aliveEnemies = new();
    private int roomNumber;
    #endregion

    #region Unity & Listener methods
    private void OnEnable() {
        MinigameOpenedEventChannel.OnEventRaised += OnMinigameOpened;
        CloseGameEventChannel.OnEventRaised += OnMinigameClosed;
    }
    private void OnMinigameOpened(MinigameOpenedEvent evt) {
        if (evt.controller == null) return;
        currentController = evt.controller;
    }
    private void OnMinigameClosed(CloseGameEvent evt)
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;

        // Returning to the level, hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        // enemyText = floor.GetComponentInChildren<TextMeshProUGUI>().ToString();
        // Debug.Log(enemyText);

        // Add each room with an enemy to the list for easier access
        floors.Add(floor);
        floors.Add(floor1);
        floors.Add(floor2);
        floors.Add(floor3);
        floors.Add(floor4);
        floors.Add(floor5);
        floors.Add(floor6);
        floors.Add(floor7);
        floors.Add(floor8);
        for (int i = 0; i < 9; i++)
        {
            aliveEnemies.Add(i);
        }
        AddFloors();
    }
    #endregion

    #region game methods
    public void restartMathsGame() { 
        for (int i = 0; i < buttons.Count; i++) {
            buttons[i].GetComponentInChildren<MathWarGameButtonController>().isDefeated = false;
            buttons[i].reviveEnemy();
            buttons[i].setPlayerToStart();
        }
        aliveEnemies.Clear();
        for (int i = 0; i < 9; i++) {
            aliveEnemies.Add(i);
        }
        buttons[0].resetPlayerScore();
        
    }

    // Update is called once per frame
    void AddFloors()
    {
        // Select a random Scriptable object to use for the minigame data
        roomNumber = Random.Range(0, 4);
        // Track items in list for getting items from scriptable objects
        int floorNumber = 0;

        // Debug.Log("Floors size: " + floors.Count);
        // Debug.Log("Random room number chosen: " + roomNumber);

        // Column variable for grid layout
        for (int i = 0; i < 3; i++)
        {
            // Row variable for grid layout
            for (int j = 0; j < 3; j++)
            {
                // Debug.Log(gameFloorsSO.Count);
                // Debug.Log(i);
                // Debug.Log(j);

                // Get the floor text from list of floors and set to scriptable object data
                enemyScoreRef = floors[floorNumber].GetComponentInChildren<TextMeshProUGUI>(); 
                enemyText = gameFloorsSO[roomNumber].stages[i].floors[j].ToString();
                enemyScoreRef.text = enemyText;
                floorNumber++;
            }
            // GameObject button = Instantiate(floor, gameGrid);
            // enemyText = "test";
        }
    }

    public bool checkIfGameWon(){
        
        
        int defeated = 0;
        for (int i = 0; i < buttons.Count; i++)
        {
            // Debug.Log(buttons[i].GetComponentInChildren<MathWarGameButtonController>().isDefeated);
            if (buttons[i].GetComponentInChildren<MathWarGameButtonController>().isDefeated == true)
            {
                // Increments if an enemy was defeated this turn
                defeated++;
            }
        }
        if (defeated == floors.Count)
        {
            quitButton.enabled = false;
            backdrop.SetActive(true);
            thumb.SetActive(true);
            continueButton.SetActive(true);
            MinigameCompleteEventChannel.RaiseEvent(new MinigameCompleteEvent(currentController));
            return true;
        }
        return false;
    }
    #endregion
}
