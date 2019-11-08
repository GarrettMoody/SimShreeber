using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packager : MonoBehaviour
{
	public int requiredItems;
    public ObjectInstantiator packageInstantiator;

    private int itemCount;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<CheeseBlock>() != null)
        {
            Destroy(collider.gameObject);
            itemCount++;
            if (itemCount == requiredItems)
            {
                PackageItems();
            }
        }
    }

    public Package PackageItems()
    {
        itemCount = 0;
        return packageInstantiator.InstantiateObject().GetComponent<Package>();
    }
 }
