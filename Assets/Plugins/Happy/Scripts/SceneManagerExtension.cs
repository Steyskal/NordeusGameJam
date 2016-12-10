/*
 *
 *	Happy Framework
 *	by Toni Steyskal, 2016-2017
 * 
 */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Happy
{
	public static class SceneManagerExtension
	{
		public static void ReloadScene()
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}

		public static void LoadNextScene()
		{
			int nextSceneIndex = SceneManager.GetActiveScene ().buildIndex + 1;

			if(nextSceneIndex < SceneManager.sceneCountInBuildSettings)
				SceneManager.LoadScene (nextSceneIndex);
			else
				Debug.LogError("Scene with buildIndex " + nextSceneIndex + " does not exist!");
		}
	}
}
