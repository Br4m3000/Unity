using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class WeatherController : MonoBehaviour {

    [Header("Rain Settings")]
    public GameObject rain;
    public bool allowRain;

    [Space(10)]
    [Header("Debugging")]
    public bool isRaining;

    public bool startRain; //Button to start rain
    public bool stopRain;

    void Update()
    {
        if (startRain)
        {
            StartRain();
            startRain = false;
        }

        if (stopRain)
        {
            StopRain();
            stopRain = false;
        }
    }

    void Start () {
        isRaining = false;
	}

    public void StartRain()
    {     
        if(!isRaining && allowRain)
        {
            Debug.Log("Start rain");
            Instantiate(rain);
            isRaining = true;
        }
        
    }

    public void StopRain()
    {
        if (isRaining)
        {
            Debug.Log("Stop rain");
            Destroy(GameObject.FindGameObjectWithTag("RainParent"));
            isRaining = false;
        }        
    }   
}
