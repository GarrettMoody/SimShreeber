using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuilder : MonoBehaviour
{
    public BuildManager buildManager;
    public Door doorPrefab;
    public WallBuilder wallBuilder;

    private bool isBuilding;
    private Door door;
    private Wall wall;
    private Wall leftWall;
    private Wall rightWall;

    private float doorHeight;

    private void Start()
    {
        doorHeight = doorPrefab.GetComponent<BoxCollider>().size.y;
    }

    public void StartBuilding()
    {
        buildManager.StartBuilding();
        door = Instantiate(doorPrefab);
        door.transform.position = buildManager.GetMousePoint();
        isBuilding = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBuilding)
        {
            Vector3 mousePoint = buildManager.GetMousePoint();

            if (buildManager.IsWall(buildManager.GetMousePointGameObject()))
            {
                //If pointing to new wall
                if (wall != buildManager.GetMousePointGameObject().GetComponent<Wall>())
                {
                    if (wall != null)
                    {
                        wall.UseNormalMaterial();
                    }
                    wall = buildManager.GetMousePointGameObject().GetComponent<Wall>();

                    wall.UseInvisibleMaterial();
                    door.transform.position = new Vector3(mousePoint.x, doorHeight/2, mousePoint.z);
                    door.transform.LookAt(wall.wallStart.transform);
                    if(leftWall != null)
                    {
                        Destroy(leftWall.gameObject);
                    }
                    if(rightWall != null)
                    {
                        Destroy(rightWall.gameObject);
                    }

                    leftWall = wallBuilder.CreateNewWall(wall.wallStart, door.wallStud2);
                    rightWall = wallBuilder.CreateNewWall(wall.wallEnd, door.wallStud1);
                    door.wallStud1.GetComponent<BoxCollider>().enabled = false;
                    door.wallStud2.GetComponent<BoxCollider>().enabled = false;
                } else
                {
                    door.transform.position = new Vector3(mousePoint.x, doorHeight/2, mousePoint.z);
                    wallBuilder.UpdateWall(leftWall);
                    wallBuilder.UpdateWall(rightWall);
                }
            }
            else
            {
                if(wall != null)
                {
                    wall.UseNormalMaterial();
                    wall = null;
                }
                if(leftWall != null)
                {
                    Destroy(leftWall.gameObject);
                }
                if(rightWall != null)
                {
                    Destroy(rightWall.gameObject);
                }
                door.transform.position = new Vector3(mousePoint.x, doorHeight/2, mousePoint.z);
            }

            if(Input.GetMouseButtonDown(0))
            {
                if (buildManager.IsWall(buildManager.GetMousePointGameObject()))
                {
                    Destroy(buildManager.GetMousePointGameObject());
                    leftWall = null;
                    rightWall = null;
                    door.GetComponent<BoxCollider>().enabled = true;
                    door.wallStud1.GetComponent<BoxCollider>().enabled = true;
                    door.wallStud2.GetComponent<BoxCollider>().enabled = true;
                    door = null;
                    isBuilding = false;
                }
            }
        }
    }

    

    public void StopBuilding()
    {
        if(door != null)
        {
            Destroy(door.gameObject);
        }
        if(leftWall != null)
        {
            Destroy(leftWall.gameObject);
        }
        if(rightWall != null)
        {
            Destroy(rightWall.gameObject);
        }
        if(wall != null)
        {
            wall.UseNormalMaterial();
        }
        isBuilding = false;
    }

    public bool IsBuilding()
    {
        return isBuilding;
    }
}
