using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemManager")]
public class ItemManager : ScriptableObject
{
    public BuildableItem[] items;
    public Dictionary<string, BuildableItem> itemList = new Dictionary<string, BuildableItem>();
    private static ItemManager instance;

    public void Awake()
    {
        foreach(BuildableItem item in items)
        {
            itemList.Add(item.itemName, item);
        }
    }

    public static ItemManager Instance()
    {
        return instance;
    }
}
