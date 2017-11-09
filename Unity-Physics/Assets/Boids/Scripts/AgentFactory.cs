using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Max;
using UnityEngine;

public class AgentFactory : MonoBehaviour
{
    public static int cFactor =0, sFactor = 0, aFactor = 0, wFactor = 0;
    public int count;
    public List<Agent> agents = new List<Agent>();
    public List<AgentBehavior> agentBehaviors = new List<AgentBehavior>();
    public static List<AgentBehavior> currentAgents = new List<AgentBehavior>();
    [ContextMenu("Create")]
    public void Create()
    {
        if (agents == null)
            agents = new List<Agent>();
        if(agentBehaviors == null)
            agentBehaviors = new List<AgentBehavior>();
        for (int i = 0; i < count; i++)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var behavior = go.AddComponent<BoidBehavior>();
            var boid = ScriptableObject.CreateInstance<Boid>();
            boid.Initialize(1,10,this.transform.position);
            go.transform.SetParent(this.transform);
            go.name = "Agent: " + i;
            agentBehaviors.Add(behavior);
            agents.Add(boid);
            behavior.SetAgent(boid);
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

    }

    // Update is called once per frame
    void Update()
    {
        currentAgents = FindObjectsOfType<AgentBehavior>().ToList();
    }
}
