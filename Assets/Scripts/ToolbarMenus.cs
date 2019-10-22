using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarMenus : MonoBehaviour
{
    //Public Variables
    public GameObject buildMenu;
    public GameObject furnitureMenu;

    public void OpenBuildMenu()
    {
        CloseAllMenus();
        buildMenu.SetActive(true);

    }

    public void OpenFurnitureMenu()
    {
        CloseAllMenus();
        furnitureMenu.SetActive(true);
    }

    public void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
    }

    public void CloseAllMenus()
    {
        CloseBuildMenu();
        furnitureMenu.SetActive(false);
    }
}
