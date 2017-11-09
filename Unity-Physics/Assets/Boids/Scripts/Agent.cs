using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Max
{



    public abstract class Agent : ScriptableObject
    {

        [SerializeField]
        protected float mass = 1;
        [SerializeField]
        protected Vector3 velocty, acceleration, position;
        [SerializeField]
        protected float maxSpeed = 10;

        public abstract Vector3 Update_Agent();

        public bool Add_Force(float force, Vector3 direction)
        {
            if (force == 0)
                return false;
            Vector3 fVec = direction.normalized * force;
            acceleration += fVec / mass;
            return true;
        }

        public bool Add_Force(Vector3 force)
        {
            if (Add_Force(force.magnitude, force.normalized))
                return true;
            return false;
        }

        public void Initialize(float Mass, float maxS,Vector3 pos)
        {
            
            this.mass = Mass;
            maxSpeed = maxS;
            position = pos;


        }
        public Vector3 GetPosition()
        {
            return position;
        }

    }
}
