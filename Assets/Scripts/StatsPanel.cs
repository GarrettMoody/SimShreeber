using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StatsPanel : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    public void Start()
    {
        GameManager.UpdatedMoney += GameManager_UpdatedMoney;
    }

    private void GameManager_UpdatedMoney(float obj)
    {
        moneyText.text = obj.ToString("C");
    }
}
