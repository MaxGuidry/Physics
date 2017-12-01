using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

namespace Cloth
{


    [System.Serializable]
    public class Particle
    {
        [SerializeField] private Vector3 r;
        [SerializeField] private Vector3 v;
        [SerializeField] private Vector3 a;
        [SerializeField] private Vector3 f;
        [SerializeField] private float m;
        [HideInInspector]
        public bool anchorx, anchory, anchorz, gravity;

        public bool m_IsAnchor;

        public Vector3 position
        {
            get { return r; }
            set { r = value; }
        }

        public Vector3 velocity
        {
            get { return v; }
            private set { }
        }
        public Vector3 acceleration
        {
            get { return a; }
            private set { }
        }
        public Vector3 force
        {
            get { return f; }
            private set { }
        }
        public float mass
        {
            get { return m; }
            private set { }
        }

        public Particle(Vector3 pos, Vector3 velo, float mass)
        {
            r = pos;
            v = velo;
            m = mass;
            a = Vector3.zero;
        }
        public void AddForce(Vector3 force)
        {
            f += force;
        }
        public void Update(float deltaTime)
        {
            if (m_IsAnchor) return;
            a = f / m;
            v = v + a * deltaTime;
            r = r + v * deltaTime;
            f = Vector3.zero;
        }
    }

    [System.Serializable]
    public class SpringDamper
    {
        public Particle a, b;
        public float k;
        public float l;

        public SpringDamper()
        {

        }
        public SpringDamper(Particle a, Particle b, float ks, float lo)
        {
            this.a = a;
            this.b = b;
            k = ks;
            l = Vector3.Distance(a.position, b.position);
     
        }

    }

}