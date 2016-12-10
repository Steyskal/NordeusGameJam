using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Agent2D : Agent
    {
        public override void Update()
        {
            orientation += rotation * Time.deltaTime;
            if (orientation < 0.0f)
                orientation += 360.0f;
            else if (orientation > 360.0f)
                orientation -= 360.0f;

            Vector3 displacement = velocity * Time.deltaTime;

            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.forward, orientation);

            // example of turn towards direction
            /* 
            Vector2 turnTo = (_wantedDirection * Time.deltaTime);
                float angle = Mathf.Atan2(turnTo.y, turnTo.x) * Mathf.Rad2Deg;
                Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
                if (RotationSpeed < MAX_ROTATION_SPEED)
                {   
                    quaternion = Quaternion.RotateTowards(transform.rotation, quaternion, RotationSpeed);
                }
                transform.rotation = quaternion;
            */
            transform.Translate(displacement, Space.World);
        }
    }
}

