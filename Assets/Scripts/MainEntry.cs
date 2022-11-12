using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEntry : MonoBehaviour
{
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
}
