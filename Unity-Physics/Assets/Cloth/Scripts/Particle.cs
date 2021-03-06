﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        [HideInInspector] public bool anchorx, anchory, anchorz, gravity;

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
        public static List<Collider> gos = new List<Collider>();
        public Particle(Vector3 pos, Vector3 velo, float mass)
        {
            r = pos;
            v = velo;
            m = mass;
            a = Vector3.zero;
            gos = GameObject.FindObjectsOfType<Collider>().ToList();
        }

        public void AddForce(Vector3 force)
        {
            f += force;
        }

        public void Update(float deltaTime)
        {
            if (m_IsAnchor) return;
            //CheckCollision();
            a = f / m;
            v = v + a * deltaTime;
            if (v.magnitude > 75)
                v = v.normalized * 75;
            r = r + v * deltaTime;
            if (Single.IsInfinity(r.x) || Single.IsInfinity(r.y) || Single.IsInfinity(r.z))
                r = Vector3.zero;
            if (Single.IsNaN(r.x) || Single.IsNaN(r.y) || Single.IsNaN(r.z))
                r = Vector3.zero;

            f = Vector3.zero;
            if (r.y <= 0)
                r.y = 0;
        }

        //public void FixBallCollisionPosition(Vector3 dir, float dist)
        //{
        //    r += dir.normalized * dist;
        //}

        public void DragParticle(Vector3 pos)
        {
            r = pos;
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