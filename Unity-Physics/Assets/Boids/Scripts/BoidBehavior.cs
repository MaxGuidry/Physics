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

        void OnEnable()
        {
            if (a == null)

            {
                a = new Boid();
                a.Initialize(1, 2, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
            }
        }
        // Use this for initialization
        void Start()
        {
            //SphereCollider col = this.GetComponent<SphereCollider>();
            //if (col == null)
            //    col = this.gameObject.AddComponent<SphereCollider>();
            //col.radius = 20;
            //col.isTrigger = true;
           

            foreach (BoidBehavior boid in GameObject.FindObjectsOfType<AgentBehavior>())
            {
                if (this.gameObject == boid.gameObject)
                    continue;
                if ((boid.a.GetPosition() - a.GetPosition()).magnitude < 20)
                    (a as Boid).AddNeighbor(boid.a as Boid);
            }
           
        }

        // Update is called once per frame
        void Update()
        {

            
            Vector3 f = .1f * (a as Boid).Cohesion();
            a.Add_Force(f);

          
        }
        
        void OnDisable()
        {
           
        }
        void FixedUpdate()
        {
            this.transform.position = a.Update_Agent();
        }

        //void OnTriggerEnter(Collider other)
        //{
        //    BoidBehavior bh = other.gameObject.GetComponent<BoidBehavior>();
        //    if (bh!= null)
        //        (a as Boid).AddNeighbor(bh.a as Boid);
        //}

        //void OnTriggerExit(Collider other)
        //{
        //    BoidBehavior bh = other.gameObject.GetComponent<BoidBehavior>();
        //    if (bh != null)
        //        (a as Boid).RemoveNeighbor(bh.a as Boid);
        //}
    }
}