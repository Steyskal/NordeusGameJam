using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

[DisallowMultipleComponent]
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerMovementController : Agent2D
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

	protected override void Start ()
	{
	}

	public override void Update ()
	{
		RotateToMouse ();
		Move ();

		// Aleric needs this.
		velocity = _rigidbody2D.velocity;
	}

	private void RotateToMouse()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 direction = mousePosition - transform.position;
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		_rigidbody2D.rotation = angle;
	}

	private void Move()
	{
		_movementDirection = new Vector2 (Input.GetAxisRaw (HorizontalAxisName), Input.GetAxisRaw (VerticalAxisName));
		_rigidbody2D.velocity = _movementDirection * MovementVelocity;
	}

	public override void LateUpdate ()
	{
		
	}
}
