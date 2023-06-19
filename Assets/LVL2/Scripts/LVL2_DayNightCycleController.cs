using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LVL2_DayNightCycleController : MonoBehaviour
{

    #region Inspector
   
    // throws an event to be used on the swap of day to night
    public UnityEvent onDayNightSwap;

    /// <summary>
    /// Day Night Global SO
    /// </summary>
    [Tooltip("SO for the Global Day Night")]
    [SerializeField] private LVL2_DayNightSO dayNightSO;
   
   
    /// <summary>
    /// Time span in hours for the cycle until it swaps
    /// </summary>
    [Tooltip("Time span in hours for the cycle")]
    [Range(0, 23)]
    [SerializeField] private int timeSpan = 12;
   
    /// <summary>
    /// Speed at which the cycle progresses
    /// </summary>
    [Tooltip("Time span in hours for the cycle")]
    [SerializeField] private float speed = 1;



    #endregion


    #region Private Variables
    private float currentTime;
    #endregion


    #region Unity Methods
    void Start()
    {
        currentTime = 0;
    }

    void Update()
    {
        currentTime += Time.deltaTime * speed;

        //if the time is greater than
        if (currentTime > timeSpan)
        {
            currentTime = 0;
            dayNightSO.day = !dayNightSO.day;
            onDayNightSwap.Invoke();
        }
    }
    #endregion
}
