using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarMenus : MonoBehaviour
{
    //Public Variables
    public GameObject buildMenu;
    public GameObject furnitureMenu;
    public GameObject marketMenu;


    public void OpenBuildMenu()
    {
        CloseAllMenus();
        buildMenu.SetActive(true);
        GameManager.MouseClicked += GameManager_MouseClicked;
    }

    private void GameManager_MouseClicked()
    {
        if(!BuildHelper.IsPointerOverUI())
        {
            CloseAllMenus();
        }
    }

    public void OpenFurnitureMenu()
    {
        CloseAllMenus();
        furnitureMenu.SetActive(true);
        GameManager.MouseClicked += GameManager_MouseClicked;
    }

    public void OpenMarketMenu()
    {
        CloseAllMenus();
        marketMenu.SetActive(true);
        GameManager.MouseClicked += GameManager_MouseClicked;

    }

    public void CloseAllMenus()
    {
        buildMenu.SetActive(false);
        furnitureMenu.SetActive(false);
        marketMenu.SetActive(false);
        GameManager.MouseClicked -= GameManager_MouseClicked;
    }
}
