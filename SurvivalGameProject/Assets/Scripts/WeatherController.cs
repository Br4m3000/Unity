using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class WeatherController : MonoBehaviour {

    [Header("Rain Settings")]
    public GameObject rainParent;
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
        if(!isRaining && allowRain && !rainParent.activeSelf)
        {
            Debug.Log("Start rain");
            rainParent.SetActive(true);
            isRaining = true;
        }
    }

    public void StopRain()
    {
        if (isRaining && rainParent.activeSelf)
        {
            Debug.Log("Stop rain");
            rainParent.SetActive(false);
            isRaining = false;
        }
    }   
}
