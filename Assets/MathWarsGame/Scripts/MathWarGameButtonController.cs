using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MathWarGameButtonController : MonoBehaviour
{
    #region Inspector

    // James Fixing thigns
    [SerializeField] private int column;
    [SerializeField] private int floor;
    [SerializeField] private MathWarGameController mathWarGameController;
    [SerializeField] private GameObject enemy;
    public bool isDefeated = false;
    public int floorValue;
    #endregion

    #region Helper Methods
    public void whenClicked()
    {
        // James Fixing
        if (mathWarGameController.interaction(column, floor)) {
            // Disable button
            this.gameObject.SetActive(false);
            //this.gameObject.GetComponent<Button>().interactable = false;
            // Disable Enemy Sprite
            enemy.SetActive(false);
            // Set Enemy to Defeated
            isDefeated = true;
        }
    }
    public void reviveEnemy() {
        // makes the enemy active again
        enemy.SetActive(true);
        isDefeated = false;
    }

    #endregion
}
