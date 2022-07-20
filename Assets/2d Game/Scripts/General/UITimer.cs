using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UITimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] public string startTime;       //To Compare the start time in other classes like PlayAttack -> Like to start potion atack

    private float currentTime;
    public bool timerStarted = false;               //To check if the timer started to not spam lol

    void Start()
    {
        currentTime = (float)Convert.ToDouble(startTime);
    }

    void Update()
    {
        if (timerStarted)
        {
            currentTime -= Time.deltaTime;
        }
        if (currentTime <= 0)   
        {
            timerStarted = false;
            currentTime = (float)Convert.ToDouble(startTime);
        }      
        timerText.text = $"{currentTime:F2}"; 
    }
}
