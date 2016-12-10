using System;
using System.Collections;
using System.Collections.Generic;
using UI.MultiListView;
using UnityEngine;
using UnityEngine.UI;

public class FootprintsExampleCharacter : MonoBehaviour, IFootprintsCharacter
{
    private Vector3 lastDirection;
    void Update()
    {
        lastDirection = Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime + Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += lastDirection;
    }
    public Vector3 GetDirection()
    {
        return lastDirection;
    }
}
