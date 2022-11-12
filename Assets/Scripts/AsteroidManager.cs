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
    public GameObject[] tabAsteroid;
    public int numberAsteroids;
    public float spawnRate = 3f;

    const float SIZE_BIG_ASTEROID = 1.2f;
    const float SIZE_MEDIUM_ASTEROID = 0.7f;
    const float SIZE_SMALL_ASTEROID = 0.3f;

    private float nextActionTime = 0.0f;
    public float period = 3.0f;

    public void Initialize()
    {
        asteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroid");
    }

    public void SecondInitialize(int numAsteroids)
    {
        numberAsteroids = numAsteroids;
        tabAsteroid = new GameObject[numberAsteroids];
    }

    int counter = 0;

    public void Refresh()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;

            if(counter < numberAsteroids)
            {
                Spawn(counter);
                counter++;
            }
        }
    }

    public void PhysicsResfresh()
    {

    }

    public void Spawn(int i)
    {
        float xVal = Random.Range(GameManager.LIMIT_LEFT, GameManager.LIMIT_RIGHT);
        float yVal = Random.Range(GameManager.LIMIT_BOTTOM, GameManager.LIMIT_TOP);

        GameObject asteroid = GameObject.Instantiate(asteroidPrefab);
        asteroid.transform.position = new Vector3(xVal, yVal, -1);

        tabAsteroid[i] = asteroid;
    }

    private void RespawnPlayerWhenOutOfMap()
    {
        for (int i = 0; i < numberAsteroids; i++)
        {
            if (tabAsteroid[i].transform.position.y - (tabAsteroid[i].transform.localScale.y / 2) > GameManager.LIMIT_TOP)
            {
                //top
                tabAsteroid[i].transform.position = new Vector3(tabAsteroid[i].transform.position.x, GameManager.LIMIT_BOTTOM - (tabAsteroid[i].transform.localScale.y / 2), tabAsteroid[i].transform.position.z);
            }
            else if (tabAsteroid[i].transform.position.y + (tabAsteroid[i].transform.localScale.y / 2) < GameManager.LIMIT_BOTTOM)
            {
                //bottom
                tabAsteroid[i].transform.position = new Vector3(tabAsteroid[i].transform.position.x, GameManager.LIMIT_TOP + (tabAsteroid[i].transform.localScale.y / 2), tabAsteroid[i].transform.position.z);
            }
            else if (tabAsteroid[i].transform.position.x - (tabAsteroid[i].transform.localScale.x / 2) > GameManager.LIMIT_RIGHT)
            {
                //right
                tabAsteroid[i].transform.position = new Vector3(GameManager.LIMIT_LEFT - (tabAsteroid[i].transform.localScale.y / 2), tabAsteroid[i].transform.position.y, tabAsteroid[i].transform.position.z);
            }
            else if (tabAsteroid[i].transform.position.x + (tabAsteroid[i].transform.localScale.x / 2) < GameManager.LIMIT_LEFT)
            {
                //left
                tabAsteroid[i].transform.position = new Vector3(GameManager.LIMIT_RIGHT + (tabAsteroid[i].transform.localScale.x / 2), tabAsteroid[i].transform.position.y, tabAsteroid[i].transform.position.z);
            }
            else
            {
                tabAsteroid[i].transform.position = tabAsteroid[i].transform.position;
            }
        }
    }
}