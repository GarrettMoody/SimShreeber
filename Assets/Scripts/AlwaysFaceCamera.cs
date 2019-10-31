using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{ 
    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(2 * this.transform.position - Camera.main.transform.position);
    }
}
