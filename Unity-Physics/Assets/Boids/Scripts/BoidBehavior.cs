using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Assertions.Must;

namespace Max
{

    public class BoidBehavior : AgentBehavior
    {
        private Thread t;
        public void SetAgent(Agent agent)
        {

            a = agent;
        }
        // Use this for initialization
        void Start()
        {
           
            if (a == null)

            {
                a = new Boid();
                a.Initialize(1, 2, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
            }

            foreach (var boid in GameObject.FindObjectsOfType<BoidBehavior>())
            {
                if (this.gameObject == boid.gameObject)
                    continue;
                if ((boid.gameObject.transform.position - this.transform.position).magnitude < 5f)
                    (a as Boid).AddNeighbor(boid.a as Boid);
            }
            t= new Thread(() =>
            {
                while (true)
                {
                    Thread.CurrentThread.IsBackground = true;
                    Vector3 f = .1f * (a as Boid).Cohesion();
                    a.Add_Force(f);
                }
            });
            t.Start();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var boid in GameObject.FindObjectsOfType<BoidBehavior>())
            {
                if (this.gameObject == boid.gameObject)
                    continue;
                if ((boid.gameObject.transform.position - this.transform.position).magnitude < 15f)
                    (a as Boid).AddNeighbor(boid.a as Boid);
            }
    
        }
        void FixedUpdate()
        {
            this.transform.position = a.Update_Agent();
        }
    }
}