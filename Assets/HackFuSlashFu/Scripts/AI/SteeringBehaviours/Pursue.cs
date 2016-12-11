using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace AI
{
    public class Pursue : Seek
    {
        public float maxPrediction;
        private GameObject targetAux;
        private Agent targetAgent;

        public override void Awake()
        {
            base.Awake();
            Setup();
        }
        void OnEnable()
        {
            if (targetAux == null) Setup();
        }
        public void Setup()
        {
            if (target != null)
            {
                targetAgent = target.GetComponent<Agent>();
                targetAux = target;
                target = new GameObject();
            }
        }
        public void Setup(GameObject target)
        {
            targetAgent = target.GetComponent<Agent>();
            targetAux = target;
            this.target = new GameObject();
        }

        void OnDestroy()
        {
            Destroy(this.target);
        }
        public override Steering GetSteering()
        {
			if(targetAux)
			{
				Vector3 direction = targetAux.transform.position - transform.position;
				float distance = direction.magnitude;
				float speed = agent.GetVelocity().magnitude;
				float prediction;
				if (speed <= distance / maxPrediction)
					prediction = maxPrediction;
				else
					prediction = distance / speed;
				target.transform.position = targetAux.transform.position;
				target.transform.position += targetAgent.velocity * prediction;
			
			}

			return base.GetSteering();
        }
    }
}
