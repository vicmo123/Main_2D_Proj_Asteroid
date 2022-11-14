using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipManager
{
    #region Singleton
    private static SpaceShipManager instance;
    public static SpaceShipManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SpaceShipManager();
            return instance;
        }
    }

    private SpaceShipManager() { }
    #endregion

    //SPaceship
    private GameObject spaceShipPrefab;
    public GameObject spaceShip { get; private set; }
    private Rigidbody2D rb;
    public float spaceShipRadius = 0.48f;
    public float rocketForce = 3.0f;
    public float rocketTorque = 1.1f;

    //Thruster
    public Transform thruster;
    bool thrusterActive;
    public float thrusterGrowth = 0.5f;
    public float thrusterMin = 0;
    public float thrusterMax = 0.8f;

    public void Initialize()
    {
        spaceShipPrefab = Resources.Load<GameObject>("Prefabs/Spaceship");
    }

    public void SecondInitialize()
    {
        CreateSpaceship();
        rb = spaceShip.GetComponent<Rigidbody2D>();
        thruster = spaceShip.transform.GetChild(1);
    }

    public void Refresh()
    {
        UpdateThrusterScale();
        RespawnPlayerWhenOutOfMap();
    }

    public void PhysicsRefresh()
    {
        UpdateMoveShip();
    }

    private void CreateSpaceship()
    {
        spaceShip = GameObject.Instantiate(spaceShipPrefab);
        spaceShip.transform.position = new Vector3(0, 0, -1);
    }

    public void UpdateMoveShip()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Full force reactor
            rb.AddRelativeForce(new Vector2(0, rocketForce), ForceMode2D.Force);
            thrusterActive = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //Half force reactor for landing
            rb.AddRelativeForce(new Vector2(0, rocketForce / 2), ForceMode2D.Force);
            thrusterActive = true;
        }
        else
        {
            thrusterActive = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(rocketTorque);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-rocketTorque);
        }
    }

    private void UpdateThrusterScale()
    {
        if (thrusterActive)
        {
            //Grow thruster
            thruster.localScale += new Vector3(0, thrusterGrowth * Time.deltaTime, 0);

            //Decrease thruster
            if (thruster.localScale.y > thrusterMax)
            {
                thruster.localScale = new Vector3(thruster.localScale.x, thrusterMax, thruster.localScale.z);
            }
        }
        else
        {
            //Decrease thruster if not on
            thruster.localScale += new Vector3(0, -thrusterGrowth * Time.deltaTime, 0);

            //Keeps from going backwards
            if (thruster.localScale.y < thrusterMin)
            {
                thruster.localScale = new Vector3(thruster.localScale.x, thrusterMin, thruster.localScale.z);
            }
        }
    }

    private void RespawnPlayerWhenOutOfMap()
    {

        if (spaceShip.transform.position.y - (spaceShip.transform.localScale.y / 2)  > GameManager.LIMIT_TOP)
        {
            //top
            spaceShip.transform.position = new Vector3(spaceShip.transform.position.x, GameManager.LIMIT_BOTTOM - (spaceShip.transform.localScale.y / 2), spaceShip.transform.position.z);
        }
        else if (spaceShip.transform.position.y + (spaceShip.transform.localScale.y / 2) < GameManager.LIMIT_BOTTOM)
        {
            //bottom
            spaceShip.transform.position = new Vector3(spaceShip.transform.position.x, GameManager.LIMIT_TOP + (spaceShip.transform.localScale.y / 2), spaceShip.transform.position.z);
        }
        else if (spaceShip.transform.position.x - (spaceShip.transform.localScale.x / 2) > GameManager.LIMIT_RIGHT)
        {
            //right
            spaceShip.transform.position = new Vector3(GameManager.LIMIT_LEFT - (spaceShip.transform.localScale.y / 2), spaceShip.transform.position.y, spaceShip.transform.position.z);
        }
        else if (spaceShip.transform.position.x + (spaceShip.transform.localScale.x / 2) < GameManager.LIMIT_LEFT)
        {
            //left
            spaceShip.transform.position = new Vector3(GameManager.LIMIT_RIGHT + (spaceShip.transform.localScale.x / 2), spaceShip.transform.position.y, spaceShip.transform.position.z);
        }
        else
        {
            spaceShip.transform.position = spaceShip.transform.position;
        }
    }
}
