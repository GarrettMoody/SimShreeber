using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallBuilder : MonoBehaviour
{
    public BuildManager buildManager;
    public WallStud wallStudPrefab;

    private bool isBuilding;
    private WallStud wallStart;
    private WallStud wallEnd;
    private Wall wall;
    private bool startSet;
    private float wallStudHeight;

    public void Start()
    {
        wallStudHeight = wallStudPrefab.GetComponent<BoxCollider>().size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            Vector3 mousePoint;

            if (buildManager.GetSnapToGridToggle())
            {
                mousePoint = BuildHelper.GetClosestGridPoint(BuildHelper.GetMousePoint());
            }
            else
            {
                mousePoint = BuildHelper.GetMousePoint();
            }


            mousePoint = new Vector3(mousePoint.x, mousePoint.y + wallStudHeight / 2, mousePoint.z);

            //Build hasn't started yet
            if (!startSet)
            {
                GameObject mouseObject = BuildHelper.GetMousePointGameObject();

                if (mouseObject != null)
                {
                    //If pointing to a wall
                    if (buildManager.IsWall(mouseObject))
                    {
                        wallStart.gameObject.SetActive(true);
                        wallStart.transform.position = new Vector3(BuildHelper.GetMousePoint().x, BuildHelper.GetMousePointGameObject().transform.position.y, BuildHelper.GetMousePoint().z);
                    }
                    //If pointing to a wall stud
                    else if (buildManager.IsWallStud(mouseObject))
                    {
                        wallStart.gameObject.SetActive(false);
                    }
                    //Pointing to anything else
                    else
                    {
                        wallStart.gameObject.SetActive(true);
                        wallStart.transform.position = mousePoint;
                    }
                }
            }
            //Build has started
            else
            {
                GameObject mouseObject = BuildHelper.GetMousePointGameObject();

                //If pointing to another wall
                if (buildManager.IsWall(mouseObject))
                {
                    Wall mouseWall = mouseObject.GetComponent<Wall>();
                    //Get Angle
                    Vector2 connectingWallVector = new Vector2(mouseWall.wallStart.transform.position.x, mouseWall.wallStart.transform.position.z) - new Vector2(mouseWall.wallEnd.transform.position.x, mouseWall.wallEnd.transform.position.z);
                    Vector2 newWallVector = new Vector2(wallStart.transform.position.x, wallStart.transform.position.z) - new Vector2(BuildHelper.GetMousePoint().x, BuildHelper.GetMousePoint().z);

                    if (Mathf.Abs(90f - Vector2.Angle(connectingWallVector, newWallVector)) < 5f)
                    {
                        mousePoint = mouseWall.GetComponent<BoxCollider>().ClosestPoint(wallStart.transform.position);
                        //mousePoint = new Vector3(mousePoint.x, buildManager.GetMousePointGameObject().transform.position.y, mousePoint.z);
                    }
                    else
                    {
                        mousePoint = new Vector3(BuildHelper.GetMousePoint().x, BuildHelper.GetMousePointGameObject().transform.position.y, BuildHelper.GetMousePoint().z);
                    }

                    //mousePoint = new Vector3(buildManager.GetMousePoint().x, buildManager.GetMousePointGameObject().transform.position.y, buildManager.GetMousePoint().z);
                    wallEnd.gameObject.SetActive(true);
                    wallEnd.transform.position = mousePoint;
                }
                else if (buildManager.IsWallStud(mouseObject))
                {
                    wallEnd.gameObject.SetActive(false);
                    wallEnd.transform.position = mouseObject.transform.position;
                }
                else
                {
                    wallEnd.gameObject.SetActive(true);
                    wallEnd.transform.position = mousePoint;
                    wallEnd.transform.LookAt(wallStart.transform);
                }
                UpdateWall();
            }

            if (Input.GetMouseButtonDown(0) && !BuildHelper.IsPointerOverUI())
            {
                if (!startSet)
                {
                    GameObject mouseObject = BuildHelper.GetMousePointGameObject();

                    //Started on Wall
                    if (buildManager.IsWall(mouseObject))
                    {
                        //Pointing to wall. Split wall and place new stud
                        Wall mouseWall = BuildHelper.GetMousePointGameObject().GetComponent<Wall>();
                        WallStud previousWallStart = mouseWall.wallStart;
                        WallStud previousWallEnd = mouseWall.wallEnd;

                        //Create new walls
                        CreateNewWall(previousWallStart, wallStart);
                        CreateNewWall(wallStart, previousWallEnd);

                        SetStart(wallStart);
                        //Destroy the old wall that used to be there
                        Destroy(mouseWall.gameObject);
                    }
                    //Started on Stud
                    else if (buildManager.IsWallStud(mouseObject))
                    {
                        Destroy(wallStart.gameObject);
                        SetStart(mouseObject.GetComponent<WallStud>());
                    }
                    //Started on anything else
                    else
                    {
                        SetStart(wallStart);
                    }

                    startSet = true;
                }
                //Build has started
                else
                {
                    GameObject mouseObject = BuildHelper.GetMousePointGameObject();

                    //Ended on Wall
                    if (buildManager.IsWall(mouseObject))
                    {
                        //Pointing to wall. Split wall and place new stud
                        Wall mouseWall = BuildHelper.GetMousePointGameObject().GetComponent<Wall>();
                        WallStud previousWallStart = mouseWall.wallStart;
                        WallStud previousWallEnd = mouseWall.wallEnd;

                        //Create new walls
                        CreateNewWall(previousWallStart, wallEnd);
                        CreateNewWall(wallEnd, previousWallEnd);

                        Destroy(mouseWall.gameObject);
                    }
                    //Ended on Stud
                    else if (buildManager.IsWallStud(mouseObject))
                    {
                        Destroy(wallEnd.gameObject);
                        wallEnd = BuildHelper.GetMousePointGameObject().GetComponent<WallStud>();

                    }
                    //Ended on anything else
                    else
                    {
                        wall.wallEnd = wallEnd;
                        wall.wallStart = wallStart;
                    }

                    ContinueBuild();
                }
            }
        }
    }

    public void StartBuilding()
    {
        buildManager.StartBuilding();
        isBuilding = true;
        startSet = false;
        wallStart = Instantiate(wallStudPrefab);
    }

    /// <summary>
    /// Cancels the current build.
    /// </summary>
    public void StopBuilding()
    {
        isBuilding = false;
        if (wallStart != null)
        {
            wallStart.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        if (wallEnd != null)
        {
            wallEnd.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
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

    public Wall CreateNewWall(WallStud wallStart, WallStud wallEnd)
    {
        Wall newWall = Instantiate(buildManager.wallPrefab);

        wallStart.AddWall(newWall);
        wallEnd.AddWall(newWall);
        //Set start and end
        newWall.wallStart = wallStart;
        newWall.wallEnd = wallEnd;
        newWall.wallStart.transform.LookAt(newWall.wallEnd.transform);
        newWall.wallEnd.transform.LookAt(newWall.wallStart.transform);

        //Create Wall and set proper size
        float distance = Vector3.Distance(newWall.wallStart.transform.position, newWall.wallEnd.transform.position);
        newWall.transform.position = newWall.wallStart.transform.position + distance / 2 * newWall.wallStart.transform.forward;
        newWall.transform.LookAt(newWall.wallStart.transform);
        newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, newWall.transform.localScale.y, distance / 3);

        newWall.GetComponent<BoxCollider>().enabled = true;
        newWall.wallStart.GetComponent<BoxCollider>().enabled = true;
        newWall.wallEnd.GetComponent<BoxCollider>().enabled = true;

        return newWall;
    }

    public void ContinueBuild()
    {
        //Place wall and continue building
        //Enable Colliders
        wallStart.GetComponent<BoxCollider>().enabled = true;
        wallEnd.GetComponent<BoxCollider>().enabled = true;
        wall.GetComponent<BoxCollider>().enabled = true;

        //Assign Studs to walls and walls to studs
        wallStart.AddWall(wall);
        wallEnd.AddWall(wall);
        wall.wallStart = wallStart;
        wall.wallEnd = wallEnd;

        wallStart = wallEnd;
        wall = Instantiate(buildManager.wallPrefab);
        wall.transform.position = BuildHelper.GetMousePoint();
        wallEnd = Instantiate(wallStudPrefab);
        wallEnd.transform.position = BuildHelper.GetMousePoint();
    }

    public void UpdateWall()
    {
        wallStart.transform.LookAt(wallEnd.transform);

        float distance = Vector3.Distance(wallStart.transform.position, wallEnd.transform.position);
        wall.transform.position = wallStart.transform.position + distance / 2 * wallStart.transform.forward;
        wall.transform.LookAt(wallStart.transform);
        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance / 3);
    }

    public void UpdateWall(Wall updateWall)
    {
        updateWall.wallStart.transform.LookAt(updateWall.wallEnd.transform);

        float distance = Vector3.Distance(updateWall.wallStart.transform.position, updateWall.wallEnd.transform.position);
        updateWall.transform.position = updateWall.wallStart.transform.position + distance / 2 * updateWall.wallStart.transform.forward;
        updateWall.transform.LookAt(updateWall.wallStart.transform);
        updateWall.transform.localScale = new Vector3(updateWall.transform.localScale.x, updateWall.transform.localScale.y, distance / 3);
    }

    public void SetStart(WallStud wallStartValue)
    {
        wallStart = wallStartValue;
        wallStart.GetComponent<BoxCollider>().enabled = true;
        wallEnd = Instantiate(wallStudPrefab);
        wall = Instantiate(buildManager.wallPrefab);
        wall.wallStart = wallStart;
        wall.transform.position = BuildHelper.GetMousePoint();
        wallEnd.transform.position = BuildHelper.GetMousePoint();
    }

    public bool IsBuilding()
    {
        return isBuilding;
    }
}
