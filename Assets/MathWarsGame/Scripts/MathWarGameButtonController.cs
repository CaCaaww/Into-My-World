using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MathWarGameButtonController : MonoBehaviour
{
    #region Inspector
    [Header("References")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private TextMeshProUGUI playerScoreRef;
    [SerializeField]
    private TextMeshProUGUI enemyScoreRef;
    [SerializeField]
    private MathWarGameController mathWarGameController;
    public bool isDefeated = false;
    

    #endregion

    #region Helper Methods
    public void whenClicked()
    {
        //Player and Enemy Score References
        int playerScore = int.Parse(playerScoreRef.text);
        int enemyScore = int.Parse(enemyScoreRef.text);
        if (playerScore > enemyScore)
        {
            //Teleport Player to the button
            player.transform.position = transform.position + Vector3.down * 70f;
            //Disable Button
            this.gameObject.SetActive(false);
            //Add playerScore and enemyScore
            playerScore += enemyScore;
            //Disable Enemy
            enemy.SetActive(false);
            //Update playerScore UI
            playerScoreRef.text = playerScore.ToString();
            // Marks enemy as defeated to track win or lose
            isDefeated = true;
        }
        else
        {
            //Teleport the player back
            player.transform.position = new Vector3(500, 300, 0);

        }    
        mathWarGameController.checkIfGameWon();
    }



    #endregion
}
