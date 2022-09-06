using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieReturn : MonoBehaviour
{
    private Pooling objectPool;

    private void Start()
    {
        objectPool = FindObjectOfType<Pooling>();
    }

    private void OnDisable()
    {
        if (objectPool != null)
            objectPool.ReturnZombie(this.gameObject);
    }
}
