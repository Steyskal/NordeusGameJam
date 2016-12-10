using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Happy
{
	[DisallowMultipleComponent]
	public class DontDestroyOnLoad : MonoBehaviour
	{
		void Awake ()
		{
			DontDestroyOnLoad (gameObject);
		}
	}
}
