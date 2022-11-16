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

    public GameObject canvas { get; private set; }
    private GameObject mainMenu;

    public void Initialize()
    {
        canvas = GameObject.FindGameObjectWithTag("MainMenu");
        mainMenu = canvas.transform.GetChild(0).gameObject;

        ShowMainMenu();
    }

    public void Refresh(bool disableMenu)
    {
        if (disableMenu)
        {
            mainMenu.SetActive(false);
        }
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
    }
}
