using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private Queue<GameObject> zombiePool = new Queue<GameObject>();
    [SerializeField]
    private int poolStartSize = 10;

    private void Start()
    {
        for (int i = 0; i < poolStartSize; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombiePool.Enqueue(zombie);
            zombie.SetActive(false);
        }
    }

    public GameObject GetZombie()
    {
        if (zombiePool.Count > 0)
        {
            GameObject zombie = zombiePool.Dequeue();
            zombie.SetActive(true);
            return zombie;
        }
        else
        {
            GameObject zombie = Instantiate(zombiePrefab);
            return zombie;
        }
    }

    public void ReturnZombie (GameObject zombie)
    {
        zombiePool.Enqueue(zombie);
        zombie.SetActive(false);
    }
}
