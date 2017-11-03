using UnityEngine;

public class ColliderBox : MonoBehaviour
{
    public AABB collider;
   
    //public GameObject gameObject;
    // Use this for initialization
    void Start()
    {
        collider = new AABB();
        collider.min = this.transform.position - this.transform.localScale;
        collider.max = this.transform.position + this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
