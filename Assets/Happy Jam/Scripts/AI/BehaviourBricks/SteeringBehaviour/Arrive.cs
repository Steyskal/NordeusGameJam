using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Arrive")]
    [Help("Steering Behaviour Arrive that requires Agent component")]
    public class Arrive : BaseSteeringBehaviour
    {
        [InParam("target")]
        [Help("Target position where the game object will be moved")]
        public Transform target;

        [InParam("targetRadius")]
        public float targetRadius = 1;
        [InParam("slowRadius")]
        public float slowRadius = 4;
        public float timeToTarget = 0.1f;

        private bool isClose = false;
        private Transform _transform;

        public override void OnStart()
        {
            base.OnStart();
            _transform = gameObject.transform;
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            /*
            if (isClose)
                return TaskStatus.COMPLETED;*/

            return TaskStatus.RUNNING;
        }

        public override void OnAbort()
        {
        }

        public override Steering GetSteering()
        {
            isClose = false;
            Steering steering = new Steering();
            Vector3 direction = target.transform.position - _transform.position;
            float distance = direction.magnitude;
            float targetSpeed;
            if (distance < targetRadius)
            {
                isClose = true;
                return steering;
            }
            if (distance > slowRadius)
                targetSpeed = agent.GetMaxSpeed();
            else
                targetSpeed = agent.GetMaxSpeed() * distance / slowRadius;

            Vector3 desiredVelocity = direction;
            desiredVelocity.Normalize();
            desiredVelocity *= targetSpeed;
            steering.linear = desiredVelocity - agent.GetVelocity();
            steering.linear /= timeToTarget;
            if (steering.linear.magnitude > agent.GetMaxAccel())
            {
                steering.linear.Normalize();
                steering.linear *= agent.GetMaxAccel();
            }
            return steering;
        }
    }
}
