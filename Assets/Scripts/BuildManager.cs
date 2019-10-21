using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
    #region Public Variables

    public Wall wallPrefab;

    #endregion

    #region Private Variables

    protected ToolbarMenus toolbarMenus;
    public WallBuilder wallBuilder;
    public DoorBuilder doorBuilder;
    public float gridSize;
    public TextMeshProUGUI snapText;
     
    public GameObject floor;
    public Material floorNormalMaterial;
    public Material floorBuildingMaterial;

    private bool isBuilding;
    private bool isDeleting;
    private bool snapToGridToggle;

    #endregion

    private void Start()
    {
        UpdateSnapToGridText();
        StopBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        //Escape Key Pressed
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            StopBuilding();
        }

        if (isDeleting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GetMousePointGameObject().name != "Floor")
                {
                    Destroy(GetMousePointGameObject());
                }
            }
        }

        if (isBuilding)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                snapToGridToggle = !snapToGridToggle;
                UpdateSnapToGridText();
            }
        }
    }

    public void StartBuilding()
    {
        //GameObject [] wallStuds = GameObject.FindGameObjectsWithTag("WallStud");
        //foreach (GameObject wallStud in wallStuds)
        //{
        //    wallStud.GetComponent<WallStud>().UseBuildingMaterial();
        //}

        floor.GetComponent<Renderer>().material = floorBuildingMaterial;

        isBuilding = true;
        UpdateSnapToGridText();
        snapText.enabled = true;
    }

    public void StopBuilding()
    {
        //Change colors of studs
        //GameObject[] wallStuds = GameObject.FindGameObjectsWithTag("WallStud");
        //foreach (GameObject wallStud in wallStuds)
        //{
        //    wallStud.GetComponent<WallStud>().UseNormalMaterial();
        //}

        floor.GetComponent<Renderer>().material = floorNormalMaterial;

        if(wallBuilder.IsBuilding())
        {
            wallBuilder.StopBuilding();
        }
        if (doorBuilder.IsBuilding())
        {
            doorBuilder.StopBuilding();
        }

        isBuilding = false;
        isDeleting = false;
        snapText.enabled = false;
    }

    public void UpdateSnapToGridText()
    {
        if(isBuilding)
        {
            if (snapToGridToggle)
            {
                snapText.text = "SNAP";
            }
            else
            {
                snapText.text = "FREE";
            }
        }
    }

    public bool GetSnapToGridToggle()
    {
        return snapToGridToggle;
    }

    public void OnBuildWallButtonClickListener()
    {
        isDeleting = false;
        wallBuilder.StartBuilding();
    }

    public void OnBuildDoorButtonClickListener()
    {
        isDeleting = false;
        doorBuilder.StartBuilding();
    }


    public void OnDeleteButtonClicked()
    {
        StartBuilding();
        if(wallBuilder.IsBuilding())
        {
            wallBuilder.StopBuilding();
        } else if(doorBuilder.IsBuilding())
        {
            doorBuilder.StopBuilding();
        }
        isDeleting = true;
    }


    public Vector3 GetClosestGridPoint(Vector3 point)
    {
        float xPoint = Mathf.RoundToInt(point.x / gridSize);
        float yPoint = Mathf.RoundToInt(point.y / gridSize);
        float zPoint = Mathf.RoundToInt(point.z / gridSize);

        return new Vector3(xPoint * gridSize, yPoint * gridSize, zPoint * gridSize);
    }

    public Vector3 GetClosestGridPoint(Vector3 point, Vector3 offset)
    {
        point -= offset;

        float xPoint = Mathf.RoundToInt(point.x / gridSize);
        float yPoint = Mathf.RoundToInt(point.y / gridSize);
        float zPoint = Mathf.RoundToInt(point.z / gridSize);

        Vector3 result = new Vector3(xPoint * gridSize, yPoint * gridSize, zPoint * gridSize);
        result += offset;
        return result;
    }

    /// <summary>
    /// Returns the point on the object the mouse pointer is currently pointing at.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMousePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        { 
            return hit.point;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Returns the GameObject the mouse pointer is pointing at.
    /// </summary>
    /// <returns></returns>
    public GameObject GetMousePointGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider.gameObject;
        }

        return null;
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
        if (gameObject.tag == "WallStud")
        {
            return true;
        }
        return false;
    }
}