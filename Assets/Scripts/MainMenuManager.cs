using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager
{
    #region Singleton
    private static MainMenuManager instance;
    public static MainMenuManager Instance
    {
        get
        {
            if (instance == null)
                instance = new MainMenuManager();
            return instance;
        }
    }

    private MainMenuManager() { }
    #endregion

    private GameObject mainMenu;
    
    public void Initialize()
    {
        mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        mainMenu.SetActive(true);
    }

    public void Refresh(bool disableMenu)
    {
        if (disableMenu)
        {
            mainMenu.SetActive(false);
        }
    }

    public void CreateMainMenu()
    {

    }
}
