using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cloth
{
    public class SpringDamperBehavior : MonoBehaviour
    {
        public SpringDamper sd;
        public ParticleBehavior a, b;
        public static float springConstant;
        public float restCoefficient;
        public bool dot;

        public float Kd;

        // Use this for initialization
        void Start()
        {
            if (!a || !b)
                return;
            sd = new SpringDamper(a.p, b.p, springConstant, restCoefficient);
        }

        public void Init()
        {
            sd = new SpringDamper(a.p, b.p, springConstant, restCoefficient);
        }

        // Update is called once per frame
        void Update()
        {
            //spring(springConstant,restLength);
#if UNITY_EDITOR
            Debug.DrawLine(a.p.position, b.p.position);
#endif
        }

        public bool Break()
        {
            if ((b.p.position - a.p.position).magnitude > 3.5f * sd.l)
                return true;
            return false;
        }
        public void spring(float springK, float restL)
        {
            sd.k = springK;
            //sd.l = restL;
            dot = true;
            if (!dot)
            {
                Vector3 dir = -(b.p.position - a.p.position).normalized;
                float dist = (b.p.position - a.p.position).magnitude;
                var springForce = -sd.k * (dist - sd.l) * dir;
                var dampForcea = -a.p.velocity * Kd;
                var dampForceb = -b.p.velocity * Kd;
                a.p.AddForce(springForce + dampForcea);
                b.p.AddForce(-springForce + dampForceb);
            }
            else
            {
                Vector3 ep = (b.p.position - a.p.position);
                float l = ep.magnitude;
                Vector3 e = ep / l;

                float v1 = Vector3.Dot(e, a.p.velocity);
                float v2 = Vector3.Dot(e, b.p.velocity);

                float fsd = -sd.k * (restL - l) - Kd * (v1 - v2);
                Vector3 f = fsd * e;

                a.p.AddForce(f);
                b.p.AddForce(-f);
            }
        }
    }
}