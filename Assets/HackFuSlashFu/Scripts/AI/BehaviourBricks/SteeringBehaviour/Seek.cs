using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Seek")]
    [Help("Steering Behaviour Seek that requires Agent component")]
    public class Seek : BaseSteeringBehaviour
    {
        [InParam("target")]
        [Help("Target position from which agent will flee")]
        public Transform target;

        protected Transform _target;
        protected Transform _transform;

        public override void OnStart()
        {
            base.OnStart();
            _target = target;
            _transform = gameObject.transform;
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            if (agent is Agent2D)
            {
                steering.linear = (Vector2)_target.position- (Vector2)_transform.position;
            }
            else
            {
                steering.linear = _target.position -  _transform.position;
            }
            steering.linear.Normalize();
            steering.linear = steering.linear * agent.GetMaxAccel();
            return steering;
        }
    }
}
