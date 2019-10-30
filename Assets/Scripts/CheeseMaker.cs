using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseMaker : MonoBehaviour
{
    public ObjectInstantiator objectInstantiator;

    public GameObject MakeCheese()
    {
        return objectInstantiator.InstantiateObject();
    }

    public void OnMouseDown()
    {
        GameObject newCheese = MakeCheese();
        newCheese.GetComponent<Rigidbody>().isKinematic = false;
    }
}
