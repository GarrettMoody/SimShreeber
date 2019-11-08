using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheeseCollector : MonoBehaviour
{
    public static event Action<Package> PackageInCollector;

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Package>() != null)
        {
            PackageInCollector(other.GetComponent<Package>());
        }
    }
}
