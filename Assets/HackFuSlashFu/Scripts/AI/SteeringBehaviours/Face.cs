using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Face : Allign
    {
        public bool Use2D = true;
        protected GameObject targetEmptyAgent;
        protected GameObject targetAux;
        protected bool _initialited = false;

        public override void Awake()
        {
            base.Awake();
        }
        protected virtual void OnEnable()
        {
            Setup();
        }
        public virtual void Setup()
        {
            if (!_initialited)
            {
                targetAux = target;

                target = new GameObject();
                if (Use2D)
                    target.AddComponent<Agent2D>();
                else
                    target.AddComponent<Agent>();

                if (targetEmptyAgent != null)
                {
                    Destroy(targetEmptyAgent);
                }
                targetEmptyAgent = target;
                _initialited = true;
            }
        }
        void OnDestroy()
        {
            if (target && target.tag != "Player")
                Destroy(target);
        }

        public override Steering GetSteering()
        {
            Vector3 direction = targetAux.transform.position - transform.position;

            Debug.DrawRay(this.transform.position + Vector3.up, direction, Color.gray, 2f);
            if (direction.magnitude > 0.0f)
            {
                float targetOrientation = 0;

                if (Use2D)
                    targetOrientation = Mathf.Atan2(direction.y, direction.x);
                else
                    targetOrientation = Mathf.Atan2(direction.x, direction.z);

                targetOrientation *= Mathf.Rad2Deg;
                target.GetComponent<Agent>().orientation = targetOrientation;
            }
            return base.GetSteering();
        }
    }
}
