using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObjectBuilder : MonoBehaviour
{
    //public BuildManager buildManager;
    public Vector3 buildOffset;


    private bool isBuilding;
    //private GameObject objectInHand;


    // Update is called once per frame
    void Update()
    {
        if(isBuilding)
        {
            
           this.transform.position = BuildHelper.GetMousePoint() + buildOffset;

            if(Input.GetMouseButtonDown(0))
            {
                if(this.GetComponent<Collider>() != null)
                {
                    this.GetComponent<Collider>().enabled = true;
                }
                if(this.GetComponent<Rigidbody>() != null)
                {
                    this.GetComponent<Rigidbody>().isKinematic = false;
                }
                isBuilding = false;
            }
            if(Input.GetMouseButtonDown(1))
            {
                Destroy(this.gameObject);
                isBuilding = false;
                //buildManager.StopBuilding();
            }
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                this.transform.Rotate(Vector3.up, 45f);
            }
            
        }
    }

    public void StartBuilding()
    {
        isBuilding = true;
    }
}
