using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MathWarGameController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private TextMeshProUGUI playerScoreRef;
    [SerializeField]
    private TextMeshProUGUI enemyScoreRef;
    // Start is called before the first frame update
    void Start()
    {
        //playerStartPos = player.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void whenClicked()
    {

        Debug.Log("click");
        //playerScore.text = "risultato";
        int playerScore = int.Parse(playerScoreRef.text);
        int enemyScore = int.Parse(enemyScoreRef.text);
        if (playerScore > enemyScore)
        {
            //Debug.Log("it works");
            player.transform.position = transform.position + Vector3.down * 70f;
            this.gameObject.SetActive(false);
            playerScore += enemyScore;
            enemy.SetActive(false);
            Debug.Log(playerScore);
            playerScoreRef.text = playerScore.ToString();
        }
        else
        {
            //Debug.Log(playerStartPos.transform.position);
            player.transform.position = new Vector3(500, 300, 0);

        }
    }
}
