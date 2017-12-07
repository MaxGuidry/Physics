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
            Vector3 v = (p1.velocity + p2.velocity + p3.velocity) / 3f - Wind;
            Vector3 e = -v.normalized;
            float a0 = .5f * (Vector3.Cross((p2.position - p1.position).normalized, (p3.position - p1.position).normalized)).magnitude;
            Vector3 n = Vector3.Cross((p2.position - p1.position), (p3.position - p1.position)).normalized;
            float a = a0 * (Vector3.Dot(v, n) / v.magnitude);
            Vector3 f = -.5f * p * (v.magnitude * v.magnitude) * Cd * a * n;
            if (Single.IsNaN(f.x) || Single.IsNaN(f.y) || Single.IsNaN(f.z))
                return;
            if (Single.IsInfinity(f.x) || Single.IsInfinity(f.y) || Single.IsInfinity(f.z))
                return;
            p1.AddForce(f / 3f);
            p2.AddForce(f / 3f);
            p3.AddForce(f / 3f);
        }


        public bool CheckCollision()
        {
            Vector3 center = (p1.position + p2.position + p3.position) / 3f;
            Vector3 avgVelo = (p1.velocity + p2.velocity + p3.velocity) / 3f;
            foreach (var collider in Particle.gos)
            {
                if(!collider)
                    continue;
                BoxCollider box = collider.gameObject.GetComponent<BoxCollider>();
                if (box)
                {
                    continue;
                }
                SphereCollider sp = collider.gameObject.GetComponent<SphereCollider>();
                if(sp==null)
                    continue;
                if (sp)
                {

                    //if (Vector3.Distance(center, sp.transform.position) < sp.radius * sp.transform.localScale.x)
                    if (Vector3.Distance(p1.position, sp.transform.position) < Vector3.Distance((p2.position + p3.position) / 2f, p1.position) + sp.radius * sp.transform.localScale.x -.1f&& Vector3.Distance(p2.position, sp.transform.position) < Vector3.Distance((p1.position + p3.position) / 2f, p2.position) + sp.radius * sp.transform.localScale.x - .1f && Vector3.Distance(p3.position, sp.transform.position) < Vector3.Distance((p1.position + p2.position) / 2f, p3.position) + sp.radius * sp.transform.localScale.x - .1f)
                    {
                        var dir = (center - sp.transform.position).normalized;
                        //AddForce(dir * velocity.magnitude*(Vector3.Dot(-f.normalized,dir.normalized)*f.magnitude));
                        Vector3 avgForce = (p1.force + p2.force + p3.force) / 3f;
                        Vector3 avgAccel = (p1.acceleration + p3.acceleration + p3.acceleration) / 3f;
                        float accel = avgAccel.magnitude;
                        float mod = Vector3.Dot(avgVelo.normalized, -dir);
                        if (mod < 0)
                            mod = 0;
                        if (mod < .1f)
                            mod = .2f;
                        if (accel < .2f)
                            accel = 1f;
                        p1.AddForce((p1.position - sp.transform.position).normalized * accel * mod * 4f / 10f * Mathf.Pow(Vector3.Distance(p1.position, sp.transform.position) - sp.transform.localScale.x, 2));//(-(5*Vector3.Distance(p1.position,sp.transform.position)-Mathf.Pow(sp.transform.localScale.x,2)-sp.transform.localScale.x)+ sp.transform.localScale.x));
                        p2.AddForce((p1.position - sp.transform.position).normalized * accel * mod * 4f / 10f * Mathf.Pow(Vector3.Distance(p2.position, sp.transform.position) - sp.transform.localScale.x, 2));//(-(5*Vector3.Distance(p2.position,sp.transform.position)-Mathf.Pow(sp.transform.localScale.x,2)-sp.transform.localScale.x)+ sp.transform.localScale.x));
                        p3.AddForce((p1.position - sp.transform.position).normalized * accel * mod * 4f / 10f * Mathf.Pow(Vector3.Distance(p3.position, sp.transform.position) - sp.transform.localScale.x, 2));//(-(5*Vector3.Distance(p3.position,sp.transform.position)-Mathf.Pow(sp.transform.localScale.x,2)-sp.transform.localScale.x)+ sp.transform.localScale.x));
                    }
                    //if (Vector3.Distance(r, sp.transform.position) < sp.radius * sp.transform.localScale.x)
                    //{
                    //    var dir = (r - sp.transform.position).normalized;
                    //    //AddForce(dir * velocity.magnitude*(Vector3.Dot(-f.normalized,dir.normalized)*f.magnitude));
                    //    AddForce(dir * velocity.magnitude * 100f);
                    //}
                }
            }
            return true;
        }

    }
}