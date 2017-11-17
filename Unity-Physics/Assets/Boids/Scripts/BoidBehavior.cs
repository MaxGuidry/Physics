using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

namespace Max
{

    public class BoidBehavior : AgentBehavior
    {
        private Thread t;
        public void SetAgent(Agent agent)
        {

            a = agent;
        }

        void OnEnable()
        {

            if (a == null)

            {
                a = ScriptableObject.CreateInstance<Boid>();
                a.Initialize(1, 50, new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)));

            }

        }
        // Use this for initialization
        void Start()
        {
            SphereCollider col = this.GetComponent<SphereCollider>();
            if (col == null)
                col = this.gameObject.AddComponent<SphereCollider>();
            col.radius = 2;
            col.isTrigger = true;
            Rigidbody rb = this.GetComponent<Rigidbody>();
            if (rb == null)
                rb = this.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;

            //foreach (BoidBehavior boid in GameObject.FindObjectsOfType<AgentBehavior>())
            //{
            //    if (this.gameObject == boid.gameObject)
            //        continue;
            //    if ((boid.a.GetPosition() - a.GetPosition()).magnitude < col.radius)
            //        (a as Boid).AddNeighbor(boid.a as Boid);
            //}

        }

        // Update is called once per frame
        void Update()
        {
            //(a as Boid).neighbors = AgentFactory.currentAgents; //AgentFactory.GetAgents(typeof(Boid));
            Vector3 c = .5f * AgentFactory.cFactor * (a as Boid).Cohesion();
            Debug.DrawLine(this.transform.position, this.transform.position + c.normalized * 2, Color.blue);
            Vector3 s = .5f * AgentFactory.sFactor * (a as Boid).Seperation();
            Debug.DrawLine(this.transform.position, this.transform.position + s.normalized * 2, Color.green);
            Vector3 al = .5f * AgentFactory.aFactor * (a as Boid).Alignment();
            Debug.DrawLine(this.transform.position, this.transform.position + al.normalized * 2, Color.red);
            Vector3 w = .5f * AgentFactory.wFactor * (a as Boid).Wander();
            a.Add_Force(c + s + al + w);
            //this.transform.LookAt(this.transform.position + (a as Boid).GetVelocity());
            this.transform.up = (a as Boid).GetVelocity().normalized;

        }

        void OnDisable()
        {
            if (t != null)
                t.Abort();
        }
        void FixedUpdate()
        {
            //if (AgentFactory.currentAgents.Count != 0)
            //    Debug.Log(AgentFactory.currentAgents[0].maxSpeed);
          
            this.transform.position = a.Update_Agent();
            if (this.transform.position.magnitude > 50)
                a.Add_Force(50 * (a as Boid).GetPosition().magnitude, -a.GetPosition());
        }

        void OnTriggerEnter(Collider other)
        {
            BoidBehavior bh = other.gameObject.GetComponent<BoidBehavior>();
            if (bh != null)
                (a as Boid).AddNeighbor(bh.a as Boid);
        }

        void OnTriggerExit(Collider other)
        {
            BoidBehavior bh = other.gameObject.GetComponent<BoidBehavior>();
            if (bh != null)
                (a as Boid).RemoveNeighbor(bh.a as Boid);
        }

        void OnMouseDown()
        {
            //Camera.main.GetComponent<CameraController>().target = this.transform;
        }
    }
}