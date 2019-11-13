using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildableItem))]
public class SingleObjectBuilder : MonoBehaviour
{
    public Vector3 buildOffset;

    private bool isBuilding;
    private BuildableItem item;

    public void Awake()
    {
        item = this.GetComponent<BuildableItem>();
        if(item.isMoveable)
        {
            item.contextActions.Add("Move", PickObjectUp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If the object is being built
        if(isBuilding)
        {
            this.transform.position = BuildHelper.GetMousePoint() + buildOffset;

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

    public void StartBuilding()
    {
        item.ShowPrice();
        isBuilding = true;
    }

    public void StopBuilding()
    {
        item.HidePrice();
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
}
