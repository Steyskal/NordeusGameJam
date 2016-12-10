using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public interface IAgent
    {
        Vector3 GetVelocity();
        void SetSteering(Steering steering, int priority);
        void SetSteering(Steering steering, float weight);
        Steering GetPrioritySteering();
        GameObject GetGameObject();
        Transform GetTransform();
        float GetMaxSpeed();
        float GetMaxAccel();
        float GetOrientation();
        float GetRotation();
        float GetMaxAngularAccel();
        float GetMaxRotation();
        bool IsUsingPriority();
    }
}

