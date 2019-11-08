using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float speed;
    public Transform objectTransform;
    public Vector3 direction;

    private Dictionary<GameObject, bool> objectsBeingPushed = new Dictionary<GameObject, bool>();


    /// <summary>
    /// When the object hits the trigger, it will also be added to the dictionary
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        objectsBeingPushed.Add(other.gameObject, true);
    }

    /// <summary>
    /// If there is an object transform for the object being pushed to move towards, it will go there. Once
    /// it gets close enough to the "endpoint" it will just continue forward. Since there could be multiple
    /// objects in the trigger at the same time, we need to handle each one separately using the dicitonary.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerStay(Collider other)
    {
        Vector3 velocity;
        if(objectTransform != null)
        {
            if (Vector3.Distance(objectTransform.position, other.transform.position) < 0.2f)
            {
                objectsBeingPushed[other.gameObject] = false;
            }
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

    /// <summary>
    /// When the object leaves, clean up the dictionary.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        objectsBeingPushed.Remove(other.gameObject);
    }


}
