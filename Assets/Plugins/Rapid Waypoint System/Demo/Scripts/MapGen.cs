using UnityEngine;
using System.Collections;

public class MapGen : MonoBehaviour {

    public GameObject[,] tiles;
    public int sizeX, sizeY;

    private WaypointManager m_waypointManager;
    public GameObject startTile, endTile, pathTile, otherTile;

    // This is an example of how to use the dynamic navigation creation system with the waypoint manager
    void Start()
    {
        m_waypointManager = GetComponent<WaypointManager>();

        BuildTestMap();

        m_waypointManager.BuildNavigationMap(tiles, sizeX, sizeY);
    }

    void BuildTestMap()
    {
        tiles = new GameObject[sizeX, sizeY];

        for (int y = 0; y < sizeY; ++y)
        {
            for (int x = 0; x < sizeX; ++x)
            {
                if (x == 1 || x == sizeX - 2)
                {
                    if (y == 1)
                        tiles[x, y] = (GameObject)Instantiate(x == 1 ? startTile : endTile, new Vector3(x, 0, y), Quaternion.identity);
                    else if (y < sizeY - 1)
                        tiles[x, y] = (GameObject)Instantiate(pathTile, new Vector3(x, 0, y), Quaternion.identity);
                    else
                        tiles[x, y] = (GameObject)Instantiate(otherTile, new Vector3(x, 0, y), Quaternion.identity);
                }
                else if (x > 1 && x < sizeX - 2)
                {
                    if (y == sizeY - 2)
                        tiles[x, y] = (GameObject)Instantiate(pathTile, new Vector3(x, 0, y), Quaternion.identity);
                    else
                        tiles[x, y] = (GameObject)Instantiate(otherTile, new Vector3(x, 0, y), Quaternion.identity);
                }
                else
                    tiles[x, y] = (GameObject)Instantiate(otherTile, new Vector3(x, 0, y), Quaternion.identity);
            }
        }

        for (int y = 0; y < sizeY; ++y)
        {
            for (int x = 0; x < sizeX; ++x)
            {
                tiles[x, y].GetComponent<Tile>().posX = x;
                tiles[x, y].GetComponent<Tile>().posY = y;
                tiles[x, y].transform.parent = transform;
            }
        }
    }
}
