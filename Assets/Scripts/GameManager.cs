using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public int round { get; set; }
    private int initalNumAsteroids = 2;
    private int additionNumAsteroids = 3;
    private int numberAsteroids;

    //Map size
    public const float LIMIT_TOP = 7.05f;
    public const float LIMIT_BOTTOM = -7.05f;
    public const float LIMIT_LEFT = -9.8f;
    public const float LIMIT_RIGHT = 9.8f;

    //Player stats
    public int InitialNumberLives = 3;
    public int MaxNumberLives = 6;
    public int ActualnumLives { get; set; }
    public int score { get; set; }

    public bool StartGameButtonPressed { get; set; }

    public bool won;

    public void Initialize()
    {
        StartGameButtonPressed = false;

        round = 0;
        score = 0;

        ActualnumLives = InitialNumberLives;

        gamePhases = GamePhase.MainMenuWaiting;

        MainMenuManager.Instance.Initialize();
        PlayerStatsUiManager.Instance.Initialize(MaxNumberLives);
        SpaceShipManager.Instance.Initialize();
        AsteroidManager.Instance.Initialize();
        BulletManager.Instance.Initialize();
        SoundManager.Instance.Initialize();
    }

    public void SecondInitialize()
    {
        SoundManager.Instance.PlayMainMusic();

        PlayerStatsUiManager.Instance.lostLifeEvent.AddListener(LostLife);

        won = false;
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
    }

    public void PhysicsRefresh()
    {
        if(gamePhases == GamePhase.AsteroidDestruction)
        {
            SpaceShipManager.Instance.PhysicsRefresh();
            AsteroidManager.Instance.PhysicsResfresh();
            BulletManager.Instance.PhysicsRefresh();
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
        BulletManager.Instance.SecondInitialize();
        PlayerStatsUiManager.Instance.SecondInitialize(ActualnumLives);

        gamePhases = GamePhase.AsteroidDestruction;
    }

    private void AsteroidDestructionUpdate()
    {
        BulletManager.Instance.Refresh();
        SpaceShipManager.Instance.Refresh();
        AsteroidManager.Instance.Refresh();
        PlayerStatsUiManager.Instance.Refresh(score, round);

        if(AsteroidManager.Instance.QuantityOfAsteroidToDestroyInRound == 0)
        {
            won = true;
            gamePhases = GamePhase.Reset;
        }

        if(ActualnumLives == 0)
        {
            GameOver();
        }
    }

    private void ResetUpdate()
    {
        EndGame(won);

        gamePhases = GamePhase.AsteroidDestruction;
    }

    private void LostLife()
    {
        gamePhases = GamePhase.Reset; 
    }

    public void EndGame(bool _won)
    {
        if (_won)
        {
            round++;
            AsteroidManager.Instance.SecondInitialize(initalNumAsteroids + round * additionNumAsteroids);
            PlayerStatsUiManager.Instance.AddLife(PlayerStatsUiManager.Instance.numLives);

            SoundManager.Instance.PlayWinSound();
        }
        else
        {
            round = round;
            AsteroidManager.Instance.ClearAllAsteroids();
            AsteroidManager.Instance.SecondInitialize(initalNumAsteroids + round * additionNumAsteroids);
        }

        won = false;
    }

    public void GameOver()
    {
        round = 0;
        score = 0;

        PlayerStatsUiManager.Instance.SecondInitialize(InitialNumberLives);

        won = false;

        SoundManager.Instance.PlayLooseSound();
    }

    public void ClearSingleton()
    {
        instance = null;
    }
}
