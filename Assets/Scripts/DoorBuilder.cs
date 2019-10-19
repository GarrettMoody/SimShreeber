using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuilder : MonoBehaviour
{
    public Door doorPrefab;
    public BuildManager buildManager;
    public WallBuilder wallBuilder;

    private bool isBuilding;
    private Door door;
    private Wall wall;
    private Wall leftWall;
    private Wall rightWall;

    public void StartBuilding()
    {
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
                        wall.GetComponent<Renderer>().material = buildManager.wallNormal;
                    }
                    wall = buildManager.GetMousePointGameObject().GetComponent<Wall>();

                    wall.GetComponent<Renderer>().material = buildManager.wallInvisible;
                    door.transform.position = new Vector3(mousePoint.x, .5f, mousePoint.z);
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
                } else
                {
                    door.transform.position = new Vector3(mousePoint.x, .5f, mousePoint.z);
                    UpdateWall(leftWall);
                    UpdateWall(rightWall);
                }
            }
            else
            {
                if(wall != null)
                {
                    wall.GetComponent<Renderer>().material = buildManager.wallNormal;
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
                door.transform.position = buildManager.GetMousePoint();
            }

            if(Input.GetMouseButtonDown(0))
            {
                Destroy(buildManager.GetMousePointGameObject());
                leftWall = null;
                rightWall = null;
                door.GetComponent<MeshCollider>().enabled = true;
                door = null;
                isBuilding = false;
            }
        }
    }

    public void UpdateWall(Wall updateWall)
    {
        updateWall.wallStart.transform.LookAt(updateWall.wallEnd.transform);

        float distance = Vector3.Distance(updateWall.wallStart.transform.position, updateWall.wallEnd.transform.position);
        updateWall.transform.position = updateWall.wallStart.transform.position + distance / 2 * updateWall.wallStart.transform.forward;
        updateWall.transform.LookAt(updateWall.wallStart.transform);
        updateWall.transform.localScale = new Vector3(updateWall.transform.localScale.x, updateWall.transform.localScale.y, distance - .1f);
    }
}
