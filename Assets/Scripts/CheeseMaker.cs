using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheeseMaker : MonoBehaviour
{
    public ObjectInstantiator objectInstantiator;
    public float cooldownTime;
    public Slider cooldownSlider;

    private float currentCooldownValue;
    private bool isCooling; 

    public void Update()
    {
        if(isCooling)
        {
            currentCooldownValue += Time.deltaTime;
            cooldownSlider.value = currentCooldownValue;
            if(currentCooldownValue >= cooldownTime)
            {
                isCooling = false;
                cooldownSlider.gameObject.SetActive(false);
            }
        }
    }

    public GameObject MakeCheese()
    {
        if(!isCooling)
        {
            StartCooling();
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

    private void StartCooling()
    {
        isCooling = true;
        currentCooldownValue = 0f;
        cooldownSlider.gameObject.SetActive(true);
    }
}
