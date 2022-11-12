using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SoundManager();
            return instance;
        }
    }

    private SoundManager() { }

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
