using System.Collections.Generic;
using System.Drawing.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MathWarGameController : Minigame
{
    #region Inspector
    [SerializeField, Tooltip("Scriptable Object patterns that hold numbers for the Math Game")]
    private List<LVL4_MathWarGameFloorSO> gameFloorsSO;
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
    private GameObject thumb;
    [SerializeField]
    private GameObject continueButton;

    //James Trying to fix whatever this mess is
    [SerializeField] private GenericEventChannelSO<QuitButtonEvent> quitButtonEventChanel;
    [SerializeField] private List<List<GameObject>> columns = new List<List<GameObject>>();
    //This stores all the data of the floors. It is oriented as columns, each containing 3 floors. 
    // in the puzzle it is top-down. So column[0][0] is the top left corner, and column[0][1] is the space below that, and etc...
    [SerializeField] private GameObject player;
    private int playerScore;
    public int currentColumn; //the current column of the player
    private int currentRoomInColumn; // the current floor of the player in the current column
    public int enemiesDefeatedInCurrentColumn; // the number of enemies defeated in this column

    #endregion

    #region Public & Private Variables
    private int roomNumber;
    #endregion

    #region Unity & Listener 
    void Start()
    {     
        //James Fixing stuff
        playerScore = 10;
        updatePlayerScore();
        currentColumn = 0;
        currentRoomInColumn = 0;
        enemiesDefeatedInCurrentColumn = 0;
        List<GameObject> column0 = new List<GameObject>();
        List<GameObject> column1= new List<GameObject>();
        List<GameObject> column2 = new List<GameObject>();
        column0.Add(floor);
        column0.Add(floor1);
        column0.Add(floor2);
        column1.Add(floor3);
        column1.Add(floor4);
        column1.Add(floor5);
        column2.Add(floor6);
        column2.Add(floor7);
        column2.Add(floor8);
        columns.Add(column0);
        columns.Add(column1);
        columns.Add(column2);
        AddFloors();
        
    }
    public void QuitButtonClicked() {
        Debug.Log("Quit Button Clicked");
        quitButtonEventChanel.RaiseEvent(new QuitButtonEvent());
    }
    #endregion

    #region game methods

    public bool interaction(int columnNum, int floorNum) {
        // check to make sure that they are in the same 
        if (columnNum != currentColumn) {
            // if not in the same column, make sure that they player is able to be moved up
            if (enemiesDefeatedInCurrentColumn == 3 && columnNum == currentColumn+1) {
                if (playerScore > getScoreAtLocation(columnNum, floorNum)) {
                    setPlayerPosition(columnNum, floorNum);
                    playerScore += getScoreAtLocation(columnNum, floorNum);
                    updatePlayerScore();
                    currentColumn = columnNum;
                    enemiesDefeatedInCurrentColumn = 1;
                    currentRoomInColumn = floorNum;
                    return true;
                } else {
                    sendPlayerToStart();
                    return false;
                }
            } else {
                return false;
            }
        } else {
            if (playerScore > getScoreAtLocation(columnNum, floorNum)) {
                setPlayerPosition(columnNum, floorNum);
                playerScore += getScoreAtLocation(columnNum, floorNum);
                updatePlayerScore();
                enemiesDefeatedInCurrentColumn++;
                currentRoomInColumn = floorNum;
                IsFinished();
                return true;
            } else {
                sendPlayerToStart();
                return false;
            }
        }

    }
    private void setPlayerPosition(int columnNum, int floorNum) {
        //Debug.Log("Setting Player to Position: " + (columns[columnNum][floorNum].transform.position + Vector3.down * 70f));
        if (columnNum == 0 && floorNum == 0) {
            player.transform.position = new Vector3(1490f, 1460f, 0f);
        } else {
            player.transform.position = (columns[columnNum][floorNum].transform.position + (Vector3.down * 70f));
        }
    }
    private void sendPlayerToStart() {
        //Debug.Log("Sending Player to Start");
        player.transform.position = player.transform.position = new Vector3(300, 300, 0);
    }
    private void updatePlayerScore() {
        player.GetComponentInChildren<TextMeshProUGUI>().text = playerScore.ToString();
    }
    private int getScoreAtLocation(int columnNum, int floorNum) {
        return columns[columnNum][floorNum].GetComponentInChildren<MathWarGameButtonController>().floorValue;
    }
    public override void Restart()
    {
        // James Fixing 
        foreach (List<GameObject> column in columns) {
            foreach (GameObject floor in column) {
                floor.transform.GetChild(4).gameObject.SetActive(true);
                floor.GetComponentInChildren<MathWarGameButtonController>().reviveEnemy();
            }
        }
        playerScore = 10;
        updatePlayerScore();
        currentColumn = 0;
        currentRoomInColumn = 0;
        enemiesDefeatedInCurrentColumn = 0;
        sendPlayerToStart();
        /* ========================== SEND DATA TO SERVER HERE ==============================*/

    }

    // Update is called once per frame
    void AddFloors()
    {
        //James fixing stuff
        roomNumber = Random.Range(0, 4); // selecting a random number that will be used to identify a random UI
        for (int i = 0; i < 3; i++) {
            for (int k = 0; k < 3; k++) {
                //Sets the text on the board to what it is supposed to be according to the SO
                columns[i][k].GetComponentInChildren<MathWarGameButtonController>().floorValue = gameFloorsSO[roomNumber].stages[i].floors[k];
                columns[i][k].GetComponentInChildren<TextMeshProUGUI>().text = gameFloorsSO[roomNumber].stages[i].floors[k].ToString();

            }
        }
    }

    public override bool IsFinished() {
        //James fixing 
        // if all enemies in the last column are defeated, then the game is won
        if (enemiesDefeatedInCurrentColumn == 3 && currentColumn == 2) {
            thumb.SetActive(true);
            continueButton.SetActive(true);
            minigameCompleteEventChannel.RaiseEvent(new MinigameCompleteEvent(this));

            /* ========================== SEND DATA TO SERVER HERE ==============================*/
            ForwardMinigameCompleteData();

            return true;
        }
        return false;
    }
    #endregion
}
