using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public WallStud wallStart;
    public WallStud wallEnd;

    public void OnDestroy()
    {
        if(wallStart != null)
        {
            wallStart.RemoveWall(this);

        }
        if(wallEnd != null)
        {
            wallEnd.RemoveWall(this);
        }
    }
}
