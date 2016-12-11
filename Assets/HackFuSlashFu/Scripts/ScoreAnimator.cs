using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAnimator : MonoBehaviour
{
	private Animator _animator;

	void Awake ()
	{
		_animator = GetComponent<Animator> ();
	}

	public void UpdateScoreAnimation()
	{
		_animator.SetTrigger ("ScoreUpdate");
	}
}
