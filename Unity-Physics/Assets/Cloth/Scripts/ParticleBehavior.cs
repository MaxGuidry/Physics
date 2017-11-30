using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{



    public class ParticleBehavior : MonoBehaviour
    {

        public Particle p;

        public bool anchor;
        // Use this for initialization
        void Start()
        {
            p = new Particle(this.transform.position, Vector3.zero, 1);
            p.anchor = anchor;
        }

        // Update is called once per frame
        void Update()
        {
            p.anchor = anchor;

            // p.AddForce(Vector3.right);
        }

        void FixedUpdate()
        {
            p.Update(Time.deltaTime);
            transform.position = p.position;
        }
    }
}