using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            
            return position;

        }

        public void AddNeighbor(Boid neighbor)
        {
            if(!neighbors.Contains(neighbor))
                neighbors.Add(neighbor);
        }

        public Vector3 Cohesion()
        {
            Vector3 force;
            if(neighbors.Count == 0)
                return Vector3.zero;
            Vector3 com = new Vector3();
            foreach (var neighbor in neighbors)
            {
                com += neighbor.position;
            }
            com = com / neighbors.Count;
            force = this.position - com;
            return force;
        }

       
    }
}