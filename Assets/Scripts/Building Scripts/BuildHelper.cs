using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class BuildHelper
{
    public static Vector3 GetMousePoint()
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
    public static GameObject GetMousePointGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    public static Vector3 GetClosestGridPoint(Vector3 point)
    {
        float xPoint = Mathf.RoundToInt(point.x / BuildManager.GetGridSize());
        float yPoint = Mathf.RoundToInt(point.y / BuildManager.GetGridSize()); 
        float zPoint = Mathf.RoundToInt(point.z / BuildManager.GetGridSize());

        return new Vector3(xPoint * BuildManager.GetGridSize(), yPoint * BuildManager.GetGridSize(), zPoint * BuildManager.GetGridSize());
    }

    public static Vector3 GetClosestGridPoint(Vector3 point, Vector3 offset)
    {
        point -= offset;

        float xPoint = Mathf.RoundToInt(point.x / BuildManager.GetGridSize());
        float yPoint = Mathf.RoundToInt(point.y / BuildManager.GetGridSize());
        float zPoint = Mathf.RoundToInt(point.z / BuildManager.GetGridSize());

        Vector3 result = new Vector3(xPoint * BuildManager.GetGridSize(), yPoint * BuildManager.GetGridSize(), zPoint * BuildManager.GetGridSize());
        result += offset;
        return result;
    }

    public static bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
