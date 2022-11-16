using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStatsUiManager
{
    #region Singleton
    private static PlayerStatsUiManager instance;
    public static PlayerStatsUiManager Instance
    {
        get
        {
            if (instance == null)
                instance = new PlayerStatsUiManager();
            return instance;
        }
    }

    private PlayerStatsUiManager() { }
    #endregion

    private GameObject playerStatsUI;
    private GameObject heartPrefab;
    private Text scoreText;
    private Text roundText;
    private Transform lifePanel;

    private GameObject[] tabLives;
    public int numLives { get; set; }

    private string scoreString { get; set; }

    public UnityEvent lostLifeEvent;

    public void Initialize(int MaxLives)
    {
        lostLifeEvent = new UnityEvent();

        playerStatsUI = MainMenuManager.Instance.canvas.transform.GetChild(1).gameObject;

        scoreText = playerStatsUI.transform.GetChild(0).gameObject.GetComponent<Text>();
        roundText = playerStatsUI.transform.GetChild(1).gameObject.GetComponent<Text>();
        lifePanel = playerStatsUI.transform.GetChild(2);

        heartPrefab = Resources.Load<GameObject>("Prefabs/ImageHeart");
        scoreString = "Score: ";

        tabLives = new GameObject[MaxLives];
    }

    public void SecondInitialize(int initialNumLives)
    {
        ShowStatsUI();
        for (int i = 0; i < initialNumLives; i++)
        {
            AddLife(i);
        }
    }

    public void Refresh(int score, int round)
    {
        scoreText.text = scoreString + score;
        roundText.text = 0 + round.ToString();

        GameManager.Instance.ActualnumLives = numLives; 
    }

    public void ShowStatsUI()
    {
        playerStatsUI.SetActive(true);
    }

    public void AddLife(int i)
    {
        if(numLives < 6)
        {
            GameObject heart = GameObject.Instantiate(heartPrefab);
            tabLives[i] = heart;
            tabLives[i].transform.SetParent(lifePanel.transform);

            numLives++;
        }
    }

    public void RemoveLife()
    {
        if(numLives > 0 && numLives < GameManager.Instance.MaxNumberLives + 1)
        {
            GameObject.Destroy(tabLives[numLives - 1].gameObject);
            numLives--;

            lostLifeEvent.Invoke();
        }
    }
}
