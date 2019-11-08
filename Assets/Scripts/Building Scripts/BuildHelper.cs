using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
