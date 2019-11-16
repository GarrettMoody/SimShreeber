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
        UpdateBuyButton();
    }

    public void OnBuyButtonClicked()
    {
        //If there is enough money and shelf slots available
        if (totalPriceValue <= playerMoney.GetMoney() && cheeseInputValue <= gameManager.warehouseManager.shelfSlotsRemaining)
        {
            playerMoney.SubtractMoney(totalPriceValue);
            gameManager.warehouseManager.AddPackagesToShelves(cheeseInputValue);
        }

        UpdateBuyButton();
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
        if(cheeseInputValue > gameManager.warehouseManager.shelfSlotsRemaining)
        {
            cheeseInputValue = (int)gameManager.warehouseManager.shelfSlotsRemaining;
        }
        cheeseInputField.text = cheeseInputValue.ToString();
        CalculateTotalPriceValue();
    }

    public void CalculateTotalPriceValue()
    {
        totalPriceValue = cheeseInputValue * pricePerCheeseValue;
        totalPrice.text = totalPriceValue.ToString("C");
    }

    private void UpdateBuyButton()
    {
        if (cheeseInputValue > gameManager.warehouseManager.shelfSlotsRemaining)
        {
            buyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Inventory Full");
            buyButton.enabled = false;
        }
        else if (totalPriceValue > playerMoney.GetMoney())
        {
            buyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Insufficient Funds");
            buyButton.enabled = false;
        } else
        {
            buyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Buy");
            buyButton.enabled = true;
        }
    }

}
