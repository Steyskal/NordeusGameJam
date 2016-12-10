using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Allign : AgentBehaviour
    {
        public float targetRadius;
        public float slowRadius;
        public float timeToTarget = 0.1f;

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            float targetOrientation = target.GetComponent<Agent>().orientation;
            float rotation = targetOrientation - agent.GetOrientation();
            rotation = MapToRange(rotation);
            float rotationSize = Mathf.Abs(rotation);
            if (rotationSize < targetRadius)
                return steering;
            float targetRotation;
            if (rotationSize > slowRadius)
                targetRotation = agent.GetMaxRotation();
            else
                targetRotation = agent.GetMaxRotation() * rotationSize / slowRadius;
            targetRotation *= rotation / rotationSize;
            steering.angular = targetRotation - agent.GetRotation();
            steering.angular /= timeToTarget;
            float angularAccel = Mathf.Abs(steering.angular);
            if (angularAccel > agent.GetMaxAngularAccel())
            {
                steering.angular /= angularAccel;
                steering.angular *= agent.GetMaxAngularAccel();
            }
            return steering;
        }
    }
}
