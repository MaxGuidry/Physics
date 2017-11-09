using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Max;
using UnityEngine;

public class AgentFactory : MonoBehaviour
{
    private Thread t1,t2,t3,t4;
    private bool threadAlive;
    public int count;
    public List<Agent> agents = new List<Agent>();
    public List<AgentBehavior> agentBehaviors = new List<AgentBehavior>();
    public static List<AgentBehavior> currentAgents = new List<AgentBehavior>();
    [ContextMenu("Create")]
    public void Create()
    {
        if (agents == null)
            agents = new List<Agent>();
        if (agentBehaviors == null)
            agentBehaviors = new List<AgentBehavior>();
        for (int i = 0; i < count; i++)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var behavior = go.AddComponent<BoidBehavior>();
            var boid = ScriptableObject.CreateInstance<Boid>();
            boid.Initialize(1, 10, this.transform.position);
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
        threadAlive = true;
        t1 = new Thread(() =>
        {
            while (threadAlive)
            {
                Thread.CurrentThread.IsBackground = true;
                for (int i = 0; i < AgentFactory.currentAgents.Count / 4f; i++)
                {
                    (AgentFactory.currentAgents[i] as BoidBehavior).UpdateBoidInfo();
                }
                Debug.Log("Thread 1");
            }
        });
        t1.Start();
        t2 = new Thread(() =>
        {
            while (threadAlive)
            {
                Thread.CurrentThread.IsBackground = true;
                for (int i =(int)(AgentFactory.currentAgents.Count/4f); i < (AgentFactory.currentAgents.Count / 4f)+(AgentFactory.currentAgents.Count/4f); i++)
                {
                    (AgentFactory.currentAgents[i] as BoidBehavior).UpdateBoidInfo();
                }
                Debug.Log("Thread 2");
            }
        });
        t2.Start();
        t3 = new Thread(() =>
        {
            while (threadAlive)
            {
                Thread.CurrentThread.IsBackground = true;
                for (int i = (int)(AgentFactory.currentAgents.Count / 4f) + (int)(AgentFactory.currentAgents.Count/4f); i < (AgentFactory.currentAgents.Count / 4f) + (2 * AgentFactory.currentAgents.Count/4f); i++)
                {
                    (AgentFactory.currentAgents[i] as BoidBehavior).UpdateBoidInfo();
                }
                Debug.Log("Thread 3");
            }
        });
        t3.Start();
        t4 = new Thread(() =>
        {
            while (threadAlive)
            {
                Thread.CurrentThread.IsBackground = true;
                for (int i = (int)(AgentFactory.currentAgents.Count / 4f) + (int)(2 * AgentFactory.currentAgents.Count/4f); i < (AgentFactory.currentAgents.Count / 4f) + (3 * AgentFactory.currentAgents.Count/4f); i++)
                {
                    //Debug.Log(i);
                    (AgentFactory.currentAgents[i] as BoidBehavior).UpdateBoidInfo();
                }
                Debug.Log("Thread 4");
            }
        });
        t4.Start();
    }

    // Update is called once per frame
    void Update()
    {
        currentAgents = FindObjectsOfType<AgentBehavior>().ToList();
        if (Input.GetKeyDown(KeyCode.Space))
            threadAlive = !threadAlive;
    }
    void OnDisable()
    {
        threadAlive = false;
        if (t1.IsAlive)
            t1.Abort();
        if (t2.IsAlive)
            t2.Abort();
        if (t3.IsAlive)
            t3.Abort();
        if (t4.IsAlive)
            t4.Abort();
    }
}
