using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectClickMenu : MonoBehaviour
{
    private static ObjectClickMenu instance;

    public Button moveButton;
    public Button sellButton;

    public static event Action MoveButtonClicked;
    public static event Action SellButtonClicked;

    public void Awake()
    {
        instance = this;
    }

    public void OnMoveButtonClicked()
    {
        MoveButtonClicked();
    }

    public void OnSellButtonClicked()
    {
        SellButtonClicked();
    }

    public static ObjectClickMenu Instance()
    {
        return instance;
    }

    public static void RemoveAllEvents()
    {
        MoveButtonClicked = null;
        SellButtonClicked = null;
    }
}
