using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cloth;
using UnityEngine;

public class TestSpringDriver : MonoBehaviour
{

    public float ks;

    public float RestCoefficient;
    public float kd;

    private List<SpringDamperBehavior> sdbs = new List<SpringDamperBehavior>();
    private List<ParticleBehavior> pbl = new List<ParticleBehavior>();
	// Use this for initialization
	void Start ()
	{
	    sdbs = FindObjectsOfType<SpringDamperBehavior>().ToList();        
	    pbl = FindObjectsOfType<ParticleBehavior>().ToList();
    }
	
	// Update is called once per frame
	void Update () {
	    foreach (var sd in sdbs)
	    {
	        sd.springConstant = ks;
	        sd.restCoefficient = RestCoefficient ;
	        sd.Kd = kd;
	    }
	    foreach (var pb in pbl)
	    {
	        if (pb.gravity)
	            pb.p.AddForce(new Vector3(0, -9.81f, 0) * .5f);
        }
	    foreach (var sdb in sdbs)
	    {
           // Debug.Log(sdb.sd.l * sdb.restCoefficient);
	        sdb.spring(ks,sdb.sd.l);
	    }
	    foreach (var pb in pbl)
	    {
	        pb.UpdateParticle();
	    }
    }
}
