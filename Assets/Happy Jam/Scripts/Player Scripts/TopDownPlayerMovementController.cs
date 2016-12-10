using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent (typeof(Rigidbody2D))]
public class TopDownPlayerMovementController : MonoBehaviour
{
	[Header ("Input Axis Properties")]
	public string HorizontalAxisName = "Horizontal";
	public string VerticalAxisName = "Vertical";

	[Header ("Movement Properties")]
	public float MovementVelocity = 5.0f;

	[Header ("Read-Only")]
	[SerializeField]
	private Vector2 _movementDirection;

	private Rigidbody2D _rigidbody2D;

	void Awake ()
	{
		_rigidbody2D = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		_movementDirection = new Vector2 (Input.GetAxisRaw (HorizontalAxisName), Input.GetAxisRaw (VerticalAxisName));
		_rigidbody2D.velocity = _movementDirection * MovementVelocity;
	}

}
