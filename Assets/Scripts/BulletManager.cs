using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletManager
{
    #region Singleton
    private static BulletManager instance;
    public static BulletManager Instance
    {
        get
        {
            if (instance == null)
                instance = new BulletManager();
            return instance;
        }
    }

    private BulletManager() { }
    #endregion

    private GameObject bulletPrefab;
    private Transform spaceShipPosition;

    //from circle collider -> radius
    private float bulletRadius = 0.15f;
    private float speed = 10f;
    private float timeBeforeDestruction = 3f;

    private List<Rigidbody2D> tabBullets;

    public UnityEvent shotFiredEvent;
    public UnityEvent<Rigidbody2D> AsteroidCollisionEvent;

    public void monoParser(MonoBehaviour mono)
    {
        //We can now use StartCoroutine from MonoBehaviour in a non MonoBehaviour script
        mono.StartCoroutine(MoveAndDetroyBullet());

        //And also use StopCoroutine function
        mono.StopCoroutine(MoveAndDetroyBullet());
    }

    public void Initialize()
    {
        shotFiredEvent = new UnityEvent();
        AsteroidCollisionEvent = new UnityEvent<Rigidbody2D>();

        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

        tabBullets = new List<Rigidbody2D>();
    }

    public void SecondInitialize()
    {
        spaceShipPosition = SpaceShipManager.Instance.spaceShip.transform;
    }

    public void Refresh()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shotFiredEvent.Invoke();
        }

        CleanList();

        CheckCollisionWithAsteroid();
        RespawnPlayerWhenOutOfMap();
    }

    public void PhysicsRefresh()
    {
       
    }

    IEnumerator MoveAndDetroyBullet()
    {  
        GameObject bullet = GameObject.Instantiate(bulletPrefab, spaceShipPosition.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = spaceShipPosition.up.normalized * speed;

        tabBullets.Add(rb);

        yield return new WaitForSeconds(timeBeforeDestruction);

        if(rb != null)
        {
            GameObject.Destroy(rb.gameObject);
        }
        tabBullets.Remove(rb);
    }

    private void RespawnPlayerWhenOutOfMap()
    {
        if(tabBullets.Count > 0)
        {
            foreach (Rigidbody2D bullet in tabBullets)
            {
                if (bullet != null)
                {
                    if (bullet.transform.position.y - (bullet.transform.localScale.y / 2) > GameManager.LIMIT_TOP)
                    {
                        //top
                        bullet.transform.position = new Vector3(bullet.transform.position.x, GameManager.LIMIT_BOTTOM - (bullet.transform.localScale.y / 2), bullet.transform.position.z);
                    }
                    else if (bullet.transform.position.y + (bullet.transform.localScale.y / 2) < GameManager.LIMIT_BOTTOM)
                    {
                        //bottom
                        bullet.transform.position = new Vector3(bullet.transform.position.x, GameManager.LIMIT_TOP + (bullet.transform.localScale.y / 2), bullet.transform.position.z);
                    }
                    else if (bullet.transform.position.x - (bullet.transform.localScale.x / 2) > GameManager.LIMIT_RIGHT)
                    {
                        //right
                        bullet.transform.position = new Vector3(GameManager.LIMIT_LEFT - (bullet.transform.localScale.y / 2), bullet.transform.position.y, bullet.transform.position.z);
                    }
                    else if (bullet.transform.position.x + (bullet.transform.localScale.x / 2) < GameManager.LIMIT_LEFT)
                    {
                        //left
                        bullet.transform.position = new Vector3(GameManager.LIMIT_RIGHT + (bullet.transform.localScale.x / 2), bullet.transform.position.y, bullet.transform.position.z);
                    }
                    else
                    {
                        bullet.transform.position = bullet.transform.position;
                    }
                }
                else
                {
                    tabBullets.Remove(bullet);
                }
            }
        }   
    }


    private void CheckCollisionWithAsteroid()
    {
        if (tabBullets.Count > 0)
        {
            for (int i = 0; i < tabBullets.Count; i++)
            {
                if (AsteroidManager.Instance.tabAsteroid.Count > 0)
                {
                    for (int j = 0; j < AsteroidManager.Instance.tabAsteroid.Count; j++)
                    {
                        try
                        {
                            if (AsteroidManager.Instance.tabAsteroid[j] != null && tabBullets[i] != null)
                            {
                                Vector2 bulletToAsteroid = AsteroidManager.Instance.tabAsteroid[j].position - tabBullets[i].position;

                                if (bulletToAsteroid.magnitude <= (AsteroidManager.Instance.Radius * AsteroidManager.Instance.tabAsteroid[j].transform.localScale.x) + bulletRadius)
                                {
                                    GameObject.Destroy(tabBullets[i].gameObject);
                                    AsteroidCollisionEvent.Invoke(AsteroidManager.Instance.tabAsteroid[j]);

                                    tabBullets.Remove(tabBullets[i]);
                                    AsteroidManager.Instance.tabAsteroid.Remove(AsteroidManager.Instance.tabAsteroid[j]);

                                }
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            //Debug.Log(i + " " + j);
                        }   
                    } 
                }
            }
        }
    }  
    
    private void CleanList()
    {
        tabBullets.RemoveAll(item => item == null);
    }
}
