using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{
    [Action("Steering Behaviour/Evade")]
    [Help("Steering Behaviour Evade that requires Agent component")]
    public class Evade : Flee
    {
        [InParam("Max Prediction")]
        public float maxPrediction;

        private Transform targetAux;
        private IAgent targetAgent;
        //private bool isFar = false;
        private bool _initialized = false;

        public override void OnStart()
        {
            base.OnStart();
            if (!_initialized && target != null)
            {
                _target = target;
                targetAgent = _target.GetComponent<Agent>();
                if (targetAgent == null)
                {
                    Debug.LogError("Target is not Agent");
                }
                targetAux = _target;
                GameObject go = new GameObject();
                _target = go.transform;
                _initialized = true;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_target != null)
                SetSteering();
            return TaskStatus.RUNNING;
        }
        public override void OnEnd()
        {
            base.OnEnd();
            End();
        }
        void End()
        {
            MonoBehaviour.Destroy(_target.gameObject);
            targetAux = null; _initialized = false;
        }
        public override void OnAbort()
        {
            base.OnAbort();
            End();
        }

        public override Steering GetSteering()
        {
            //isFar = false;
            Vector3 direction = targetAux.transform.position - _transform.position;
            float distance = direction.magnitude;
            float speed = agent.GetVelocity().magnitude;
            float prediction;
            /*if (distance >= maxPrediction)
            {
                isFar = true;
                Debug.Log("Far");
            }*/

            if (speed <= distance / maxPrediction)
            {
                //Closest
                prediction = maxPrediction;
            }
            else
            {
                prediction = distance / speed;
            }
            _target.transform.position = targetAux.transform.position;
            _target.transform.position += targetAgent.GetVelocity() * prediction;
            return base.GetSteering();
        }
    }
}
