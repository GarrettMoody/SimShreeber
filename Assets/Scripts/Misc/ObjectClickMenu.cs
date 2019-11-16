using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ObjectClickMenu : MonoBehaviour
{
    private static ObjectClickMenu instance;

    public void Awake()
    {
        if(instance != null)
        {
            CloseObjectClickMenu();
        } else
        {
            instance = this;
            GameManager.MouseClicked += GameManager_MouseClicked;
        }
    }

    public void OnDestroy()
    {
        GameManager.MouseClicked -= GameManager_MouseClicked;
    }

    private void GameManager_MouseClicked()
    {
        if(BuildHelper.GetMousePointGameObject() != instance.gameObject)
        {
            CloseObjectClickMenu();
        }
    }

    public static ObjectClickMenu Instance()
    {
        return instance;
    }

    public static void CloseObjectClickMenu()
    {
        Destroy(instance.gameObject);
    }

    public static void OpenContextMenu(BuildableItem item)
    {
        foreach(KeyValuePair<string, Action> action in item.contextActions)
        {
            GameObject menuItem = (GameObject)Instantiate(Resources.Load("Prefabs/MenuButton"), instance.gameObject.transform);
            menuItem.name = action.Key;
            menuItem.GetComponentInChildren<TextMeshProUGUI>().text = action.Key;

            menuItem.GetComponent<Button>().onClick.AddListener(() => { action.Value.Invoke(); });
            menuItem.GetComponent<Button>().onClick.AddListener(() => { CloseObjectClickMenu(); });

        }
    }
}
