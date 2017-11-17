using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Max;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    private Agent a;
    // Use this for initialization
    void Start()
    {


        if (AgentFactory.currentAgents.Count > 0)
            a = AgentFactory.currentAgents[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            List<RaycastHit> hits;
            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).ToList();
            foreach (var hit in hits)
            {
                if (hit.collider.GetType() == typeof(MeshCollider))
                    target = hit.transform;
            }
        }
        if (!target)
        {
            //Quaternion q = this.transform.rotation;
            //this.transform.LookAt(a.GetPosition());
            //this.transform.rotation = Quaternion.Slerp(q, this.transform.rotation, .1f);
            this.transform.LookAt(a.GetPosition());
        }
        else
        {
            //Quaternion q = this.transform.rotation;
            //this.transform.LookAt(target);
            //this.transform.rotation = Quaternion.Slerp(q, this.transform.rotation, .1f);
            this.transform.LookAt(target);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        this.transform.position += this.transform.forward * scroll * 2f;
    }
}
