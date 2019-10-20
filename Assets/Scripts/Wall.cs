using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public WallStud wallStart;
    public WallStud wallEnd;
    public Material wallNormal;
    public Material wallInvisible;

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

    public void UseNormalMaterial()
    {
        this.GetComponent<Renderer>().material = wallNormal;
    }

    public void UseInvisibleMaterial()
    {
        this.GetComponent<Renderer>().material = wallInvisible;
    }
}
