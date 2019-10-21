using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public WallStud wallStart;
    public WallStud wallEnd;
    public Material[] normalMaterials;
    public Material[] wallInvisible;

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
        this.GetComponent<Renderer>().materials = normalMaterials;
    }

    public void UseInvisibleMaterial()
    {
        this.GetComponent<Renderer>().materials = wallInvisible;
    }
}
