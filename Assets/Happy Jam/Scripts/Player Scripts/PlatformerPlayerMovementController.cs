using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;

[DisallowMultipleComponent]
[RequireComponent (typeof(Rigidbody2D))]
public class PlatformerPlayerMovementController : MonoBehaviour
{
	[Header ("Input Axis Properties")]
	public string HorizontalAxisName = "Horizontal";
	public string JumpAxisName = "Jump";

	[Header ("Movement Properties")]
	public float MovementVelocity = 5.0f;
	public float JumpForce = 600.0f;
	public Transform GroundCheckTransform;
	public LayerMask GroundLayerMask;
	public bool CanInterruptJump = true;

	[Header ("Read-Only")]
	[SerializeField]
	private float _moveH = 0.0f;
	[SerializeField]
	private float _moveV = 0.0f;
	[SerializeField]
	private bool _isGrounded = false;
	[SerializeField]
	private bool _isFacingRight = false;

	private Transform _transform;
	private Rigidbody2D _rigidbody2D;

	void Awake ()
	{
		_transform = transform;
		_rigidbody2D = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		_moveH = Input.GetAxisRaw (HorizontalAxisName);
		_moveV = _rigidbody2D.velocity.y;

		_isGrounded = Physics2D.Linecast (_rigidbody2D.position, GroundCheckTransform.position, GroundLayerMask);

		if (Input.GetButtonDown ("Jump") && _isGrounded)
		{
			_moveV = 0.0f;
			_rigidbody2D.AddForce (new Vector2 (0.0f, JumpForce));
		}

		if (Input.GetButtonUp ("Jump") && (_moveV > 0.0f) && CanInterruptJump)
			_moveV = 0.0f;
	
		_rigidbody2D.velocity = new Vector2 (_moveH * MovementVelocity, _moveV);
	}

	void LateUpdate ()
	{
		Vector3 localScale = _transform.localScale;

		if (_moveH > 0.0f)
			_isFacingRight = true;
		else if (_moveH < 0.0f)
				_isFacingRight = false;

		if ((_isFacingRight && (localScale.x < 0.0f)) || (!_isFacingRight && (localScale.x > 0.0f)))
			localScale.x *= -1.0f;

		_transform.localScale = localScale;
	}

	void OnDrawGizmos ()
	{
		GizmosExtension.DrawLine (transform.position, GroundCheckTransform.position, Color.yellow);
	}
}
