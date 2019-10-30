using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiator : MonoBehaviour
{
    public GameObject objectToBuild;

    public GameObject InstantiateObject()
    {
        GameObject newObject = Instantiate(objectToBuild, this.transform.position, Quaternion.identity);
        return newObject;
    }
}
