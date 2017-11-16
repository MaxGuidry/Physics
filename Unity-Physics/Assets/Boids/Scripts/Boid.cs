using System;
using System.Collections;
using System.Collections.Generic;
using Max;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Max
{
    [System.Serializable]
    public class Boid : Agent
    {
        
        public List<Agent> neighbors = new List<Agent>();
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
                if (neighbor == (this as Agent))
                    continue;
                com += (neighbor as Boid).position * (neighbor as Boid).mass;
            }
            
            com = com / neighbors.Count;
            //Debug.DrawLine(this.position,com,Color.black);
            com = com - this.position;
            force = com;

            //  }).Start();
            return force.normalized;
        }

        public Vector3 Seperation()
        {
            if(neighbors.Count == 0)
                return Vector3.zero;
            Vector3 force = new Vector3();
            foreach (var neighbor in neighbors)
            {
                if(neighbor == (this as Agent))
                    continue;
                Vector3 dif = position - (neighbor as Boid).position;
                if (dif.magnitude == 0)
                    force += Vector3.zero;
                else if (dif.magnitude < 3)
                    force += (position - (neighbor as Boid).position).normalized * (30f/(dif.magnitude+2)-5);
            }
            return force;
        }

        public Vector3 Alignment()
        {
            if(neighbors.Count == 0)
                return Vector3.zero;
            Vector3 avgVelo = new Vector3();
            foreach (var neighbor in neighbors)
            {
                if (neighbor == (this as Agent))
                    continue;
                avgVelo += (neighbor as Boid).velocity;
            }
            avgVelo = avgVelo / neighbors.Count;
             
            return avgVelo.normalized;

        }

        public Vector3 Wander()
        {
            //return (new Vector3(Mathf.Cos(Random.Range(0,170)), Mathf.Cos(Random.Range(0, 170)), Mathf.Cos(Random.Range(0, 170))).normalized * .1f * (velocity.magnitude+1f));
            //new Vector3(Mathf.Cos(Random.Range(0, 170)), Mathf.Cos(Random.Range(0, 170)), Mathf.Cos(Random.Range(0, 170))) + position
            return velocity.normalized + new Vector3(Mathf.Cos(Random.Range(-60, 60)), Mathf.Cos(Random.Range(-60, 60)),
                       Mathf.Cos(Random.Range(-60, 60))).normalized * 5;
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }
    }
}

