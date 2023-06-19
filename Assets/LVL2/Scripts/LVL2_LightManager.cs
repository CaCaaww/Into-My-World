using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL2_LightManager : MonoBehaviour
{
    [Tooltip("SO for the Global Day Night")]
    [SerializeField] private LVL2_DayNightSO dayNightSO;

    private GameObject[] LightsDay;
    private GameObject[] LightsNight;

    //get and set up the lights
    //was thinking of using an event system for each light, but instead decided to use an array
    private void Start()
    {
       LightsDay = GameObject.FindGameObjectsWithTag("Day");
       LightsNight = GameObject.FindGameObjectsWithTag("Night");

        //set up the lights depending on the time of day
        if (dayNightSO.day)
        {
            for(int i = 0; i < LightsDay.Length; i++)
            {
                LightsDay[i].SetActive(true);
            }

            for(int i = 0; i < LightsNight.Length; i++)
            {
                LightsNight[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < LightsDay.Length; i++)
            {
                LightsDay[i].SetActive(false);
            }

            for (int i = 0; i < LightsNight.Length; i++)
            {
                LightsNight[i].SetActive(true);
            }
        }
    }

    public void OnDayNightSwap()
    {
        if(dayNightSO.day)
        {
            Debug.Log("day");
            for (int i = 0; i < LightsDay.Length; i++)
            {
                LightsDay[i].SetActive(true);
            }

            for (int i = 0; i < LightsNight.Length; i++)
            {
                LightsNight[i].SetActive(false);
            }
        }
        else if(!dayNightSO.day)
        {
            Debug.Log("night");
            for (int i = 0; i < LightsDay.Length; i++)
            {
                LightsDay[i].SetActive(false);
            }

            for (int i = 0; i < LightsNight.Length; i++)
            {
                LightsNight[i].SetActive(true);
            }
        }
    }








 }
