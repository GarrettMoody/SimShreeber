using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseMaker : MonoBehaviour
{
    public ObjectInstantiator objectInstantiator;
    public CooldownBar cooldownBar;
    
    public GameObject MakeCheese()
    {
        if(!cooldownBar.isCooling)
        {
            cooldownBar.StartCooling();
            return objectInstantiator.InstantiateObject();
        } else
        {
            return null;
        }
    }

    public void OnMouseDown()
    {
        GameObject newCheese = MakeCheese();
        if(newCheese != null)
        {
            newCheese.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

   
}
