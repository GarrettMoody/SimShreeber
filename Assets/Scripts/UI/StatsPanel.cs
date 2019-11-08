using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsPanel : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public PlayerMoneyManager playerMoney;

    public void Start()
    {
        PlayerMoneyManager.UpdatedMoney += PlayerMoneyManager_UpdatedMoney;
		PlayerMoneyManager_UpdatedMoney();

	}

    private void PlayerMoneyManager_UpdatedMoney()
    {
        moneyText.text = playerMoney.GetMoney().ToString("C");
    }
}
