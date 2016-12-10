using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum TileType
    {
        WaypointStart,
        WaypointEnd,
        WaypointPath,
        Other
    }

    public TileType m_tileType;
    public int posX, posY;
}
