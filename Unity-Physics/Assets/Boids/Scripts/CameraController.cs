using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 center = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        foreach (var currentAgent in AgentFactory.currentAgents)
        {
            center += currentAgent.GetPosition();
            center = center / AgentFactory.currentAgents.Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        center = Vector3.zero;
        
        foreach (var currentAgent in AgentFactory.currentAgents)
        {
            center += currentAgent.GetPosition();
            

        }
        center = center / AgentFactory.currentAgents.Count;
        this.transform.LookAt(center);

    }
}
