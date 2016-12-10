using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Face")]
    [Help("Steering Behaviour Face that alligns forever that requires Agent component")]
    public class Face : Allign
    {

        [InParam("Use 2D")]
        public bool Use2D = false;
        protected GameObject targetAux;
        private bool _initialized = false;
        private Transform _transform;

        public override void OnStart()
        {
            base.OnStart();
            Initialize();

        }
        protected void Initialize()
        {
            if (!_initialized)
            {
                targetAux = target.gameObject;
                GameObject go = new GameObject();
                target = go.transform;
                go.AddComponent<Agent>();
                _transform = gameObject.transform;
            }
            _initialized = true;
        }

        public override TaskStatus OnUpdate()
        {
            SetSteering();
            return TaskStatus.RUNNING;
        }

        public override Steering GetSteering()
        {
            Vector3 direction = targetAux.transform.position - _transform.position;

            Debug.DrawRay(_transform.position + Vector3.up, direction, Color.gray, 2f);
            if (direction.magnitude > 0.0f)
            {
                float targetOrientation = 0;
                if (Use2D)
                    Mathf.Atan2(direction.y, direction.x);
                else
                    Mathf.Atan2(direction.x, direction.z);
                targetOrientation *= Mathf.Rad2Deg;
                target.GetComponent<Agent>().orientation = targetOrientation;
            }
            return base.GetSteering();
        }
    }
}
