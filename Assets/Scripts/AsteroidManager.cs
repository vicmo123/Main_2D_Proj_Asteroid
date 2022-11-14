using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager
{
    #region Singleton
    private static AsteroidManager instance;
    public static AsteroidManager Instance
    {
        get
        {
            if (instance == null)
                instance = new AsteroidManager();
            return instance;
        }
    }

    private AsteroidManager() { }
    #endregion

    public GameObject asteroidPrefab;
    public List<Rigidbody2D> tabAsteroid { get; private set; }
    public float Radius { get; private set; }

    public int intialNumberAsteroids;
    public float spawnRate = 3f;
    public int qtyMedium = 2;
    public int qtySmall = 3;

    const float SIZE_BIG_ASTEROID = 1.2f;
    const float SIZE_MEDIUM_ASTEROID = 0.7f;
    const float SIZE_SMALL_ASTEROID = 0.5f;

    const float SPEED_BIG = 1f;
    const float SPEED_MEDIUM = 1.5f;
    const float SPEED_SMALL = 1.7f;

    const string BIG_ASTEROID_TAG = "BigAsteroid";
    const string MEDIUM_ASTEROID_TAG = "MediumAsteroid";
    const string SMALL_ASTEROID_TAG = "SmallAsteroid";

    private float currentTime = 0.0f;
    private float nextActionTime = 0.0f;
    public float period = 3.0f;

    public void Initialize()
    {
        Radius = 0.61f;
        asteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroid");
    }

    public void SecondInitialize(int numAsteroids)
    {
        intialNumberAsteroids = numAsteroids;
        tabAsteroid = new List<Rigidbody2D>();

        BulletManager.Instance.AsteroidCollisionEvent.AddListener(SplitAsteroid);
    }

    int counter = 0;

    public void Refresh()
    {
        currentTime += Time.deltaTime;

        if (currentTime > nextActionTime)
        {
            nextActionTime += period;

            if(counter < intialNumberAsteroids)
            {
                Spawn(counter);
                counter++;
            }
        }


        CheckCollisionWithSpaceShip();

        CleanList();
    }

    public void PhysicsResfresh()
    {
        RespawnPlayerWhenOutOfMap();
    }

    private void Spawn(int i)
    {
        float xVal = Random.Range(GameManager.LIMIT_LEFT, GameManager.LIMIT_RIGHT);
        float yVal = Random.Range(GameManager.LIMIT_BOTTOM, GameManager.LIMIT_TOP);

        if(xVal <= SpaceShipManager.Instance.spaceShip.transform.position.x + 1 || xVal >= SpaceShipManager.Instance.spaceShip.transform.position.x - 1)
        {
            xVal += 2;
        }
        if (yVal <= SpaceShipManager.Instance.spaceShip.transform.position.y + 1 || yVal >= SpaceShipManager.Instance.spaceShip.transform.position.y - 1)
        {
            yVal = 2;
        }

        GameObject asteroid = GameObject.Instantiate(asteroidPrefab);
        asteroid.transform.position = new Vector3(xVal, yVal, -1);
        asteroid.transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        asteroid.transform.localScale = new Vector3(SIZE_BIG_ASTEROID, SIZE_BIG_ASTEROID, 1);
        asteroid.tag = BIG_ASTEROID_TAG;

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>(); 
        rb.velocity = SetVelocity(BIG_ASTEROID_TAG);
        rb.transform.GetChild(0).GetComponent<CircleCollider2D>().isTrigger = true;
        tabAsteroid.Add(rb);
    }

    private void RespawnPlayerWhenOutOfMap()
    {
        foreach (Rigidbody2D asteroid in tabAsteroid)
        {
            if (asteroid != null)
            {
                if (asteroid.transform.position.y - (asteroid.transform.localScale.y / 2) > GameManager.LIMIT_TOP)
                {
                    //top
                    asteroid.transform.position = new Vector3(asteroid.transform.position.x, GameManager.LIMIT_BOTTOM - (asteroid.transform.localScale.y / 2), asteroid.transform.position.z);
                }
                else if (asteroid.transform.position.y + (asteroid.transform.localScale.y / 2) < GameManager.LIMIT_BOTTOM)
                {
                    //bottom
                    asteroid.transform.position = new Vector3(asteroid.transform.position.x, GameManager.LIMIT_TOP + (asteroid.transform.localScale.y / 2), asteroid.transform.position.z);
                }
                else if (asteroid.transform.position.x - (asteroid.transform.localScale.x / 2) > GameManager.LIMIT_RIGHT)
                {
                    //right
                    asteroid.transform.position = new Vector3(GameManager.LIMIT_LEFT - (asteroid.transform.localScale.y / 2), asteroid.transform.position.y, asteroid.transform.position.z);
                }
                else if (asteroid.transform.position.x + (asteroid.transform.localScale.x / 2) < GameManager.LIMIT_LEFT)
                {
                    //left
                    asteroid.transform.position = new Vector3(GameManager.LIMIT_RIGHT + (asteroid.transform.localScale.x / 2), asteroid.transform.position.y, asteroid.transform.position.z);
                }
                else
                {
                    asteroid.transform.position = asteroid.transform.position;
                }
            }
        }
    }

    private void SplitAsteroid(Rigidbody2D collidedAsteroid)
    {
        if (collidedAsteroid.gameObject.CompareTag(BIG_ASTEROID_TAG))
        {
            SpawnSmallerAsteroids(qtyMedium, collidedAsteroid.transform, MEDIUM_ASTEROID_TAG);
        }
        else if (collidedAsteroid.gameObject.CompareTag(MEDIUM_ASTEROID_TAG))
        {
            SpawnSmallerAsteroids(qtySmall, collidedAsteroid.transform, SMALL_ASTEROID_TAG);
        }
        else if (collidedAsteroid.gameObject.CompareTag(SMALL_ASTEROID_TAG))
        {
            GameObject.Destroy(collidedAsteroid.gameObject);
        }
    }

    private void SpawnSmallerAsteroids(int quantity, Transform _position, string tag)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject asteroid = GameObject.Instantiate(asteroidPrefab);
            asteroid.transform.position = _position.position;
            asteroid.transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);

            float scale = SIZE_SMALL_ASTEROID;

            switch (tag)
            {
                case BIG_ASTEROID_TAG:
                    scale = SIZE_BIG_ASTEROID;
                    break;
                case MEDIUM_ASTEROID_TAG:
                    scale = SIZE_MEDIUM_ASTEROID;
                    break;
                case SMALL_ASTEROID_TAG:
                    scale = SIZE_SMALL_ASTEROID;
                    break;
                default:
                    Debug.Log("Unhandleld switch" + tag);
                    break;
            }

            asteroid.transform.localScale = new Vector3(scale, scale, 1);
            asteroid.tag = tag;

            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
            rb.velocity = SetVelocity(tag);
            rb.transform.GetChild(0).GetComponent<CircleCollider2D>().isTrigger = true;
            tabAsteroid.Add(rb);
        }

        GameObject.Destroy(_position.gameObject);
    }

    private Vector2 SetVelocity(string tag)
    {
        float xVal = Random.Range(-1f, 1f);
        float yVal = Random.Range(-1f, 1f);

        Vector2 velocity = new Vector2(xVal, yVal).normalized;

        switch (tag)
        {
            case BIG_ASTEROID_TAG:
                velocity *= SPEED_BIG;
                break;
            case MEDIUM_ASTEROID_TAG:
                velocity *= SPEED_MEDIUM;
                break;
            case SMALL_ASTEROID_TAG:
                velocity *= SPEED_SMALL;
                break;
            default:
                Debug.Log("Unhandleld switch" + tag);
                break;
        }

        return velocity;
    }

    private void CheckCollisionWithSpaceShip()
    {
        foreach (Rigidbody2D asteroid in tabAsteroid)
        { 
            if ((asteroid.position - (Vector2)SpaceShipManager.Instance.spaceShip.transform.position).magnitude <= (Radius * asteroid.transform.localScale.x) + SpaceShipManager.Instance.spaceShipRadius && asteroid != null)
            {
                Debug.Log("Ship is dead");
                //GameObject.Destroy(SpaceShipManager.Instance.spaceShip.gameObject);
                //GameObject.Destroy(asteroid.gameObject);
            }
        }
    }

    private void CleanList()
    {
        tabAsteroid.RemoveAll(item => item == null);
    }
}