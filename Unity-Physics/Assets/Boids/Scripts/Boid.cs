using System;
using System.Collections;
using System.Collections.Generic;
using Max;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Max
{

    public class Boid : Agent
    {
        
        public List<Boid> neighbors = new List<Boid>();
        public override Vector3 Update_Agent()
        {
            
            velocity += acceleration * Time.fixedDeltaTime;
            
            if (velocity.magnitude > maxSpeed)
                velocity = velocity.normalized * maxSpeed;
            position += velocity * Time.fixedDeltaTime;
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

        public void RemoveNeighbor(Boid neighbor)
        {
            if(neighbor== null)
                return;
            if (neighbors.Contains(neighbor))
                neighbors.Remove(neighbor);
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
            com = com - this.position;
            force = com;

            //  }).Start();
            return force;
        }

        public Vector3 Seperation()
        {
            if(neighbors.Count == 0)
                return Vector3.zero;
            Vector3 force = new Vector3();
            foreach (var neighbor in neighbors)
            {
                Vector3 dif = position - neighbor.position;
                if (dif.magnitude == 0)
                    return Wander();
                if (dif.magnitude < 10)
                    force += (position - neighbor.position).normalized * 10f/dif.magnitude;
            }
            return force;
        }

        public Vector3 Alignment()
        {
            if(neighbors.Count == 0)
                return Vector3.zero;
            Vector3 force = new Vector3();
            Vector3 avgVelo = new Vector3();
            foreach (var neighbor in neighbors)
            {
                avgVelo += neighbor.velocity;
            }
            avgVelo = avgVelo / neighbors.Count;
            force = avgVelo;
            return force;

        }

        public Vector3 Wander()
        {
            return (new Vector3(Mathf.Cos(Random.Range(0,180)), Mathf.Cos(Random.Range(0, 180)), Mathf.Cos(Random.Range(0, 180))).normalized * (velocity.magnitude+1f));
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }
    }
}

