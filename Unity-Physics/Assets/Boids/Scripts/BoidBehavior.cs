using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Assertions.Must;

namespace Max
{

    public class BoidBehavior : AgentBehavior
    {

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

            foreach (BoidBehavior boid in AgentFactory.currentAgents)
            {
                if (this.gameObject == boid.gameObject)
                    continue;
                if ((boid.a.GetPosition() - a.GetPosition()).magnitude < 5f)
                    (a as Boid).AddNeighbor(boid.a as Boid);
            }

        }

        // Update is called once per frame
        void Update()
        {



        }


        void FixedUpdate()
        {
            this.transform.position = a.Update_Agent();
        }

        public void UpdateBoidInfo()
        {

            foreach (BoidBehavior boid in AgentFactory.currentAgents)
            {
                if (a == boid.a)
                    continue;
                if ((boid.a.GetPosition() - a.GetPosition()).magnitude < 10f)
                    (a as Boid).AddNeighbor(boid.a as Boid);
            }
            Vector3 f = 1f * (a as Boid).Cohesion();
            a.Add_Force(f);
        }
    }
}