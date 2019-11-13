using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class WarehouseShelf : BuildableItem
{
   
    private List<ShelfSlot> shelfSlots = new List<ShelfSlot>();
    public int shelfSlotsRemaining { get; private set; }

    public static event Action<WarehouseShelf>  ShelfBuilt = delegate { };
    public static event Action<WarehouseShelf> ShelfDestroyed = delegate { };
   
    public void OnDestroy()
    {
        foreach(ShelfSlot slot in shelfSlots)
        {
            if(slot.objectInSlot != null)
            {
                Destroy(slot.objectInSlot);
            }
        }
        ShelfDestroyed(this);
    }

    public new void Awake()
    {
        base.Awake();
        foreach (ShelfSlot slot in this.GetComponentsInChildren<ShelfSlot>())
        {
            shelfSlots.Add(slot);
        }
        UpdateShelfSlotsRemaining();
        ShelfBuilt(this);
    }

    public void RemoveRandomSlot()
    {
        ShelfSlot slot = GetRandomFilledSlot();
        slot.EmptySlot();
        UpdateShelfSlotsRemaining();
    }

    public void FillRandomSlot(GameObject newObject)
    {
        ShelfSlot slot = GetRandomEmptySlot();
        newObject.transform.position = slot.transform.position;
        newObject.transform.rotation = slot.transform.rotation;
        slot.FillSlot(newObject);
        UpdateShelfSlotsRemaining();
    }

    private ShelfSlot GetRandomEmptySlot()
    {
        if (IsShelfFull())
        {
            return null;
        }

        List<ShelfSlot> emptySlots = new List<ShelfSlot>();
        emptySlots = shelfSlots.FindAll(x => !x.isFilled);

        int random = UnityEngine.Random.Range(0, emptySlots.Count);
        bool found = false;
        ShelfSlot slot = emptySlots[random];
        while(!found)
        {
            if(!slot.IsParentFilled())
            {
				slot = slot.parent;
            } else
			{
				found = true;
			}
        }

        return slot;

    }

    private ShelfSlot GetRandomFilledSlot()
    {
        if (IsShelfEmpty())
        {
            return null;
        }

        List<ShelfSlot> filledSlots = new List<ShelfSlot>();
        filledSlots = shelfSlots.FindAll(x => x.isFilled);

        int random = UnityEngine.Random.Range(0, filledSlots.Count);
        bool found = false;
        ShelfSlot slot = filledSlots[random];
        while (!found)
        {
            if (slot.GetAnyFilledChild() != null)
            {
                slot = slot.GetAnyFilledChild();
            } else
            {
                found = true;
            }
        }

        return slot;

    }

    public bool IsShelfFull()
    {
        return shelfSlots.Select(x => !x.isFilled) == null;
    }

    public bool IsShelfEmpty()
    {
        List<ShelfSlot> filledSlots = shelfSlots.FindAll(x => x.isFilled);
        if(filledSlots == null || filledSlots.Count == 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void UpdateShelfSlotsRemaining()
    {
        List<ShelfSlot> emptySlots = new List<ShelfSlot>();
        emptySlots = shelfSlots.FindAll(x => !x.isFilled);
        shelfSlotsRemaining = emptySlots.Count();
    }

}
