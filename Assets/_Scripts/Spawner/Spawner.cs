using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private float timeToSpawn = 5f;
    private float timeSinceSpawn;
    private Pooling objectPool;

    void Start()
    {
        objectPool = FindObjectOfType<Pooling>();
    }

    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if(timeSinceSpawn >= timeToSpawn)
        {
            GameObject newZombie = objectPool.GetZombie();
            newZombie.transform.position = this.transform.position;
            timeSinceSpawn = 0;
        }
    }
}
