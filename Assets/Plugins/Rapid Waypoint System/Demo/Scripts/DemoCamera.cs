using UnityEngine;
using System.Collections;

public class DemoCamera : MonoBehaviour {

    public float rotateSpeed = 1.0f;
    public GameObject rotatePivot;

    void Start()
    {
        transform.SetParent(rotatePivot.transform);
    }

    void Update()
    {
        rotatePivot.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
