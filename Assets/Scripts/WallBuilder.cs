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
                if (mouseObject.tag == "Wall")
                {
                    wallStart.gameObject.SetActive(true);
                    wallStart.transform.position = new Vector3(mousePoint.x, buildManager.GetMousePointGameObject().transform.position.y, mousePoint.z);
                }
                else if (mouseObject.tag == "WallStud")
                {
                    wallStart.gameObject.SetActive(false);
                }
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
                //If pointing to another wall
                if(mouseObject.tag == "Wall")
                {
                    wallEnd.gameObject.SetActive(true);
                    Vector3 mousePoint = buildManager.GetMousePoint();
                    wallEnd.transform.position = new Vector3(mousePoint.x, buildManager.GetMousePointGameObject().transform.position.y, mousePoint.z);
                }
                else if (mouseObject.tag == "WallStud")
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
                    GameObject mouseObject = buildManager.GetMousePointGameObject();

                    if(mouseObject.tag == "Wall")
                    {
                        //Pointing to wall. Split wall and place new stud
                        Wall mouseWall = buildManager.GetMousePointGameObject().GetComponent<Wall>();
                        WallStud previousWallStart = mouseWall.wallStart;
                        WallStud previousWallEnd = mouseWall.wallEnd;

                        //Create new walls
                        Wall newWall1 = Instantiate(wallPrefab);
                        newWall1.wallStart = previousWallStart;
                        newWall1.wallEnd = wallStart;
                        newWall1.wallStart.transform.LookAt(newWall1.wallEnd.transform);
                        newWall1.wallEnd.transform.LookAt(newWall1.wallStart.transform);
                        float distance = Vector3.Distance(newWall1.wallStart.transform.position, newWall1.wallEnd.transform.position);
                        newWall1.transform.position = newWall1.wallStart.transform.position + distance / 2 * newWall1.wallStart.transform.forward;
                        newWall1.transform.LookAt(newWall1.wallStart.transform);
                        newWall1.transform.localScale = new Vector3(newWall1.transform.localScale.x, newWall1.transform.localScale.y, distance - .1f);

                        //Create second wall
                        Wall newWall2 = Instantiate(wallPrefab);
                        newWall2.wallStart = wallStart;
                        newWall2.wallEnd = previousWallEnd;
                        newWall2.wallStart.transform.LookAt(newWall2.wallEnd.transform);
                        newWall2.wallEnd.transform.LookAt(newWall2.wallStart.transform);
                        distance = Vector3.Distance(newWall2.wallStart.transform.position, newWall2.wallEnd.transform.position);
                        newWall2.transform.position = newWall2.wallStart.transform.position + distance / 2 * newWall2.wallStart.transform.forward;
                        newWall2.transform.LookAt(newWall2.wallStart.transform);
                        newWall2.transform.localScale = new Vector3(newWall2.transform.localScale.x, newWall2.transform.localScale.y, distance - .1f);

                        //Place wall and continue building
                        newWall1.GetComponent<BoxCollider>().enabled = true;
                        newWall2.GetComponent<BoxCollider>().enabled = true;
                        newWall1.wallStart.GetComponent<BoxCollider>().enabled = true;
                        newWall1.wallEnd.GetComponent<BoxCollider>().enabled = true;
                        newWall2.wallEnd.GetComponent<BoxCollider>().enabled = true;
                        wallStart.GetComponent<BoxCollider>().enabled = true;
                        wall = Instantiate(wallPrefab);
                        wallEnd = Instantiate(wallStudPrefab);
                        wallEnd.transform.position = buildManager.GetMousePoint();
                        Destroy(mouseWall.gameObject);
                    }
                    else if (mouseObject.tag == "WallStud")
                    {
                        Destroy(wallStart.gameObject);
                        wallStart = mouseObject.GetComponent<WallStud>();
                        wallEnd = Instantiate(wallStudPrefab);
                        wall = Instantiate(wallPrefab);
                        wall.wallStart = wallStart;
                        wall.transform.position = buildManager.GetMousePoint();
                        wallEnd.transform.position = buildManager.GetMousePoint();

                    } else
                    {
                        //Set the starting position
                        wallStart.GetComponent<BoxCollider>().enabled = true;
                        wallEnd = Instantiate(wallStudPrefab);
                        wall = Instantiate(wallPrefab);
                        wall.wallStart = wallStart;
                        wall.transform.position = buildManager.GetMousePoint();
                        wallEnd.transform.position = buildManager.GetMousePoint();
                    }

                    startSet = true;
                }
                else
                {
                    if(buildManager.GetMousePointGameObject().tag == "Wall")
                    {
                        //Pointing to wall. Split wall and place new stud
                        Wall mouseWall = buildManager.GetMousePointGameObject().GetComponent<Wall>();
                        WallStud previousWallStart = mouseWall.wallStart;
                        WallStud previousWallEnd = mouseWall.wallEnd;

                        //Create new walls
                        Wall newWall1 = Instantiate(wallPrefab);
                        newWall1.wallStart = previousWallStart;
                        newWall1.wallEnd = wallEnd;
                        newWall1.wallStart.transform.LookAt(newWall1.wallEnd.transform);
                        newWall1.wallEnd.transform.LookAt(newWall1.wallStart.transform);
                        float distance = Vector3.Distance(newWall1.wallStart.transform.position, newWall1.wallEnd.transform.position);
                        newWall1.transform.position = newWall1.wallStart.transform.position + distance / 2 * newWall1.wallStart.transform.forward;
                        newWall1.transform.LookAt(newWall1.wallStart.transform);
                        newWall1.transform.localScale = new Vector3(newWall1.transform.localScale.x, newWall1.transform.localScale.y, distance - .1f);

                        //Create second wall
                        Wall newWall2 = Instantiate(wallPrefab);
                        newWall2.wallStart = wallEnd;
                        newWall2.wallEnd = previousWallEnd;
                        newWall2.wallStart.transform.LookAt(newWall2.wallEnd.transform);
                        newWall2.wallEnd.transform.LookAt(newWall2.wallStart.transform);
                        distance = Vector3.Distance(newWall2.wallStart.transform.position, newWall2.wallEnd.transform.position);
                        newWall2.transform.position = newWall2.wallStart.transform.position + distance / 2 * newWall2.wallStart.transform.forward;
                        newWall2.transform.LookAt(newWall2.wallStart.transform);
                        newWall2.transform.localScale = new Vector3(newWall2.transform.localScale.x, newWall2.transform.localScale.y, distance - .1f);

                        //Place wall and continue building
                        newWall1.GetComponent<BoxCollider>().enabled = true;
                        newWall2.GetComponent<BoxCollider>().enabled = true;
                        newWall1.wallStart.GetComponent<BoxCollider>().enabled = true;
                        newWall1.wallEnd.GetComponent<BoxCollider>().enabled = true;
                        newWall2.wallEnd.GetComponent<BoxCollider>().enabled = true;
                        wall.GetComponent<BoxCollider>().enabled = true;
                        wall.wallStart = wallStart;
                        wall.wallEnd = wallEnd;
                        wallStart = wallEnd;
                        wallEnd = Instantiate(wallStudPrefab);
                        wall = Instantiate(wallPrefab);
                        wall.transform.position = buildManager.GetMousePoint();
                        wallEnd.transform.position = buildManager.GetMousePoint();
                        Destroy(mouseWall.gameObject);
                    }
                    else if(buildManager.GetMousePointGameObject().tag == "WallStud")
                    {
                        Destroy(wallEnd.gameObject);
                        wallEnd = buildManager.GetMousePointGameObject().GetComponent<WallStud>();

                        //Place stud and create another stud with wall
                        wallStart.GetComponent<BoxCollider>().enabled = true;
                        wall.GetComponent<BoxCollider>().enabled = true;
                        wall.wallStart = wallStart;
                        wall.wallEnd = wallEnd;
                        wallStart = wallEnd;
                        wallEnd = Instantiate(wallStudPrefab);
                        wall = Instantiate(wallPrefab);
                        wall.transform.position = buildManager.GetMousePoint();
                        wallEnd.transform.position = buildManager.GetMousePoint();
                    }
                    else
                    {
                        //Place stud and create another stud with wall
                        wallStart.GetComponent<BoxCollider>().enabled = true;
                        wallEnd.GetComponent<BoxCollider>().enabled = true;
                        wall.GetComponent<BoxCollider>().enabled = true;
                        wall.wallStart = wallStart;
                        wall.wallEnd = wallEnd;
                        wallStart = wallEnd;
                        wallEnd = Instantiate(wallStudPrefab);
                        wall = Instantiate(wallPrefab);
                        wall.transform.position = buildManager.GetMousePoint();
                        wallEnd.transform.position = buildManager.GetMousePoint();
                    }
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

    public bool IsWallOrStud(GameObject gameObject)
    {
        if(gameObject.tag == "Wall" || gameObject.tag == "WallStud")
        {
            return true;
        } else
        {
            return false;
        }
    }
}
