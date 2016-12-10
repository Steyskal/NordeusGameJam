using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;
using System;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        GameObject enemy = Instantiate(prefab);
    }
}
