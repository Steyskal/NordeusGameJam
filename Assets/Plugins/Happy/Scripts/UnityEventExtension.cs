/*
 *
 *	Happy Framework
 *	by Toni Steyskal, 2016-2017
 * 
 */

using UnityEngine.Events;
using System;

namespace Happy
{
	[Serializable]
	public class CustomUnityEvent : UnityEvent {}

	[Serializable]
	public class CustomUnityEvent<T0> : UnityEvent<T0> {}

	[Serializable]
	public class CustomUnityEvent<T0, T1> : UnityEvent<T0, T1> {}

	[Serializable]
	public class CustomUnityEvent<T0, T1, T2> : UnityEvent<T0, T1, T2> {}

	[Serializable]
	public class CustomUnityEvent<T0, T1, T2, T3> : UnityEvent<T0, T1, T2, T3> {}
}
