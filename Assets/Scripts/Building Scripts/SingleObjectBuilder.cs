using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObjectBuilder : MonoBehaviour
{
    public Vector3 buildOffset;
    public FloatingText priceTextPrefab;

    private Price price;
    private FloatingText priceText;
    private bool isBuilding;
    private bool isShowingPrice;

    public void Awake()
    {
        if(this.gameObject.GetComponent<Price>() != null)
        {
            price = this.gameObject.GetComponent<Price>();
        }
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
            if (Input.GetMouseButtonDown(0))
            {
                if(CanBuild())
                {
                    if(price != null)
                    {
                        PlayerMoneyManager.Instance().SubtractMoney(price.priceOfObject);
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
        if(price != null)
        {
            //Return if we have enough money
            return PlayerMoneyManager.Instance().GetMoney() >= price.priceOfObject;
        } else
        {
            return true;
        }
    }

    public void ShowPrice()
    {
        if(priceTextPrefab != null && price != null)
        {
            priceText = Instantiate(priceTextPrefab);
            priceText.SetText(price.priceOfObject.ToString("C"));
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
}
