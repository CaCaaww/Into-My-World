using Pinwheel.Jupiter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL2_LightManager : MonoBehaviour
{
    [SerializeField]
    private JDayNightCycle jDayNightCycle;

    private bool day;
    private bool prevDay;

    private GameObject[] LightsDay;
    private GameObject[] LightsNight;



    //get and set up the lights
    //was thinking of using an event system for each light, but instead decided to use an array
    private void Start()
    {
        if (jDayNightCycle is null)
        {
            jDayNightCycle = gameObject.GetComponent<JDayNightCycle>();
        }

       LightsDay = GameObject.FindGameObjectsWithTag("Day");
       LightsNight = GameObject.FindGameObjectsWithTag("Night");


        if (jDayNightCycle.Time < 6 || jDayNightCycle.Time > 18)
        {
            day = false;
        }
        else
        {
            day = true;
        }

        //set up the lights depending on the time of day
        if (day)
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

    private void Update()
    {

        if(jDayNightCycle.Time < 6  || jDayNightCycle.Time > 18)
        {
            prevDay = day;
            day = false;
        }
        else
        {
            prevDay = day;
            day = true;      
        }

        if(day && !prevDay)
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
        else if(!day && prevDay)
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
