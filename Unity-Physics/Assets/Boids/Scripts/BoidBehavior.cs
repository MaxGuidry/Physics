using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var boid in GameObject.FindObjectsOfType<BoidBehavior>())
            {
                if(this.gameObject == boid.gameObject)
                    continue;
                if((boid.gameObject.transform.position - this.transform.position).magnitude < 5f)
                    (a as Boid).AddNeighbor(boid.a as Boid);
            }
            a.Add_Force( .01f * (a as Boid).Cohesion());
        }
        void FixedUpdate()
        {
            this.transform.position = a.Update_Agent();
        }
    }
}