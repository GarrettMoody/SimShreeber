using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class MultiClickObjectBuilder : MonoBehaviour
{
    public BuildManager buildManager;
    public GameObject endpointPrefab;
    public GameObject objectPrefab;
    public bool continueBuilding;

    private bool isBuilding;
    private GameObject objectStart;
    private GameObject objectEnd;
    private GameObject objectBeingBuilt;
    private bool startSet;
    private float objectHeight;
    private float endpointHeight;

    public void Start()
    {
        endpointHeight = endpointPrefab.GetComponent<MeshCollider>().bounds.size.y;
        objectHeight = objectPrefab.GetComponent<MeshCollider>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            Vector3 mousePoint;

            if (buildManager.GetSnapToGridToggle())
            {
                mousePoint = buildManager.GetClosestGridPoint(buildManager.GetMousePoint());
            }
            else
            {
                mousePoint = buildManager.GetMousePoint();
            }


            mousePoint = new Vector3(mousePoint.x, mousePoint.y + endpointHeight / 2, mousePoint.z);

            //Build hasn't started yet
            if (!startSet)
            {
                GameObject mouseObject = buildManager.GetMousePointGameObject();

                if (mouseObject != null)
                {
                    objectStart.gameObject.SetActive(true);
                    objectStart.transform.position = mousePoint;
                }
            }
            //Build has started
            else
            {
                objectEnd.gameObject.SetActive(true);
                objectEnd.transform.position = mousePoint;
                objectEnd.transform.LookAt(2 * objectEnd.transform.position - objectStart.transform.position); //Look in opposite direction

                UpdateObject();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!startSet)
                {
                    SetStart(objectStart);
                }
                //Build has started
                else
                {
                    if (continueBuilding)
                    {
                        ContinueBuild();
                    }
                    else
                    {
                        ActivateColliders();
                        objectEnd = null;
                        buildManager.StopBuilding();
                    }
                }
            }
        }
    }

    public void StartBuilding()
    {
        buildManager.StartBuilding();
        isBuilding = true;
        startSet = false;
        objectStart = Instantiate(endpointPrefab);
    }

    /// <summary>
    /// Cancels the current build.
    /// </summary>
    public void StopBuilding()
    {
        isBuilding = false;
        if (objectStart != null)
        {
            objectStart.gameObject.GetComponent<Collider>().enabled = true;
        }
        if (objectEnd != null)
        {
            objectEnd.gameObject.GetComponent<Collider>().enabled = true;
        }
        if (!startSet && objectStart != null)
        {
            Destroy(objectStart.gameObject);
        }
        if (objectEnd != null)
        {
            Destroy(objectEnd.gameObject);
            Destroy(objectBeingBuilt.gameObject);
        }
    }

    public void ContinueBuild()
    {
        //Place wall and continue building
        //Enable Colliders
        ActivateColliders();

        objectStart = objectEnd;
        objectBeingBuilt = Instantiate(objectPrefab);
        objectBeingBuilt.transform.position = buildManager.GetMousePoint();
        objectEnd = Instantiate(endpointPrefab);
        objectEnd.transform.position = buildManager.GetMousePoint();
    }

    public void UpdateObject()
    {
        objectStart.transform.LookAt(objectEnd.transform);

        float distance = Vector3.Distance(objectStart.transform.position, objectEnd.transform.position);
        objectBeingBuilt.transform.position = objectStart.transform.position + distance / 2 * objectStart.transform.forward;
        objectBeingBuilt.transform.LookAt(objectEnd.transform);
        objectBeingBuilt.transform.localScale = new Vector3(objectBeingBuilt.transform.localScale.x,
                                                            objectBeingBuilt.transform.localScale.y,
                                                            (distance * endpointPrefab.transform.localScale.z) - objectStart.GetComponent<MeshCollider>().sharedMesh.bounds.size.z);
    }

    public void SetStart(GameObject wallStartValue)
    {
        objectStart = wallStartValue;
        objectStart.GetComponent<Collider>().enabled = true;
        objectEnd = Instantiate(endpointPrefab);
        objectBeingBuilt = Instantiate(objectPrefab);
        objectBeingBuilt.transform.position = buildManager.GetMousePoint();
        objectEnd.transform.position = buildManager.GetMousePoint();
        startSet = true;
    }

    public bool IsBuilding()
    {
        return isBuilding;
    }

    private void ActivateColliders()
    {
        foreach (Collider colliders in objectStart.GetComponents<Collider>())
        {
            colliders.enabled = true;
        }
        foreach (Collider colliders in objectEnd.GetComponents<Collider>())
        {
            colliders.enabled = true;
        }
        foreach (Collider colliders in objectBeingBuilt.GetComponents<Collider>())
        {
            colliders.enabled = true;
        }
    }
}
