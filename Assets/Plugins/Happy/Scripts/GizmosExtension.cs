/*
 *
 *	Happy Framework
 *	by Toni Steyskal, 2016-2017
 * 
 */

using UnityEngine;

namespace Happy
{
	public static class GizmosExtension
	{
		public static void DrawLine (Vector3 from, Vector3 to, Color color)
		{
			Color oldColor = Gizmos.color;
			Gizmos.color = color;

			Gizmos.DrawLine (from, to);

			Gizmos.color = oldColor;
		}

		public static void DrawCircle (Vector3 center, float radius, Color color, int lineSegments = 8)
		{
			Color oldColor = Gizmos.color;
			Gizmos.color = color;

			lineSegments = Mathf.Max (8, lineSegments, Mathf.RoundToInt (radius) * 2);
			float angleIntervalRad = (2 * Mathf.PI) / lineSegments;

			float lastAngle = 0.0f;

			for (int i = 1; i <= lineSegments; i++)
			{
				float currentAngle = angleIntervalRad * i;

				Vector3 startPosition = new Vector3 (Mathf.Cos (lastAngle), Mathf.Sin (lastAngle)) * radius + center;
				Vector3 endPosition = new Vector3 (Mathf.Cos (currentAngle), Mathf.Sin (currentAngle)) * radius + center;

				Gizmos.DrawLine (startPosition, endPosition);

				lastAngle = currentAngle;
			}

			Gizmos.color = oldColor;
		}
	}
}
