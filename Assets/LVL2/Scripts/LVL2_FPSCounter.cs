using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LVL2_FPSCounter : MonoBehaviour
{
    //text to be modified to display the fps
    
    private TextMeshProUGUI fpsCounterText;
    private float pollingTime = 1f; //how often the fps will update
    private float time;
    private int frameCount;


    private void Start()
    {
        fpsCounterText = this.gameObject.GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        frameCount++;

        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsCounterText.text = frameRate.ToString() +" FPS";

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
