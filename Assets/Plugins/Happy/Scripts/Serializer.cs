/*
 *
 *	Happy Framework
 *	by Toni Steyskal, 2016-2017
 * 
 */

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Happy
{
	public static class Serializer
	{
		public static void Save<T> (T data) where T : class
		{
			string path = Application.persistentDataPath + "/" + typeof(T).Name + ".dat";

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (path);

			bf.Serialize (file, data);
			file.Close ();

			Debug.Log ("Data saved: " + path);
		}

		public static T Load<T> () where T : class
		{
			string path = Application.persistentDataPath + "/" + typeof(T).Name + ".dat";

			if (File.Exists (path))
			{
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (path, FileMode.Open);

				T data = (T)bf.Deserialize (file);
				file.Close ();

				Debug.Log ("Data loaded: " + path);

				return data;
			}

			Debug.LogWarning ("No data file found at: " + path);
			return null;
		}

		public static void Delete<T> () where T :class
		{
			string path = Application.persistentDataPath + "/" + typeof(T).Name + ".dat";

			if (File.Exists (path))
			{
				File.Delete (path);
				Debug.Log ("Data deleted: " + path);
			}
			else
			{
				Debug.LogWarning ("No data file found at: " + path);
			}
		}
	}
}
