using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{


    [System.Serializable]
    public class Triangle
    {
        //public Vector3 AirFlow = new Vector3(1,0,1).normalized;
        public float p = 1;
        public float Cd = 1;
        public Particle p1, p2, p3;

        public Triangle(Particle a, Particle b, Particle c)
        {
            p1 = a;
            p2 = b;
            p3 = c;
        }
        public void AreodynamicForce(Vector3 Wind)
        {
            Vector3 v = (p1.velocity + p2.velocity + p3.velocity)/3f-Wind;
            Vector3 e = -v.normalized;
            float a0 = .5f * (Vector3.Cross((p2.position - p1.position).normalized,(p3.position - p1.position).normalized)).magnitude;
            Vector3 n = Vector3.Cross((p2.position - p1.position), (p3.position - p1.position)).normalized;
            float a = a0 * (Vector3.Dot(v, n) / v.magnitude);
            Vector3 f = -.5f * p * (v.magnitude * v.magnitude) * Cd * a * n;
            if (Single.IsNaN(f.x) || Single.IsNaN(f.y) || Single.IsNaN(f.z))
                return;
            if (Single.IsInfinity(f.x) || Single.IsInfinity(f.y) || Single.IsInfinity(f.z))
                return;
            p1.AddForce(f/3f);
            p2.AddForce(f/3f);
            p3.AddForce(f/3f);
        }

        public void CheckCollision()
        {
            
        }
    }
}