using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class BuildableItem : MonoBehaviour
{
    public string itemName;
    public float price;
    public float sellPrice;
    public bool isMoveable;
    public bool canPlaceObjectsOn;
    [HideInInspector]
    public bool isItemPaid;
    public Dictionary<string, Action> contextActions = new Dictionary<string, Action>();

    private FloatingText priceText;
    private GameObject priceTextPrefab;
    private bool isShowingPrice;
    private GameObject clickMenuPrefab;

    public void Awake()
    {
        priceTextPrefab = (GameObject)Resources.Load("Prefabs/FloatingText");
        clickMenuPrefab = (GameObject)Resources.Load("Prefabs/ObjectClickMenu");
        contextActions.Add("Sell", SellItem);
    }

    public void Update()
    {
        if (isShowingPrice)
        {
            priceText.transform.position = BuildHelper.GetMousePoint() + new Vector3(0, .2f, 0);
        }
    }

    public void SellItem()
    {
        Destroy(this.gameObject);
        PlayerMoneyManager.Instance().AddMoney(sellPrice);
    }

    public void ShowPrice()
    {
        if (priceTextPrefab != null && price > 0)
        {
            priceText = Instantiate(priceTextPrefab).GetComponent<FloatingText>();
            priceText.SetText(price.ToString("C"));
            isShowingPrice = true;
        }
    }

    public void HidePrice()
    {
        if (priceText != null)
        {
            Destroy(priceText.gameObject);
            isShowingPrice = false;
        }
    }

    public bool IsShowingPrice()
    {
        return isShowingPrice;
    }

    public FloatingText GetPriceText()
    {
        return priceText;
    }

    public void OnMouseOver()
    {
        //On left click on object
        if (Input.GetMouseButtonDown(1))
        {
            //If mouse not over UI component
            if (!BuildHelper.IsPointerOverGameObject())
            {
                //Create new instance of context menu and subscribe to its events
                Instantiate(clickMenuPrefab, BuildHelper.GetMousePointGameObject().transform);
                ObjectClickMenu.OpenContextMenu(this);
            }
        }
    }

}
