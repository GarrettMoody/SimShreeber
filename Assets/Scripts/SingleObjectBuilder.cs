using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObjectBuilder : MonoBehaviour
{
    public BuildManager buildManager;

    private bool isBuilding;
    private GameObject objectInHand;


    // Update is called once per frame
    void Update()
    {
        if(isBuilding)
        {
            if(buildManager.GetSnapToGridToggle())
            {
                objectInHand.transform.position = buildManager.GetClosestGridPoint(buildManager.GetMousePoint());
            } else
            {
                objectInHand.transform.position = buildManager.GetMousePoint();

            }

            if(Input.GetMouseButtonDown(0))
            {
                objectInHand.GetComponent<MeshCollider>().enabled = true;
                objectInHand = null;
                isBuilding = false;
            }
            if(Input.GetMouseButtonDown(1))
            {
                Destroy(objectInHand);
                isBuilding = false;
                buildManager.StopBuilding();
            }
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                objectInHand.transform.Rotate(Vector3.up, 45f);
            }
            
        }
    }

    public void StartBuilding(GameObject objectToBuild)
    {
        isBuilding = true;
        objectInHand = objectToBuild;
    }
}
