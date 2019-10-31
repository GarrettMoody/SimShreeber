using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float speed;
    public Transform objectTransform;
    public Vector3 direction;

    private Dictionary<GameObject, bool> objectsBeingPushed = new Dictionary<GameObject, bool>();

    public void OnTriggerEnter(Collider other)
    {
        objectsBeingPushed.Add(other.gameObject, true);
    }

    public void OnTriggerStay(Collider other)
    {
        Vector3 velocity;
        if(Vector3.Distance(objectTransform.position, other.transform.position) < 0.2f)
        {
            objectsBeingPushed[other.gameObject] = false;
        }
            
        if (objectTransform != null && objectsBeingPushed[other.gameObject])
        {
            velocity = Vector3.Normalize(objectTransform.position - other.transform.position) * speed;
        }
        else
        {
            velocity = transform.TransformDirection(direction) * speed;
        }
        other.GetComponent<Rigidbody>().velocity = velocity;
    }

    public void OnTriggerExit(Collider other)
    {
        objectsBeingPushed.Remove(other.gameObject);
    }


}
