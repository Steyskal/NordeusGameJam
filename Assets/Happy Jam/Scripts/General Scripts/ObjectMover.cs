using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;

public class ObjectMover : MonoBehaviour
{
	[Header("Movement Properties")]
	public bool ShouldMove = true;
	public bool ShouldLoop = true;
	public float Speed = 3.0f;
	public float WaitTimeAtWaypoint = 1.0f;
	public float MaxDistanceDelta = Mathf.Epsilon;

	[Header("Object Properties")]
	public Rigidbody2D ObjectRigidbody2D;
	public List<Transform> Waypoints = new List<Transform>();

	[Header("Read-Only")]
	[SerializeField]
	private int _waypointIndex = 0;
	[SerializeField]
	private float _waitTimer = 0.0f;

	void FixedUpdate()
	{
		if(ShouldMove && (_waitTimer < Time.time))
			Move ();
	}

	private void Move()
	{
		if(Waypoints.Count != 0)
		{
			ObjectRigidbody2D.position = Vector2.MoveTowards (ObjectRigidbody2D.position, Waypoints [_waypointIndex].position, Speed * Time.fixedDeltaTime);

			if (Vector3.Distance (ObjectRigidbody2D.position, Waypoints [_waypointIndex].position) <= MaxDistanceDelta)
			{
				_waypointIndex++;
				_waitTimer = Time.time + WaitTimeAtWaypoint;
			}

			if (_waypointIndex >= Waypoints.Count)
			{
				if (ShouldLoop)
					_waypointIndex = 0;
				else
					ShouldMove = false;
			}
		}
		else
		{
			Debug.LogWarning("There are no Waypoint references set.");
		}
	}

	void OnDrawGizmos ()
	{
		GizmosExtension.DrawLine (ObjectRigidbody2D.position, Waypoints[_waypointIndex].position, Color.grey);
	}
}
