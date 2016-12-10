using UnityEngine;
using System.Collections;

public class WaypointNode : MonoBehaviour {

    //void OnDestroy()

    public void OnDestroy()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            int n = FindObjectOfType<WaypointManager>().waypointNodes.FindIndex(x => x.transform.GetComponent<WaypointNode>().transform == transform);
            if (n < FindObjectOfType<WaypointManager>().waypointNodes.Count && n >= 0)
                FindObjectOfType<WaypointManager>().waypointNodes.RemoveAt(n);
        }
    }

}
