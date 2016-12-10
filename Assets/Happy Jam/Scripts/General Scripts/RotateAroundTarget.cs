using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;

[DisallowMultipleComponent]
public class RotateAroundTarget : MonoBehaviour
{
	public Transform Target;
	public Vector3 RotationAxis = Vector3.forward;
	public float RotationAngle = 20.0f;

	private Transform _transform;

	void Awake ()
	{
		_transform = transform;

		if (!Target)
		{
			Target = _transform;
			Debug.LogWarning ("Target reference not set, defaulting to object transform.");
		}
	}

	void Update ()
	{
		_transform.RotateAround (Target.position, RotationAxis, RotationAngle * Time.deltaTime);
	}

	void OnDrawGizmos()
	{
		GizmosExtension.DrawCircle (Target.position, Vector3.Distance (Target.position, transform.position), Color.gray);
	}
}
