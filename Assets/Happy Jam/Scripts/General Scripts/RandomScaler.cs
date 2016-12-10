using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class RandomScaler : MonoBehaviour
{
	[Header("Relative Scale Values")]
	public float MinScale = 0.75f;
	public float MaxScale = 1.25f;

	void Awake ()
	{
		RandomScale ();
	}

	private void RandomScale ()
	{
		float newScale = Random.Range (MinScale, MaxScale);
		transform.localScale = new Vector3 (transform.localScale.x * newScale, transform.localScale.y * newScale, 1.0f);
	}
}
