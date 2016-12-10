using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Seek : AgentBehaviour
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            if (agent is Agent2D)
            {
                steering.linear = (Vector2)target.transform.position- (Vector2)transform.position;
            }
            else
            {
                steering.linear = target.transform.position - transform.position;
            }
            steering.linear.Normalize();
            steering.linear = steering.linear * agent.GetMaxAccel();
            return steering;
        }
    }
}
