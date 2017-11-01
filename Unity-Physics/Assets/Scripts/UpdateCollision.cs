using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    List<ColliderBox> cols = FindObjectsOfType<ColliderBox>().ToList();
	    int i = 0;
	    foreach (var colliderBox in cols)
	    {
	        colliderBox.collider.min = colliderBox.gameObject.transform.position - (.5f * colliderBox.gameObject.transform.localScale);
	        colliderBox.collider.max = colliderBox.gameObject.transform.position + (.5f * colliderBox.gameObject.transform.localScale);
        }
	    while (i<cols.Count-1)
	    {
	       
	        if(Utilities.TestOverlap(cols[i].collider, cols[i + 1].collider))
                Debug.Log("Collision");
	        i++;
	    }
	}
}
