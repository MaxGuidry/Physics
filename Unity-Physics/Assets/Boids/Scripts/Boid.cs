using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Max
{

    public class Boid : Agent
    {
        public List<Boid> neighbors = new List<Boid>();
        public override Vector3 Update_Agent()
        {
            velocty += acceleration * Time.fixedDeltaTime;
            if (velocty.magnitude > maxSpeed)
                velocty = velocty.normalized * maxSpeed;
            position += velocty * Time.fixedDeltaTime;
            acceleration = Vector3.zero;
            return position;

        }

        public void AddNeighbor(Boid neighbor)
        {
            if (neighbor == null)
                return;
            if (!neighbors.Contains(neighbor))
                neighbors.Add(neighbor);
        }

        public Vector3 Cohesion()
        {
            Vector3 force = new Vector3();
            //new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;
            if (neighbors.Count == 0)
            {
                
                return Vector3.zero;
            }
            Vector3 com = new Vector3();
            foreach (var neighbor in neighbors)
            {
                com += neighbor.position;
            }

            com = com / neighbors.Count;
            //Debug.DrawLine(this.position,com,Color.black);
            force = com - this.position;

            //  }).Start();
            return force;
        }


    }
}