using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Flee")]
    [Help("Steering Behaviour Flee that requires Agent component")]
    public class Flee : BaseSteeringBehaviour
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
                Debug.Log("2DAgent");
                steering.linear = (Vector2)_transform.position - (Vector2)_target.position;
            }
            else
            {
                steering.linear = _transform.position - _target.position;
            }
            steering.linear.Normalize();
            steering.linear = steering.linear * agent.GetMaxAccel();
            return steering;
        }
    }
}
