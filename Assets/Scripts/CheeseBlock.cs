using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseBlock : MonoBehaviour
{
    public float fadeoutTime;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            StartCoroutine("DestroyCheese");
        }
    }

    private IEnumerator DestroyCheese()
    {
        for(float f = fadeoutTime; f >=0; f -= .05f)
        {
            Color color = this.GetComponent<Renderer>().material.color;
            color.a = f/fadeoutTime;
            this.GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
    }
}
