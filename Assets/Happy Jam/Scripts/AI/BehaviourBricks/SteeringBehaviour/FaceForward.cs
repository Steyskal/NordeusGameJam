using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Face Forward")]
    [Help("Steering Behaviour Face Forward")]
    public class FaceForward : Allign
    {
        [InParam("Use 2D")]
        public bool Use2D = false;

        private Agent _targetAgent;
        public override void OnStart()
        {
            base.OnStart();
            if (_targetAgent == null)
            {
                GameObject go = new GameObject();
                target = go.transform;
                _targetAgent = target.gameObject.AddComponent<Agent>();
            }
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }

        public override Steering GetSteering()
        {
            Vector3 velocity = agent.GetVelocity();

            if (velocity.magnitude <= 0.0001f)
                return new Steering();

            if (Use2D)
                _targetAgent.orientation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            else
                _targetAgent.orientation = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
            //    Debug.DrawRay(this.transform.position + Vector3.up, velocity, Color.green, 1f);
            return base.GetSteering();
        }
    }
}
