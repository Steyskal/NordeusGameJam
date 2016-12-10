using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Waypoint Follow")]
    [Help("Requires Rapid Waypoint System")]
    public class WaypointFollow : BaseSteeringBehaviour
    {
        [InParam("waypointManager")]
        [Help("Waypoint Manager")]
        public WaypointManager waypointManager;

        [InParam("Precision", DefaultValue =0.1f)]
        protected float precision = 0.1f;

        protected int currentIndex = 0;
        public GameObject currentTarget;
        public Vector3 currentNodeTarget = Vector3.zero;

        protected Vector3 directionVector = new Vector3(0, 1, 0);
        public Vector3 DirectionVector { get { return directionVector; } set { directionVector = value; } }
        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }
        public Bounds GetBounds()
        {
            Bounds bounds = new Bounds();
            Collider col = gameObject.GetComponent<Collider>();
            if (col != null)
            {   //Case 3D
                bounds = col.bounds;
            }
            else if (gameObject.GetComponent<Collider2D>())
            {  // Case 2D
                bounds = gameObject.GetComponent<Collider2D>().bounds;
            }
            else
            {
                Debug.LogError("Object Must Have Collider2D or Collider to calculate bounds");
            }
            return bounds;
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            if (currentTarget == null)
            {

                DirectionVector = (currentNodeTarget - gameObject.transform.position).normalized;
                steering.linear = DirectionVector;
                // No rotations for now

                if (waypointManager.ObjectIsOnNode(gameObject.transform, currentNodeTarget))
                {
                    currentIndex++;

                    if (currentIndex >= waypointManager.NodeQuantity)
                    {
                        if (!waypointManager.looping)
                        {
                            //waypointManager.RemoveEntity(this);
                            MonoBehaviour.Destroy(gameObject);
                            return null;
                        }
                        else
                            currentIndex = 0;
                    }

                    // Get a position high enough that the agent wont clip the terrain
                    // (NOTE: The pivot point must be in the center)
                    Vector3 targetPosition = new Vector3(((Random.insideUnitSphere.x * 2) * precision),
                                                            0 + (GetBounds().extents.magnitude) / 2 + 0.5f,
                                                            ((Random.insideUnitSphere.z * 2) * precision));


                    currentNodeTarget = targetPosition + waypointManager.GetNodePosition(currentIndex);
                }
            }
            else
            {
                DirectionVector = (currentTarget.transform.position - gameObject.transform.position).normalized;
                steering.linear = DirectionVector;
            }
            return steering;


        }
    }
}
