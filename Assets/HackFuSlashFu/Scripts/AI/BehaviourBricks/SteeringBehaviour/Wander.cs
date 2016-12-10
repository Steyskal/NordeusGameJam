using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Wander")]
    [Help("Steering Behaviour Wander")]
    public class Wander : Face
    {
        [InParam("offset")]
        public float offset;
        [InParam("radius")]
        public float radius;
        [InParam("rate")]
        public float rate;

        private bool _initialized = false;
        private Transform _transform;

        public override void OnStart()
        {
            InitializeWander();
        }
        protected void InitializeWander()
        {
            if (!_initialized)
            {
                GameObject go = new GameObject();
                target = go.transform;
                _transform = gameObject.transform;
                target.transform.position = _transform.position;
                base.Initialize();
                agent = gameObject.GetComponent<IAgent>();
            }
            _initialized = true;
            Debug.Log("target" + target);
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }

        public override Steering GetSteering()
        {
            Steering steering = new Steering();
            float wanderOrientation = Random.Range(-1.0f, 1.0f) * rate;
            float targetOrientation = wanderOrientation + agent.GetOrientation();
            Vector3 orientationVec = AgentBehaviour.GetOriAsVec(agent.GetOrientation());
            Vector3 targetPosition = (offset * orientationVec) + _transform.position;
            targetPosition = targetPosition + (AgentBehaviour.GetOriAsVec(targetOrientation) * radius);
            targetAux.transform.position = targetPosition;
            steering = base.GetSteering();
            steering.linear = targetAux.transform.position - _transform.position;
            steering.linear.Normalize();
            steering.linear *= agent.GetMaxAccel();
            return steering;
        }
    }
}
