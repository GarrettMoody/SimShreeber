using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheeseMaker : MonoBehaviour
{
    public static Func<bool> MakeCheeseRequest;
    public static event Action CheeseMade = delegate { };

    public ObjectInstantiator objectInstantiator;
    public CooldownBar cooldownBar;
    
    public GameObject MakeCheese()
    {
        if(!cooldownBar.isCooling)
        {
            cooldownBar.StartCooling();
            CheeseMade();
            return objectInstantiator.InstantiateObject();
        } else
        {
            return null;
        }
    }

    public void OnMouseDown()
    {
        bool canMakeCheese = MakeCheeseRequest();
        if(canMakeCheese)
        {
            GameObject newCheese = MakeCheese();
            if (newCheese != null)
            {
                newCheese.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
