using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObjects/TimeManager")]
public class TimeManager : ScriptableObject
{
    [SerializeField]
    private float realTime;
    [SerializeField]
    private float realTimeRounded;
    [SerializeField]
    private bool restartOnStart;
    [SerializeField]
    private DateTime gameTime;
    public string gameTimeText;

    public void OnEnable()
    {
        if (restartOnStart)
        {
            realTime = 0;
        }
    }

    public void UpdateTime()
    {
        realTime += Time.deltaTime;
        realTimeRounded = Mathf.Round(realTime);
        gameTime = new DateTime(1, 1, 1, 0, 0, 0).Date.AddMinutes(realTimeRounded * 5);
        gameTimeText = gameTime.ToString("hh:mm tt 'Day' dd 'Month' MM 'Year' y");
    }
}