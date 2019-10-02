using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarMenus : MonoBehaviour
{
    //Public Variables
    public GameObject buildMenu;

    public void OpenBuildMenu()
    {
        buildMenu.SetActive(true);
    }

    public void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
    }

    public void CloseAllMenus()
    {
        CloseBuildMenu();
    }
}
