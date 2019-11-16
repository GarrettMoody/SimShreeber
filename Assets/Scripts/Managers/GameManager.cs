using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WarehouseManager warehouseManager;
    public TimeManager timeManager;

    public static event Action MouseClicked = delegate { };

    private void Update()
    {
        timeManager.UpdateTime();
        if (Input.GetMouseButtonUp(0))
        {
            MouseClicked();
        }
    }
}
