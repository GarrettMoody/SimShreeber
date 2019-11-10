using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildableItem))]
public class SingleObjectBuilder : MonoBehaviour
{
    public Vector3 buildOffset;

    private FloatingText priceText;
    private bool isBuilding;
    private bool isShowingPrice;

    private GameObject clickMenuPrefab;
    private GameObject priceTextPrefab;
    private BuildableItem item;


    public void Awake()
    {
        priceTextPrefab = (GameObject)Resources.Load("Prefabs/FloatingText");
        clickMenuPrefab = (GameObject)Resources.Load("Prefabs/ObjectClickMenu");
        item = this.GetComponent<BuildableItem>();
    }

    public void OnDestroy()
    {
        UnsubscribeToObjectClickMenu();
    }

    // Update is called once per frame
    void Update()
    {
        //If the object is being built
        if(isBuilding)
        {
            this.transform.position = BuildHelper.GetMousePoint() + buildOffset;

            if (isShowingPrice)
            {
                priceText.transform.position = BuildHelper.GetMousePoint() + new Vector3(0, .2f, 0);
            }

            //Left Click - Place Object
            if (Input.GetMouseButtonDown(0) && !BuildHelper.IsPointerOverGameObject())
            {
                if(CanBuild())
                {
                    SetObjectDown();
                }
            }
            //Right Click - Cancel Build
            if(Input.GetMouseButtonDown(1))
            {
                Destroy(this.gameObject);
                StopBuilding();
            }
            //Tab Key - Rotate Object
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                this.transform.Rotate(Vector3.up, 45f);
            }
        }
    }

    public void OnMouseOver()
    {
        //On left click on object
        if (Input.GetMouseButtonDown(1))
        {
            //If mouse not over UI component
            if (!BuildHelper.IsPointerOverGameObject())
            {
                //If Context menu exists
                if (ObjectClickMenu.Instance() != null)
                {
                    HideObjectClickMenu();
                }

                //Create new instance of context menu and subscribe to its events
                Instantiate(clickMenuPrefab, BuildHelper.GetMousePointGameObject().transform).GetComponent<ObjectClickMenu>();
                SubscribeToObjectClickMenu();
            }
        }
    }

    private void ObjectClickMenu_SellButtonClicked()
    {
        PlayerMoneyManager.Instance().AddMoney(item.sellPrice);
        Destroy(this.gameObject);
        HideObjectClickMenu();
    }

    private void ObjectClickMenu_MoveButtonClicked()
    {
        PickObjectUp();
        HideObjectClickMenu();
    }

    private void HideObjectClickMenu()
    {
        UnsubscribeToObjectClickMenu();
        if (ObjectClickMenu.Instance().gameObject != null)
        {
            ObjectClickMenu.RemoveAllEvents();
            Destroy(ObjectClickMenu.Instance().gameObject);
        }
    }

    public void StartBuilding()
    {
        ShowPrice();
        isBuilding = true;
    }

    public void StopBuilding()
    {
        HidePrice();
        isBuilding = false;
    }

    public bool CanBuild()
    { 
        //If there is a price on the objec
        if(item.price > 0)
        {
            //Return if we have enough money
            return PlayerMoneyManager.Instance().GetMoney() >= item.price;
        } else
        {
            return true;
        }
    }

    public void SellObject()
    {
        Destroy(this.gameObject);
        PlayerMoneyManager.Instance().AddMoney(item.sellPrice);
    }

    public void ShowPrice()
    {
        if(priceTextPrefab != null && item.price > 0)
        {
            priceText = Instantiate(priceTextPrefab).GetComponent<FloatingText>();
            priceText.SetText(item.price.ToString("C"));
            isShowingPrice = true;
        }
    }

    public void HidePrice()
    {
        if(priceText != null)
        {
            Destroy(priceText.gameObject);
            isShowingPrice = false;
        }
    }

    public void SetObjectDown()
    {
        if (item.price > 0 && !item.isItemPaid)
        {
            PlayerMoneyManager.Instance().SubtractMoney(item.price);
            item.isItemPaid = true;
        }

        foreach (Collider colliders in this.GetComponents<Collider>())
        {
            colliders.enabled = true;
        }
        foreach (Collider colliders in this.GetComponentsInChildren<Collider>())
        {
            colliders.enabled = true;
        }
        if (this.GetComponent<Rigidbody>() != null)
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
        StopBuilding();
    }

    public void PickObjectUp()
    {
        foreach (Collider colliders in this.GetComponents<Collider>())
        {
            colliders.enabled = false;
        }
        foreach (Collider colliders in this.GetComponentsInChildren<Collider>())
        {
            colliders.enabled = false;
        }
        if (this.GetComponent<Rigidbody>() != null)
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
        isBuilding = true;
    }

    private void SubscribeToObjectClickMenu()
    {
        ObjectClickMenu.MoveButtonClicked += ObjectClickMenu_MoveButtonClicked;
        ObjectClickMenu.SellButtonClicked += ObjectClickMenu_SellButtonClicked;
    }

    private void UnsubscribeToObjectClickMenu()
    {
        ObjectClickMenu.MoveButtonClicked -= ObjectClickMenu_MoveButtonClicked;
        ObjectClickMenu.SellButtonClicked -= ObjectClickMenu_SellButtonClicked;
    }
}
