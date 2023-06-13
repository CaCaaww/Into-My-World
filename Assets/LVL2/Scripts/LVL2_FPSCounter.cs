using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class LVL2_FPSCounter : MonoBehaviour
{
    //text to be modified to display the fps
    
    private TextMeshProUGUI fpsCounterText;

    public float updateInterval = 0.5f; //How often should the number update

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;


    // Use this for initialization
    void Start()
    {
        timeleft = updateInterval;
        fpsCounterText = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = Mathf.RoundToInt(accum / frames);
            fpsCounterText.text = fps.ToString() + " FPS";
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}

