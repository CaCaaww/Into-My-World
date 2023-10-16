using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LVL4_PipesGameController : MonoBehaviour
{
    [SerializeField]
    private List<LVL4_PipesGamePatternSO> gamePatternsSO;
    [Tooltip("The panel")]
    [SerializeField]
    private Transform gameGrid;
    [SerializeField]
    private List<LVL4_PipesButtonController> buttons = new();
    
    // Start is called before the first frame update
    void Start()
    {
        AddButtons();
    }


    void AddButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject button = Instantiate(gamePatternsSO[0].Pattern[i].button, gameGrid);
            //button.transform.SetParent(gameGrid, false);

            buttons.Add(button.GetComponent<LVL4_PipesButtonController>());
            //add the sprite of the cardCover
            switch (buttons[i].PipeButtonType)
            {
                case EPipeButtonType.Angled:
                    {
                        int rotationIndex = Random.Range(0, 4);
                        buttons[i].Pivot.Rotate(0, 0, rotationIndex*90f);
                    }
                    break;
                case EPipeButtonType.Straight:
                    {
                        int rotationIndex = Random.Range(0, 2);
                        buttons[i].Pivot.Rotate(0, 0, rotationIndex * 90f);
                    }
                    break;
                default:
                    {
                        buttons[i].Pivot.Rotate(0, 0, 0);
                    }
                    break;
            }
            //Debug.Log((float)gamePatternsSO[0].Pattern[i].rotation);
            //Debug.Log(i);
            //Debug.Break();
            //buttons[i].Pivot.Rotate(0, 0, 90);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
