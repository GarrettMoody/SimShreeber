using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfSlot : MonoBehaviour
{
    [HideInInspector]
    public bool isFilled;
    [HideInInspector]
    public GameObject objectInSlot { get; private set; }
   
    public ShelfSlot parent;
    public List<ShelfSlot> children = new List<ShelfSlot>();

    public void Start()
    {
        if(this.transform.parent.GetComponentInParent<ShelfSlot>() != null)
        {
            parent = this.transform.parent.GetComponentInParent<ShelfSlot>();
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).GetComponent<ShelfSlot>() != null)
            {
                children.Add(this.transform.GetChild(i).GetComponent<ShelfSlot>());
            }
        }
    }

    public void FillSlot(GameObject newObject)
    {
        if(!isFilled)
        {
            objectInSlot = newObject;
            isFilled = true;
        }
    }

    public void EmptySlot()
    {
        if(objectInSlot != null)
        {
            Destroy(objectInSlot);
            isFilled = false;
        }
    }

    public bool IsParentFilled()
    {
        if(parent != null)
        {
            return parent.isFilled;
        }
        return true;
    }

    /// <summary>
    /// This method will return a child ShelfSlot if any of the slots are filled. If no child
    /// slots are filled, it will return null.
    /// </summary>
    /// <returns>A ShelfSlot child which is filled.</returns>
    public ShelfSlot GetAnyFilledChild()
    {
        foreach(ShelfSlot slot in children)
        {
            if(slot.isFilled)
            {
                return slot;
            }
        }

        return null;
    }
}
