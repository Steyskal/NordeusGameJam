using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Original basic agent
    /// Require changes for working with Rigidbody
    /// </summary>
    public class Agent : MonoBehaviour, IAgent
    {
        public float PriorityThreshold = 0.2f;
        public float MaxSpeed = 1;
        public float MaxAccel = 1;
        [Header("Multiple behaviours")]
        public bool UsePriority = true;
        public bool UseBlendByWeight = false;

        [Header("Used externaly")]
        public float MaxRotation;
        public float MaxAngularAccel;
        [Tooltip("Higer value better precision. Good Precision = 50")]
        public float RectionTime = 2;

        [Header("Read Only")]
        public Vector3 velocity;
        public float orientation = 0f;
        public float rotation = 0f;

        protected Steering steering;
        protected Dictionary<int, List<Steering>> groups;

        protected virtual void Start()
        {
            steering = new Steering();
            groups = new Dictionary<int, List<Steering>>();

            orientation = 1;
            rotation = 1;
        }
        public virtual void Update()
        {
            Vector3 displacement = velocity * Time.deltaTime ;

            orientation += rotation * Time.deltaTime ;
            if (orientation < 0.0f)
                orientation += 360.0f;
            else if (orientation > 360.0f)
                orientation -= 360.0f;
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, orientation);

            transform.Translate(displacement, Space.World);
        }
        public virtual void LateUpdate()
        {
            if (IsUsingPriority())
            {
                steering = GetPrioritySteering();
                groups.Clear();
            }

            velocity += steering.linear * Time.deltaTime* RectionTime;
            rotation += steering.angular * Time.deltaTime * RectionTime;

            if (velocity.magnitude > MaxSpeed)
            {
                velocity.Normalize();
                velocity = velocity * MaxSpeed;
            }
            if (steering.angular == 0.0f)
            {
                rotation = 0.0f;
            }
            if (steering.linear.sqrMagnitude == 0.0f)
            {
                velocity = Vector3.zero;
            }
            steering = new Steering();

            //CurSpeed = rigidBody.velocity.magnitude;
            //SetAnimationSpeed(CurSpeed);
        }
        public Vector3 GetVelocity()
        {
            return velocity;
        }


        public void SetSteering(Steering steering, int priority)
        {
            if (!groups.ContainsKey(priority))
            {
                groups.Add(priority, new List<Steering>());
            }
            groups[priority].Add(steering);
        }

        public void SetSteering(Steering steering, float weight)
        {
            this.steering.linear += (weight * steering.linear);
            this.steering.angular += (weight * steering.angular);
        }
        public Steering GetPrioritySteering()
        {
            Steering steering = new Steering();
            float sqrThreshold = PriorityThreshold * PriorityThreshold;
            foreach (List<Steering> group in groups.Values)
            {
                steering = new Steering();
                foreach (Steering singleSteering in group)
                {
                    steering.linear += singleSteering.linear;
                    steering.angular += singleSteering.angular;
                }
                if (steering.linear.sqrMagnitude > sqrThreshold ||
                        Mathf.Abs(steering.angular) > PriorityThreshold)
                {
                    return steering;
                }
            }
            return steering;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public float GetMaxSpeed()
        {
            return MaxSpeed;
        }

        public float GetMaxAccel()
        {
            return MaxAccel;
        }

        public float GetOrientation()
        {
            return orientation;
        }

        public float GetRotation()
        {
            return rotation;
        }

        public float GetMaxAngularAccel()
        {
            return MaxAngularAccel;
        }

        public float GetMaxRotation()
        {
            return MaxRotation;
        }
        public bool IsUsingPriority()
        {
            return UsePriority && !UseBlendByWeight;
        }
    }
}

