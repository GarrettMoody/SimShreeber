using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CooldownBar : MonoBehaviour
{
    public float cooldownTime;

    private float currentCooldownValue;

    public bool isCooling {get; private set;}

    // Update is called once per frame
    void Update()
    {
        if (isCooling)
        {
            currentCooldownValue += Time.deltaTime;
            this.GetComponent<Slider>().value = currentCooldownValue;
            if (currentCooldownValue >= cooldownTime)
            {
                isCooling = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    public void StartCooling()
    {
        isCooling = true;
        currentCooldownValue = 0f;
        this.gameObject.SetActive(true);
    }
}
