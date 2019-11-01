using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseCollector : MonoBehaviour
{
    private List<GameObject> objectsInCollector = new List<GameObject>();

    public void OnTriggerEnter(Collider other)
    {
        objectsInCollector.Add(other.gameObject);  
    }


}
