using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerMoneyManager")]
public class PlayerMoneyManager : ScriptableObject
{
    private static PlayerMoneyManager instance;

    [SerializeField]
    private float value;
    public bool ResetOnRestart;
    public float restartValue;

    public static event Action UpdatedMoney;

    public void OnEnable()
    {
        instance = this;
        value = restartValue;
    }

    public void AddMoney(float moneyToAdd)
    {
        value += moneyToAdd;
        UpdatedMoney();
    }

    public void SubtractMoney(float moneyToSubtract)
    {
        value -= moneyToSubtract;
        UpdatedMoney();
    }

    public float GetMoney()
    {
        return value;
    }

    public static PlayerMoneyManager Instance()
    {
        return instance;
    }
}
