using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MathWarGameController : MonoBehaviour
{
    [SerializeField]
    private List<LVL4_MathWarGameFloorSO> gameFloorsSO;
    [SerializeField]
    private Transform gameGrid;
    //[SerializeField]
    //private int levelIndex;
    [SerializeField]
    private GameObject floor;

    public string enemyText;
    // Start is called before the first frame update
    void Start()
    {
        enemyText = floor.GetComponentInChildren<TextMeshProUGUI>().ToString();

        AddFloors();
    }

    // Update is called once per frame
    void AddFloors()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject button = Instantiate(floor, gameGrid);
            enemyText = "test";
        }
    }
}
