using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //Public
    public GameObject wall;

    //Private
    private GameObject objectInHand;
    private bool isBuilding;
    private bool twoStepBuild;
    private bool firstStep;
    private Vector3 startingBuildPosition;
    private Transform startingBuildTransform;
    private Vector3 endingBuildPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(isBuilding)
        {
            if (firstStep)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    objectInHand.transform.position = new Vector3(hit.point.x, hit.point.y * 2, hit.point.z);

                    if (Input.GetMouseButtonDown(0))
                    {
                        startingBuildPosition = objectInHand.transform.position;
                        startingBuildTransform = objectInHand.transform;
                        firstStep = false;
                        if(!twoStepBuild)
                        {
                            isBuilding = false;
                            objectInHand = null;
                        }
                    }
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    float distance = Vector3.Distance(startingBuildPosition, hit.point);
                    Vector3 averagePoint = (startingBuildPosition + hit.point) / 2;
                    objectInHand.transform.position = new Vector3(averagePoint.x, startingBuildPosition.y, averagePoint.z);
                    objectInHand.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);
                    objectInHand.transform.LookAt(new Vector3(hit.point.x, objectInHand.transform.position.y, hit.point.z));
                    //objectInHand.transform.position = startingBuildPosition + distance / 2 * startingBuildTransform.forward;
                    if (Input.GetMouseButtonDown(0))
                    {
                        endingBuildPosition = objectInHand.transform.position;
                        isBuilding = false;
                        objectInHand = null;
                    }
                }
            }
        }
    } 

    public void OnWallButtonClicked()
    {
        objectInHand = Instantiate(wall);
        isBuilding = true;
        firstStep = true;
        twoStepBuild = true;
    }
}
