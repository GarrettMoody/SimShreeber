using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStud : MonoBehaviour
{
    public Material normalMaterial;
    public Material buildingMaterial;

	private List<Wall> walls;

    public void Start()
    {
        walls = new List<Wall>();
    }

    public void OnDestroy()
    {
        DestroyAllWalls();
    }

    public void AddWall(Wall wall)
	{
		walls.Add(wall);
	}

    public List<Wall> GetWalls()
	{
		return walls;
	}

    public void RemoveWall(Wall wall)
    {
        walls.Remove(wall);
    }

    public void DestroyAllWalls()
	{
        foreach (Wall wall in walls)
		{
            Destroy(wall.gameObject);
		}
	}

    public void UseNormalMaterial()
    {
        this.gameObject.GetComponent<Renderer>().material = normalMaterial;
    }

    public void UseBuildingMaterial()
    {
        this.gameObject.GetComponent<Renderer>().material = buildingMaterial;
    }
}
