using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cloth
{



    public class SpringDamperBehavior : MonoBehaviour
    {
        private SpringDamper sd;
        public ParticleBehavior a, b;
        // Use this for initialization
        void Start()
        {
            sd = new SpringDamper(a.p, b.p, 6f, 3);
        }

        // Update is called once per frame
        void Update()
        {
            

            spring();
        }

        public void spring()
        {
            
            Vector3 dir = -(b.p.position - a.p.position).normalized;
            float dist = (b.p.position - a.p.position).magnitude;
            a.p.AddForce(-sd.k * (dist - sd.l) * dir);
            dir = -(a.p.position - b.p.position).normalized;
            dist = (a.p.position - b.p.position).magnitude;
            b.p.AddForce(-sd.k * (dist - sd.l) * dir);
            a.p.AddForce(-a.p.velocity * .5f);
            b.p.AddForce(-b.p.velocity * .5f);

        }
    }
}