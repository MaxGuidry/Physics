using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFactors : MonoBehaviour
{

    public Slider CohesionSlider;
    public Slider SeperationSlider;
    public Slider AlignmentSlider;
    public Slider WanderSlider;
    public Slider MaxSpeedSlider;
    // Use this for initialization
    void Start()
    {
        CohesionSlider.value = AgentFactory.cFactor;
        SeperationSlider.value = AgentFactory.sFactor;
        AlignmentSlider.value = AgentFactory.aFactor;
        WanderSlider.value = AgentFactory.wFactor;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Change(string factor)
    {
        switch (factor)
        {
            case "c":
            {
                AgentFactory.cFactor = (int)CohesionSlider.value;
                break;
            }
            case "s":
            {
                AgentFactory.sFactor = (int)SeperationSlider.value;
                break;
            }
            case "a":
            {
                AgentFactory.aFactor = (int)AlignmentSlider.value;
                break;
            }
            case "w":
            {
                AgentFactory.wFactor = (int)WanderSlider.value;
                break;
            }
            case "speed":
            {
                foreach (var currentAgent in AgentFactory.currentAgents)
                {
                    currentAgent.maxSpeed = MaxSpeedSlider.value;
                }
                break;
            }
        }

    }
}
