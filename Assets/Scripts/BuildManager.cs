using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
	//Public
    public GameObject wall;
	public ToolbarMenus toolbarMenus;
	public float gridSize = .1f;

    //Private
    private GameObject objectInHand;    
    private bool isBuilding;
    private bool twoStepBuild;
    private bool firstStep;
    private Vector3 startingBuildPosition;
    private Transform startingBuildTransform;
    private Vector3 endingBuildPosition;

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
                    Vector3 gridPoint = GetClosetGridPoint(hit.point);
                    objectInHand.transform.position = new Vector3(gridPoint.x, gridPoint.y, gridPoint.z);

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
                    Vector3 gridPoint = GetClosetGridPoint(hit.point);
                    float distance = Vector3.Distance(startingBuildPosition, gridPoint);
                    if (distance == 0f)
                    {
                        distance = .1f;
                    } else
                    {
                        distance += .1f;
                    }
                    Debug.Log("Start:" + startingBuildPosition + '\t' + "End:" + gridPoint + '\t' + "Distance:" + distance);
                    Vector3 averagePoint = (startingBuildPosition + gridPoint) / 2;
                    objectInHand.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);
					objectInHand.transform.position = new Vector3(averagePoint.x, startingBuildPosition.y, averagePoint.z);
					objectInHand.transform.LookAt(new Vector3(gridPoint.x, objectInHand.transform.position.y, gridPoint.z));
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
		toolbarMenus.CloseAllMenus();
        isBuilding = true;
        firstStep = true;
        twoStepBuild = true;
    }

    public Vector3 GetClosetGridPoint(Vector3 point)
    {
        float xPoint = Mathf.RoundToInt(point.x / gridSize);
        float yPoint = Mathf.RoundToInt(point.y / gridSize);
        float zPoint = Mathf.RoundToInt(point.z / gridSize);

        return new Vector3(xPoint * gridSize, yPoint * gridSize, zPoint * gridSize);

    }
}
