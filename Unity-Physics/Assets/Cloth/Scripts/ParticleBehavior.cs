﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cloth
{



    public class ParticleBehavior : MonoBehaviour
    {
        public bool gravity = true;
        public Particle p;

        public bool anchorx,anchory,anchorz;

        public bool isAnchor;
        // Use this for initialization
        void OnEnable()
        {
            p = new Particle(this.transform.position, Vector3.zero, 1);
            p.anchorx = anchorx;
            p.anchory = anchory;
            p.anchorz = anchorz;
            p.m_IsAnchor = isAnchor;

        }

        // Update is called once per frame
        void Update()
        {
            p.gravity = gravity;
            p.anchorx = anchorx;
            p.anchory = anchory;
            p.anchorz = anchorz;
            p.m_IsAnchor = isAnchor;
            // p.AddForce(Vector3.right);
        }

        public void UpdateParticle()
        {
            //p.position = transform.position;
            p.Update(Time.fixedDeltaTime);
            transform.position = p.position;
        }

    }
}