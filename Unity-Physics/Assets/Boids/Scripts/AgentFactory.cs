using System;
using System.Collections;
using System.Collections.Generic;
using Max;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgentFactory : MonoBehaviour
{
    public GameObject agentmodelPrefab;
    public static int cFactor = 0, sFactor = 0, aFactor = 0, wFactor = 0;
    public int count;
    [HideInInspector]
    public List<Agent> agents = new List<Agent>();
    [HideInInspector]
    public List<AgentBehavior> agentBehaviors = new List<AgentBehavior>();
    public static List<Agent> currentAgents = new List<Agent>();
    [ContextMenu("Create")]
    public void Create()
    {
        if (agents == null)
            agents = new List<Agent>();
        if (agentBehaviors == null)
            agentBehaviors = new List<AgentBehavior>();
        for (int i = 0; i < count; i++)
        {
            
           // var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var go = GameObject.Instantiate(agentmodelPrefab);
            MeshRenderer r = go.GetComponent<MeshRenderer>();
            //r.material.color = Random.ColorHSV();
            r.material.color = Color.clear;
            
            var behavior = go.AddComponent<BoidBehavior>();
            var boid = ScriptableObject.CreateInstance<Boid>();
            boid.Initialize(1, 50, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
            go.transform.SetParent(this.transform);
            go.name = "Agent: " + i;
            agentBehaviors.Add(behavior);
            agents.Add(boid);
            behavior.SetAgent(boid);
            currentAgents = agents;
        }
    }
    [ContextMenu("Destroy")]
    public void Destroy()
    {
        foreach (var a in agentBehaviors)
        {
            DestroyImmediate(a.gameObject);
        }
        agentBehaviors.Clear();
        agents.Clear();
    }


    // Use this for initialization
    void Start()
    {
        Create();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static List<Agent> GetAgents(Type t)
    {
        List<Agent> l = new List<Agent>();
        foreach (var agent in currentAgents)
        {
            if (agent.GetType() == t)
                l.Add(agent);
        }
        return l;
    }
}
