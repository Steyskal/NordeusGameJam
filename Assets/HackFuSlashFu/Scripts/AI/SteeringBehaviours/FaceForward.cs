using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class FaceForward : Allign
    {
        public bool UseAgent2D = false;
        private Agent _targetAgent;
        public override void Awake()
        {
            base.Awake();
        }
        void OnEnable()
        {
            target = new GameObject();
            _targetAgent = target.AddComponent<Agent>();
        }
        void OnDestroy()
        {
            Destroy(target);
        }

        public override Steering GetSteering()
        {
            Vector3 velocity = agent.GetVelocity();

            if (velocity.magnitude <= 0.0001f)
                return new Steering();

            if (UseAgent2D)
                _targetAgent.orientation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            else
                _targetAgent.orientation = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            //    Debug.DrawRay(this.transform.position + Vector3.up, velocity, Color.green, 1f);
            return base.GetSteering();
        }
    }
}
