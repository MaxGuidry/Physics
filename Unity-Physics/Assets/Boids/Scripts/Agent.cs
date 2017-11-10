using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Max
{


    [System.Serializable]
    public abstract class Agent : ScriptableObject
    {

        [SerializeField]
        protected float mass = 1;
        [SerializeField]
        protected Vector3 velocity, acceleration, position;
        [SerializeField]
        public float maxSpeed = 10;

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
            velocity = Vector3.zero;
            //new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            acceleration = new Vector3();

        }
        public Vector3 GetPosition()
        {
            return position;
        }

    }
}
