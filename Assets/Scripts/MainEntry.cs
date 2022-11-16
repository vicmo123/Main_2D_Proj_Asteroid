using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MainEntry : MonoBehaviour
{
    UnityEvent startGameEvent;
    bool listnersAdded = false;

    private void Awake()
    {
        GameManager.Instance.Initialize();
    }

    private void Start()
    {
        GameManager.Instance.SecondInitialize();
    }

    private void Update()
    {
        GameManager.Instance.Refresh();

        if (listnersAdded == false)
        {
            BulletManager.Instance.shotFiredEvent.AddListener(StartBulletCoroutine);
            BulletManager.Instance.AsteroidCollisionEvent.AddListener(StartExplosionCoroutine);

            listnersAdded = true;
        }
    }

    private void FixedUpdate()
    {
        GameManager.Instance.PhysicsRefresh();
    }

    public void StartGame()
    {
        GameManager.Instance.StartGameButtonPressed = true;
        SoundManager.Instance.PlayClickSound();
    }

    public void StartBulletCoroutine()
    {
        int i = 0;

        BulletManager.Instance.monoParser(this, i);
    }

    public void StartExplosionCoroutine(Rigidbody2D rb)
    {
        int i = 1;
        Debug.Log("Hey 1");
        BulletManager.Instance.monoParser(this, i, rb);
    }
}
