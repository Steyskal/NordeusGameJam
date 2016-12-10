/*
 *
 *	Happy Framework
 *	by Toni Steyskal, 2016-2017
 * 
 */

using UnityEngine;

namespace Happy
{
	[DisallowMultipleComponent]
	public class TimedObjectDestructor : MonoBehaviour
	{
		[SerializeField]
		private float _timeOut = 1.0f;
		[SerializeField]
		private bool _detachChildren = false;

		private void Awake ()
		{
			Invoke ("DestroyNow", _timeOut);
		}

		private void DestroyNow ()
		{
			if (_detachChildren)
				transform.DetachChildren ();

			DestroyObject (gameObject);
		}
	}
}
