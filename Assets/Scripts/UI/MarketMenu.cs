using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketMenu : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI casesOfCheeseText;
    public TextMeshProUGUI remainingShelvesText;
    public TextMeshProUGUI pricePerCheese;
    public TextMeshProUGUI totalPrice;
    public Button minusButton;
    public Button plusButton;
    public InputField cheeseInputField;
    public Button buyButton;

    public PlayerMoneyManager playerMoney;

    private int cheeseInputValue;
    private float pricePerCheeseValue;
    private float totalPriceValue;

    public void OnEnable()
    {
        casesOfCheeseText.text = gameManager.warehouseManager.casesOfCheese.ToString();
        remainingShelvesText.text = gameManager.warehouseManager.shelfSlotsRemaining.ToString();
        cheeseInputValue = 1;
        cheeseInputField.text = cheeseInputValue.ToString();
        pricePerCheeseValue = 27.00f;
        pricePerCheese.text = pricePerCheeseValue.ToString("C");
        CalculateTotalPriceValue();
    }

    public void OnBuyButtonClicked()
    {
        //If there is enough money and shelf slots available
        if (totalPriceValue <= playerMoney.GetMoney() && cheeseInputValue <= gameManager.warehouseManager.shelfSlotsRemaining)
        {
            playerMoney.SubtractMoney(totalPriceValue);
            gameManager.warehouseManager.AddPackagesToShelves(cheeseInputValue);
        }

        this.gameObject.SetActive(false);
    }

    public void OnMinusButtonClicked()
    {
        cheeseInputValue--;
        if(cheeseInputValue < 0)
        {
            cheeseInputValue = 0;
        }
        cheeseInputField.text = cheeseInputValue.ToString();
        CalculateTotalPriceValue();
    }

    public void OnPlusButtonClicked()
    {
        cheeseInputValue++;
        cheeseInputField.text = cheeseInputValue.ToString();
        CalculateTotalPriceValue();
    }

    public void CalculateTotalPriceValue()
    {
        totalPriceValue = cheeseInputValue * pricePerCheeseValue;
        totalPrice.text = totalPriceValue.ToString("C");
    }



}
