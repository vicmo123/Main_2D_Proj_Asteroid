using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager
{
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

    public void Initialize()
    {

    }

    public void Refresh()
    {

    }

    public void CreateMainMenu()
    {

    }
}
