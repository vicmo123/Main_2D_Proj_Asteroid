using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }

    private GameManager() { }
    #endregion

    enum GamePhase
    {
        MainMenuWaiting,
        Setup,
        AsteroidDestruction,
        Reset
    }

    GamePhase gamePhases;

    private int round = 0;
    private int initalNumAsteroids = 15;
    private int additionNumAsteroids = 5;
    private int numberAsteroids;

    //Map size
    public const float LIMIT_TOP = 7.05f;
    public const float LIMIT_BOTTOM = -7.05f;
    public const float LIMIT_LEFT = -9.8f;
    public const float LIMIT_RIGHT = 9.8f;

    public bool StartGameButtonPressed { get; set; }

    public void Initialize()
    {
        StartGameButtonPressed = false;

        gamePhases = GamePhase.MainMenuWaiting;

        MainMenuManager.Instance.Initialize();
        SpaceShipManager.Instance.Initialize();
        AsteroidManager.Instance.Initialize();
        SoundManager.Instance.Initialize();
    }

    public void Refresh()
    {
        switch (gamePhases)
        {
            case GamePhase.MainMenuWaiting:
                MainMenuWaitingUpdate();
                break;
            case GamePhase.Setup:
                SetupUpdate();
                break;
            case GamePhase.AsteroidDestruction:
                AsteroidDestructionUpdate();
                break;
            case GamePhase.Reset:
                ResetUpdate();
                break;
            default:
                Debug.Log("Unhandeled case : " + gamePhases);
                break;
        }

        SoundManager.Instance.Refresh();
    }

    public void PhysicsRefresh()
    {
        if(gamePhases == GamePhase.AsteroidDestruction)
        {
            SpaceShipManager.Instance.PhysicsRefresh();
            AsteroidManager.Instance.PhysicsResfresh();
        }
    }

    public void MainMenuWaitingUpdate()
    {
        MainMenuManager.Instance.Refresh(StartGameButtonPressed);

        if (StartGameButtonPressed) {
            gamePhases = GamePhase.Setup;
        }
    }

    private void SetupUpdate()
    {
        SpaceShipManager.Instance.SecondInitialize();
        AsteroidManager.Instance.SecondInitialize(initalNumAsteroids + round * additionNumAsteroids);

        gamePhases = GamePhase.AsteroidDestruction;
    }

    private void AsteroidDestructionUpdate()
    {
        SpaceShipManager.Instance.Refresh();
        AsteroidManager.Instance.Refresh();
    }

    private void ResetUpdate()
    {

    }
}
