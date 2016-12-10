using UnityEngine;
using System.Collections;

[System.Serializable]
public class WaypointAgent : MonoBehaviour {
    
    [SerializeField] protected float minAgentSpeed = 10;
    [SerializeField] protected float maxAgentSpeed = 20;
    [SerializeField] protected WaypointManager m_waypointManager;

	[HideInInspector] protected int currentIndex = 0;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public Vector3 currentNodeTarget = Vector3.zero;
	[HideInInspector] public bool waypointUpdatingEntity = false;

	protected float speed = 10;
    protected Vector3 directionVector = new Vector3(0,1,0);
    protected float m_nodeProximityDistance = 0.1f;
    protected float m_slerpRotationSpeed = 0.1f;
    protected WaypointRotationMode m_waypointRotationMode;

    public WaypointManager WaypointSystem {  set { m_waypointManager = value; } }
    public WaypointRotationMode WaypointRotation { set { m_waypointRotationMode = value; } }
    public float SlerpSpeed { set { m_slerpRotationSpeed = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public float NodeProximityDistance { set { m_nodeProximityDistance = value; } }
    public int CurrentIndex { get { return currentIndex; } set { currentIndex = value; } }
    public Vector3 DirectionVector { get { return directionVector; } set { directionVector = value; } }

    public virtual void Start()
    {
        speed = Random.Range(minAgentSpeed, maxAgentSpeed);
    }

    public virtual void Update()
    {
        WaypointMovementUpdate();
    }

    protected IEnumerator DieAnimDelay()
    {
        yield return new WaitForSeconds(/*animations.GetClip("AI_Basic_Death").averageDuration + */2.5f);
        Destroy(gameObject);
    }

    protected IEnumerator DieWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

	public virtual void SwitchTarget(GameObject newTarget)
	{
		currentTarget = newTarget;
	}

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(currentNodeTarget, 0.15f);
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(transform.position, currentNodeTarget);
    }
    public Bounds GetBounds()
    {
        Bounds bounds = new Bounds();
        Collider col = GetComponent<Collider>();
        if (col != null)
        {   //Case 3D
            bounds = col.bounds;
        }
        else if (GetComponent<Collider2D>())
        {  // Case 2D
            bounds = GetComponent<Collider2D>().bounds;
        }
        else
        {
            Debug.LogError("Object Must Have Collider2D or Collider to calculate bounds");
        }
        return bounds;
    }

    protected void WaypointMovementUpdate()
    {
        // If the agent has a gameobject target assigned then move towards it otherwise 
        // get a target position in 3d sapce and move torawrds that
        if (currentTarget == null)
        {

            DirectionVector = (currentNodeTarget - transform.position).normalized;
            transform.Translate(DirectionVector * Time.deltaTime * Speed, Space.World);


            //This is used to give randomness to the movement
            Vector3 smudgeFactor = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

            //Rotating the object to face the target node
            //depending on the specified rotation mode.
            switch (m_waypointRotationMode)
            {
                case WaypointRotationMode.Snap:
                    transform.LookAt(currentNodeTarget + smudgeFactor);
                    break;

                case WaypointRotationMode.Slerp:
                    Quaternion targetRotation = Quaternion.LookRotation((currentNodeTarget + smudgeFactor) - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_slerpRotationSpeed);
                    break;

                default:
                    break;
            }

            if (m_waypointManager.ObjectIsOnNode(this))
            {

                currentIndex++;

                if (currentIndex >= m_waypointManager.NodeQuantity)
                {
                    if (!m_waypointManager.looping)
                    {
                        m_waypointManager.RemoveEntity(this);
                        Destroy(gameObject);
                        return;
                    }
                    else
                        currentIndex = 0;
                }

                // Get a position high enough that the agent wont clip the terrain
                // (NOTE: The pivot point must be in the center)
                Vector3 targetPosition = new Vector3(((Random.insideUnitSphere.x * 2) * m_nodeProximityDistance),
                                                        0 + (GetBounds().extents.magnitude) / 2 + 0.5f,
                                                        ((Random.insideUnitSphere.z * 2) * m_nodeProximityDistance));


                currentNodeTarget = targetPosition + m_waypointManager.GetNodePosition(currentIndex);
                
            }
        }
        else
        {

            DirectionVector = (currentTarget.transform.position - transform.position).normalized;

            transform.Translate(DirectionVector * Time.deltaTime * speed, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_slerpRotationSpeed);
        }
    }

}
