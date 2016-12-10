/*
 *
 *	Happy Framework
 *	by Toni Steyskal, 2016-2017
 * 
 */

using UnityEngine;
using UnityEngine.Events;

namespace Happy
{
	public class HappyMonoBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Invoke the method in time seconds.
		/// </summary>
		public void Invoke (UnityAction action, float time)
		{
			Invoke (action.Method.Name, time);
		}

		/// <summary>
		/// Invokes the method in time seconds, then repeatedly every repeatRate seconds.
		/// </summary>
		public void InvokeRepeating (UnityAction action, float time, float repeatRate)
		{
			InvokeRepeating (action.Method.Name, time, repeatRate);
		}

		/// <summary>
		/// Cancels all invoke calls with the given method on this behaviour.
		/// </summary>
		public void CancelInvoke (UnityAction action)
		{
			CancelInvoke (action.Method.Name);
		}
	}
}
