using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float speed;
    public Vector3 direction;

    public void OnTriggerStay(Collider other)
    {
        Vector3 velocity = transform.TransformDirection(direction) * speed;
        other.GetComponent<Rigidbody>().velocity = velocity;
        //other.rigidbody.velocity = velocity;
        //Debug.Log( other.GetComponent<Rigidbody>().velocity.magnitude);
    }

    
}
