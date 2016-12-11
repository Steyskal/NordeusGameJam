using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;
using System;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefab;
    public float SpawnDelay = 0.5f;
    public bool DestroyOnSpawn = true;
    public float DestroyDelay = 0.2f;

    void Start()
    {
        Invoke("Spawn", SpawnDelay);
    }

    void Spawn()
    {
        GameObject enemy = Instantiate(prefab);
        enemy.transform.position = this.transform.position;
        enemy.transform.rotation = this.transform.rotation;

        AI.Agent2D agent = enemy.GetComponent<AI.Agent2D>();
        agent.rotation = enemy.transform.rotation.eulerAngles.z;

        if (DestroyOnSpawn)
        {
            Invoke("Destroy", DestroyDelay);
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
