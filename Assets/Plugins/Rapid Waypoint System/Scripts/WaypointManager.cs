using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum WaypointRotationMode
{
    None, Snap, Slerp
};

[System.Serializable]
public class Node
{

    public Node() { }
    public Node(Transform transform_)
    {
        transform = transform_;
    }

    public Transform transform;
}

[System.Serializable]
public class WaypointManager : MonoBehaviour
{

    [Range(10, 60)]
    public int updateIntervalPerSecond = 20;
    [HideInInspector]
    [SerializeField]
    public List<Node> waypointNodes = new List<Node>();
    [SerializeField]
    private List<WaypointAgent> objectToMove = new List<WaypointAgent>();

    public float nodeProximityDistance = 0.1f;
    public WaypointRotationMode rotationMode;
    public float slerpRotationSpeed = 1.0f;

    public bool looping = false;

    private Transform m_spawnPoint;
    private bool shouldClean = false;
    private bool inRoutine = false;
    private float intervalRate;
    private float timeSinceLastUpdate = 0.0f;

    public int AgentQuantity { get { return objectToMove.Count; } }
    public int NodeQuantity { get { return waypointNodes.Count; } }
    public Transform SpawnPoint { get { return m_spawnPoint; } }
    //--------------------Unity Functions--------------------


    void Start()
    {
        intervalRate = 1 / updateIntervalPerSecond;
    }

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        objectToMove.TrimExcess();
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= intervalRate)
            UpdateSystem();
    }

    void OnDrawGizmos()
    {
        //Drawing the waypoint nodes and the paths
        //between them as red spheres and lines.
        if (waypointNodes.Count > 0)
        {
            Vector3 previousNode = waypointNodes[0].transform.position;
            foreach (var node in waypointNodes)
            {
                if (node.transform)
                {
                    Gizmos.DrawWireSphere(node.transform.position, nodeProximityDistance);
                    Gizmos.DrawLine(previousNode, node.transform.position);
                    previousNode = node.transform.position;
                }
            }
            if (looping)
                Gizmos.DrawLine(previousNode, waypointNodes[0].transform.position);
        }

    }

    public void AddEntity(GameObject entity)
    {
        objectToMove.Add(SpawnEntity(entity).GetComponent<WaypointAgent>());
    }

    public void AddEntity(GameObject entity, int index)
    {
        objectToMove.Insert(index, SpawnEntity(entity).GetComponent<WaypointAgent>());
    }

    public void EnableEntityAtIndex(int index)
    {
        objectToMove[index].gameObject.SetActive(true);
    }

    public void EnableInactiveEntity()
    {
        foreach (WaypointAgent entity in objectToMove)
        {
            if (!entity.gameObject.activeInHierarchy)
            {
                entity.gameObject.SetActive(true);
                return;
            }
        }
    }

    // Call this function when the interval between updates in the waypoint system should be updated
    public void UpdateIntervalTime()
    {
        intervalRate = 1 / updateIntervalPerSecond;
    }

    private void UpdateSystem()
    {
        for (int i = 0; i < objectToMove.Count; ++i)
        {
            if (objectToMove[i] == null)
            {
                objectToMove.TrimExcess();
                continue;
            }

            if (!objectToMove[i].gameObject.activeInHierarchy)
                continue;

            WaypointAgent aiObj = objectToMove[i];

            //Exiting if the target node is 
            //outside of the waypointNodes list.
            if (aiObj.CurrentIndex >= waypointNodes.Count)
            {
                if (!looping)
                {
                    RemoveEntity(aiObj);
                    Destroy(aiObj.gameObject);
                    objectToMove.TrimExcess();
                }
                else
                    aiObj.CurrentIndex = 0;

                continue;
            }
        }

        // Trim the list of unnecessary entries
        if (shouldClean && !inRoutine)
            StartCoroutine(CleanList());

        // Reset the update timer
        timeSinceLastUpdate = 0.0f;

    }

    public Vector3 GetNodePosition(int nodeIndex)
    {
        return waypointNodes[nodeIndex].transform == null ? waypointNodes[nodeIndex + 1].transform.position : waypointNodes[nodeIndex].transform.position;
    }

    public bool ObjectIsOnNode(WaypointAgent obj)
    {
        //Checking if the distance from the object to the target node is less than the proximity distance.
        return (Vector3.Distance(obj.transform.position, obj.currentNodeTarget) < nodeProximityDistance);
    }

    // For Behaviour
    public bool ObjectIsOnNode(Transform obj, Vector3 currentNodeTarget)
    {
        return (Vector3.Distance(obj.transform.position, currentNodeTarget) < nodeProximityDistance);
    }

    // Remove an entity from the list
    public void RemoveEntity(WaypointAgent entity)
    {
        if (objectToMove.Contains(entity))
            objectToMove.Remove(entity);

        shouldClean = true;
        objectToMove.TrimExcess();
    }

    public void ResetSystem()
    {
        objectToMove.TrimExcess();

        foreach (var item in objectToMove)
            Destroy(item.gameObject);

        objectToMove.Clear();
    }

    //--------------------Private Functions--------------------

    // Spawns the entity within the system....doesn't spawn it with the world. It needs to already exist
    private GameObject SpawnEntity(GameObject objectToSpawn)
    {
        objectToSpawn.transform.parent = transform;
        WaypointAgent agent = objectToSpawn.GetComponent<WaypointAgent>();
        Bounds bounds = agent.GetBounds();
        Vector3 targetPosition = new Vector3(((Random.insideUnitSphere.x * 2) * nodeProximityDistance),
                                                        0 + (bounds.extents.magnitude) / 2,
                                                        ((Random.insideUnitSphere.z * 2) * nodeProximityDistance));
        agent.currentNodeTarget = targetPosition + waypointNodes[agent.CurrentIndex].transform.position;
        agent.WaypointSystem = this;
        agent.WaypointRotation = rotationMode;
        agent.NodeProximityDistance = nodeProximityDistance;
        agent.SlerpSpeed = slerpRotationSpeed;

        return objectToSpawn;
    }

    // Trim the list of unnecessary entries
    IEnumerator CleanList()
    {
        inRoutine = true;
        yield return new WaitForSeconds(2.5f);
        objectToMove.TrimExcess();
        inRoutine = false;
        shouldClean = false;
    }

    private void AddNode(Transform newNode, int index)
    {
        waypointNodes.Insert(index, new Node(newNode));
    }

    private void AddNode(Transform newNode)
    {
        waypointNodes.Add(new Node(newNode));
    }


    private bool ObjectIsOnNode(int nodeIndex, int index)
    {
        //Checking if the distance from the object to the target node is less than the proximity distance.
        return (Vector3.Distance(objectToMove[index].transform.position, objectToMove[index].currentNodeTarget) < nodeProximityDistance);
    }

    private void ClearNodes()
    {
        foreach (var node in waypointNodes)
            Destroy(node.transform.gameObject);

        waypointNodes.Clear();
    }

    //Dynamic Node Creation Below This Point
    #region // Dynamic Node Creation

    private struct Map
    {
        public GameObject[,] map;
        public int sizeX;
        public int sizeY;
    }

    public void Addspawn(Transform point)
    {
        m_spawnPoint = point;
    }

    public bool BuildNavigationMap(GameObject[,] tiles, int sizeX_, int sizeY_)
    {
        if (waypointNodes.Count > 0)
            ClearNodes();

        Tile[] tiles_ = FindObjectsOfType<Tile>();
        List<Tile> listTile = tiles_.ToList();

        Tile startTile = listTile.Find(x => x.m_tileType == Tile.TileType.WaypointStart);

        if (startTile == null)
            return false;

        GameObject startNode = startTile.gameObject;

        Vector3 newPos = new Vector3(startNode.transform.position.x, startNode.transform.position.y + 1, startNode.transform.position.z);
        GameObject spawnNode = new GameObject("WaypointSystem_SpawnLocation");
        spawnNode.transform.parent = transform;
        spawnNode.transform.position = newPos;
        spawnNode.transform.Rotate(0, 0, 90);
        Addspawn(spawnNode.transform);

        Map map_ = new Map() { map = tiles, sizeX = sizeX_, sizeY = sizeY_ };

        AddNode((map_.map[startNode.GetComponent<Tile>().posX, startNode.GetComponent<Tile>().posY]).transform);
        return FindNextPoint(map_, startNode, startNode);
    }

    private bool FindNextPoint(Map map_, GameObject currentPoint, GameObject previousNode)
    {
        Tile path_ = currentPoint.GetComponent<Tile>();
        if (currentPoint.GetComponent<Tile>().m_tileType == Tile.TileType.WaypointEnd)
        {
            AddNode((map_.map[currentPoint.GetComponent<Tile>().posX, currentPoint.GetComponent<Tile>().posY]).transform);
            return true;
        }


        if (path_.posX + 1 < map_.sizeX && previousNode != map_.map[path_.posX + 1, path_.posY].gameObject && IsWaypointPath(map_.map[path_.posX + 1, path_.posY]))
        {
            AddNode((map_.map[path_.posX + 1, path_.posY]).transform);
            FindNextPoint(map_, (map_.map[path_.posX + 1, path_.posY]), currentPoint);
            return true;
        }

        else if (path_.posX - 1 >= 0 && previousNode != map_.map[path_.posX - 1, path_.posY].gameObject && IsWaypointPath(map_.map[path_.posX - 1, path_.posY]))
        {
            AddNode((map_.map[path_.posX - 1, path_.posY]).transform);
            FindNextPoint(map_, (map_.map[path_.posX - 1, path_.posY]), currentPoint);
            return true;

        }

        else if (path_.posY + 1 < map_.sizeY && previousNode != map_.map[path_.posX, path_.posY + 1].gameObject && IsWaypointPath(map_.map[path_.posX, path_.posY + 1]))
        {
            AddNode((map_.map[path_.posX, path_.posY + 1]).transform);
            FindNextPoint(map_, (map_.map[path_.posX, path_.posY + 1]), currentPoint);
            return true;
        }

        else if (path_.posY - 1 >= 0 && previousNode != map_.map[path_.posX, path_.posY - 1].gameObject && IsWaypointPath(map_.map[path_.posX, path_.posY - 1]))
        {
            AddNode((map_.map[path_.posX, path_.posY - 1]).transform);
            FindNextPoint(map_, (map_.map[path_.posX, path_.posY - 1]), currentPoint);
            return true;
        }
        return false;
    }

    private bool IsWaypointPath(GameObject tileToCheck)
    {
        return tileToCheck.GetComponent<Tile>().m_tileType == Tile.TileType.WaypointPath ? true : false;
    }

    #endregion 
}


