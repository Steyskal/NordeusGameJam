using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using AI;

namespace BBUnity.Actions
{

    [Action("Steering Behaviour/Find Target")]
    [Help("Setting the target Parameter")]
    public class FindTarget : GOAction
    {
        [InParam("target")]
        [Help("Target that will be setted")]
        public Transform target;

        public override void OnStart()
        {
            base.OnStart();
            if (target == null) { 
                GameObject p = GameManager.Instance.GetPlayer(gameObject.transform.position);
                target = p.transform;
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
