using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MainEntry : MonoBehaviour
{
    UnityEvent startGameEvent;

    private void Start()
    {
        GameManager.Instance.Initialize();
    }

    private void Update()
    {
        GameManager.Instance.Refresh();
    }

    private void FixedUpdate()
    {
        GameManager.Instance.PhysicsRefresh();
    }

    public void StartGame()
    {
        GameManager.Instance.StartGameButtonPressed = true;
    }
}
