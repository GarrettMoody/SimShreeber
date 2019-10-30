using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjectForward : MonoBehaviour
{
    public float speed;

    public void OnTriggerStay(Collider other)
    {
        Vector3 velocity = transform.forward * speed;
        other.GetComponent<Rigidbody>().velocity = velocity;
        //other.rigidbody.velocity = velocity;
        //Debug.Log( other.GetComponent<Rigidbody>().velocity.magnitude);
    }

    
}
