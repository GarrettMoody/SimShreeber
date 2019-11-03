using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static event Action<float> UpdatedMoney = delegate { };

    public WarehouseManager warehouseManager;
    public float money { get; private set;}

    public void Start()
    {
        money = 1000f;
        UpdatedMoney(money);
    }

    public void AddMoney(float moneyToAdd)
    {
        money += moneyToAdd;
        UpdatedMoney(money);
    }

    public void SubtractMoney(float moneyToSubtract)
    {
        money -= moneyToSubtract;
        UpdatedMoney(money);
    }
}
