using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
    #region Public Variables

    public ToolbarMenus toolbarMenus;
    public WallBuilder wallBuilder;
    public float gridSize = .1f;
    public TextMeshProUGUI snapText;
    
    #endregion

    #region Private Variables

    private bool isBuilding;
    private bool isDeleting;
    private bool snapToGridToggle;

    #endregion

    private void Start()
    {
        UpdateSnapToGridText();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            snapToGridToggle = !snapToGridToggle;
            UpdateSnapToGridText();
        }
    }

    public void UpdateSnapToGridText()
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

    public bool GetSnapToGridToggle()
    {
        return snapToGridToggle;
    }

    public void OnBuildWallButtonClickListener()
    {
        isDeleting = false;
        wallBuilder.StartBuilding();
    }

   
    public void OnDeleteButtonClicked()
    {
        wallBuilder.CancelBuild();
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

    public Vector3 GetMousePointNormal()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.normal;
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
}