using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Align")]
    [Help("Steering Behaviour Alling that alligns forever that requires Agent component")]
    public class Allign : BaseSteeringBehaviour
    {
        [InParam("target")]
        [Help("Target position where the game object will be moved")]
        public Transform target;


        [InParam("targetRadius")]
        public float targetRadius;
        [InParam("slowRadius")]
        public float slowRadius;
        [InParam("timeToTarget", DefaultValue = 0.1f)]
        public float timeToTarget = 0.1f;

        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            float targetOrientation = target.GetComponent<IAgent>().GetOrientation();
            float rotation = targetOrientation - agent.GetOrientation();
            rotation = AgentBehaviour.MapToRange(rotation);
            float rotationSize = Mathf.Abs(rotation);
            if (rotationSize < targetRadius)
            {
                return steering;
            }
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
