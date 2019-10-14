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
        if (isBuilding)
        {
            //Escape Key Pressed
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                CancelBuild();
            }

            //Build hasn't started yet
            if (!startSet)
            {
                Vector3 mousePoint = buildManager.GetMousePoint();
                GameObject mouseObject = buildManager.GetMousePointGameObject();

                //If pointing to a wall
                if (IsWall(mouseObject))
                {
                    wallStart.gameObject.SetActive(true);
                    wallStart.transform.position = new Vector3(mousePoint.x, buildManager.GetMousePointGameObject().transform.position.y, mousePoint.z);
                }
                //If pointing to a wall stud
                else if (IsWallStud(mouseObject))
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
            //Build has started
            else
            {
                GameObject mouseObject = buildManager.GetMousePointGameObject();
                Vector3 mousePoint = buildManager.GetMousePoint();

                //If pointing to another wall
                if (IsWall(mouseObject))
                {
                    wallEnd.gameObject.SetActive(true);
                    wallEnd.transform.position = new Vector3(mousePoint.x, buildManager.GetMousePointGameObject().transform.position.y, mousePoint.z);
                }
                else if (IsWallStud(mouseObject))
                {
                    wallEnd.gameObject.SetActive(false);
                    wallEnd.transform.position = mouseObject.transform.position;
                }
                else
                {
                    wallEnd.gameObject.SetActive(true);
                    wallEnd.transform.position = buildManager.GetMousePoint();
                    wallEnd.transform.LookAt(wallStart.transform);
                }
                UpdateWall();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!startSet)
                {
                    GameObject mouseObject = buildManager.GetMousePointGameObject();

                    //Started on Wall
                    if(IsWall(mouseObject))
                    {
                        //Pointing to wall. Split wall and place new stud
                        Wall mouseWall = buildManager.GetMousePointGameObject().GetComponent<Wall>();
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
                    else if (IsWallStud(mouseObject))
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
                    GameObject mouseObject = buildManager.GetMousePointGameObject();

                    //Ended on Wall
                    if(IsWall(mouseObject))
                    {
                        //Pointing to wall. Split wall and place new stud
                        Wall mouseWall = buildManager.GetMousePointGameObject().GetComponent<Wall>();
                        WallStud previousWallStart = mouseWall.wallStart;
                        WallStud previousWallEnd = mouseWall.wallEnd;

                        //Create new walls
                        CreateNewWall(previousWallStart, wallEnd);
                        CreateNewWall(wallEnd, previousWallEnd);
                    
                        Destroy(mouseWall.gameObject);
                    }
                    //Ended on Stud
                    else if(IsWallStud(mouseObject))
                    {
                        Destroy(wallEnd.gameObject);
                        wallEnd = buildManager.GetMousePointGameObject().GetComponent<WallStud>();

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
        if(wallStart != null)
        {
            wallStart.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        if(wallEnd != null)
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

    public bool IsWall(GameObject gameObject)
    {
        if (gameObject.tag == "Wall")
        {
            return true;
        }
        return false;
    }

    public bool IsWallStud(GameObject gameObject)
    {
        if(gameObject.tag == "WallStud")
        {
            return true;
        }
        return false;
    }

    public void CreateNewWall(WallStud wallStart, WallStud wallEnd)
    {
        Wall newWall = Instantiate(wallPrefab);

        //Set start and end
        newWall.wallStart = wallStart;
        newWall.wallEnd = wallEnd;
        newWall.wallStart.transform.LookAt(newWall.wallEnd.transform);
        newWall.wallEnd.transform.LookAt(newWall.wallStart.transform);

        //Create Wall and set proper size
        float distance = Vector3.Distance(newWall.wallStart.transform.position, newWall.wallEnd.transform.position);
        newWall.transform.position = newWall.wallStart.transform.position + distance / 2 * newWall.wallStart.transform.forward;
        newWall.transform.LookAt(newWall.wallStart.transform);
        newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, newWall.transform.localScale.y, distance - .1f);

        newWall.GetComponent<BoxCollider>().enabled = true;
        newWall.wallStart.GetComponent<BoxCollider>().enabled = true;
        newWall.wallEnd.GetComponent<BoxCollider>().enabled = true;
    }

    public void ContinueBuild()
    {
        //Place wall and continue building
        wallStart.GetComponent<BoxCollider>().enabled = true;
        wallEnd.GetComponent<BoxCollider>().enabled = true;
        wall.GetComponent<BoxCollider>().enabled = true;
        wall.wallStart = wallStart;
        wall.wallEnd = wallEnd;
        wallStart = wallEnd;
        wall = Instantiate(wallPrefab);
        wall.transform.position = buildManager.GetMousePoint();
        wallEnd = Instantiate(wallStudPrefab);
        wallEnd.transform.position = buildManager.GetMousePoint();
    }

    public void UpdateWall()
    {
        wallStart.transform.LookAt(wallEnd.transform);

        float distance = Vector3.Distance(wallStart.transform.position, wallEnd.transform.position);
        wall.transform.position = wallStart.transform.position + distance / 2 * wallStart.transform.forward;
        wall.transform.LookAt(wallStart.transform);
        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance - .1f);
    }

    public void SetStart(WallStud wallStartValue)
    { 
        wallStart = wallStartValue;
        wallStart.GetComponent<BoxCollider>().enabled = true;
        wallEnd = Instantiate(wallStudPrefab);
        wall = Instantiate(wallPrefab);
        wall.wallStart = wallStart;
        wall.transform.position = buildManager.GetMousePoint();
        wallEnd.transform.position = buildManager.GetMousePoint();
    }
}
