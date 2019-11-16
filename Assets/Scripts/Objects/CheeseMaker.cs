using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheeseMaker : BuildableItem
{
    public static Func<bool> MakeCheeseRequest;
    public static event Action CheeseMade = delegate { };
    public ObjectInstantiator objectInstantiator;
    public CooldownBar cooldownBar;

    private CheeseBlock[] cheeseTypes = new CheeseBlock[2];
    public new void Awake()
    {
        base.Awake();

        contextActions.Add("Change Cheese Type", ChangeCheeseType);
       
        cheeseTypes[0] = (CheeseBlock)Resources.Load("Prefabs/Cheese Block", typeof(CheeseBlock));
        cheeseTypes[1] = (CheeseBlock)Resources.Load("Prefabs/Purple Cheese Block", typeof(CheeseBlock));
        objectInstantiator.objectToBuild = cheeseTypes[0].gameObject;    
    }

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

    public void OnMouseUpAsButton()
    {
        if(!BuildHelper.IsPointerOverUI())
        {
            bool canMakeCheese = MakeCheeseRequest();
            if (canMakeCheese)
            {
                GameObject newCheese = MakeCheese();
                if (newCheese != null)
                {
                    newCheese.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
    }

    private void ChangeCheeseType()
    {
        int index = Array.IndexOf(cheeseTypes, objectInstantiator.objectToBuild.GetComponent<CheeseBlock>());
        index++;
        if(index > cheeseTypes.Length - 1)
        {
            index = 0;
        }
        objectInstantiator.objectToBuild = cheeseTypes[index].gameObject;
    }
}
