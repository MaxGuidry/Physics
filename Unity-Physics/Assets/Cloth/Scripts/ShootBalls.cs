using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cloth ;

public class ShootBalls : MonoBehaviour
{

    public GameObject Ball;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Mouse0))
            ShootBall();
	}

    public void ShootBall()
    {
       
        Ray world = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject go = Instantiate(Ball,Camera.main.transform.position,Quaternion.identity);
        go.GetComponent<Rigidbody>().AddForce(world.direction * 20,ForceMode.Impulse);
        Cloth.Particle.gos = GameObject.FindObjectsOfType<Collider>().ToList();
        Destroy(go,5);
    }

   
}
