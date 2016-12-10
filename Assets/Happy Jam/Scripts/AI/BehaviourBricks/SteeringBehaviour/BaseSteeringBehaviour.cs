using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{
    public abstract class BaseSteeringBehaviour : GOAction
    {
        [InParam("Priority", DefaultValue = 1)]
        [Help("Used if Agent with multiple behaviours is using Priority. Default is 1")]
        public int priority = 1;
        [InParam("Weight", DefaultValue = 1f)]
        [Help("Used if Agent with multiple behavioursis using Weight. Default is 1")]
        public float weight = 1;
        protected IAgent agent;

        public override void OnStart()
        {
            agent = gameObject.GetComponent<IAgent>();
            if (agent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Agent component to navigate. One with default values has been added", gameObject);
                //navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
        }
        public void SetSteering()
        {
            if (agent.IsUsingPriority())
            {
                agent.SetSteering(GetSteering(), priority);
            }
            else
            {
                agent.SetSteering(GetSteering(), weight);
            }

        }

        public virtual Steering GetSteering()
        {
            return new Steering();
        }
    }
}
