using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{

    public Wall wallPrefab;
    public WallStud wallStudPrefab;
    public BuildManager buildManager;

    private bool isBuilding;
    private WallStud wallStart;
    private WallStud wallEnd;
    private Wall wall;
    private bool startSet;

    // Update is called once per frame
    void Update()
    {

        Debug.Log(buildManager.GetMousePoint().normalized);
        if (isBuilding)
        {
            //Escape Key Pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelBuild();
            }

            //Build hasn't started yet
            if (!startSet)
            {
                wallStart.transform.position = buildManager.GetMousePoint();
            }
            //Build has started
            else
            {
                //If pointing to another wall
                if(buildManager.GetMousePointGameObject().tag == "Wall")
                {
                    Vector3 mousePoint = buildManager.GetMousePoint();
                    wallEnd.transform.position = new Vector3(mousePoint.x, buildManager.GetMousePointGameObject().transform.position.y, mousePoint.z);
                    wallEnd.transform.position = wallEnd.transform.position + .05f * -wallEnd.transform.forward;

                }
                //Pointing to anything else
                else
                {
                    wallEnd.transform.position = buildManager.GetMousePoint();
                    wallEnd.transform.LookAt(wallStart.transform);
                }

                //Update Wall
                wallStart.transform.LookAt(wallEnd.transform);
                
                float distance = Vector3.Distance(wallStart.transform.position, wallEnd.transform.position);
                wall.transform.position = wallStart.transform.position + distance / 2 * wallStart.transform.forward;
                wall.transform.LookAt(wallStart.transform);
                wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance - .1f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!startSet)
                {
                    startSet = true;
                    wallStart.GetComponent<BoxCollider>().enabled = true;
                    wallEnd = Instantiate(wallStudPrefab);
                    wall = Instantiate(wallPrefab);
                    wall.transform.position = buildManager.GetMousePoint();
                    wallEnd.transform.position = buildManager.GetMousePoint();
                }
                else
                {
                    wallStart.GetComponent<BoxCollider>().enabled = true;
                    wall.GetComponent<BoxCollider>().enabled = true;
                    wallStart = wallEnd;
                    wallEnd = Instantiate(wallStudPrefab);
                    wall.wallStart = wallStart;
                    wall.wallEnd = wallEnd;
                    wall = Instantiate(wallPrefab);
                    wall.transform.position = buildManager.GetMousePoint();
                    wallEnd.transform.position = buildManager.GetMousePoint();
                }
            }
        }
    }

    public void StartBuilding()
    {
        isBuilding = true;
        startSet = false;
        wallStart = Instantiate(wallStudPrefab);
    }

    /// <summary>
    /// Cancels the current build.
    /// </summary>
    public void CancelBuild()
    {
        isBuilding = false;
        if (!startSet && wallStart != null)
        {
            Destroy(wallStart.gameObject);
        }
        if (wallEnd != null)
        {
            Destroy(wallEnd.gameObject);
            Destroy(wall.gameObject);
        }
    }
}
