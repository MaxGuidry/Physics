using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class UpdateCollision : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    struct AABBPair
    {
        public ColliderBox object1;
        public ColliderBox object2;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var cols = FindObjectsOfType<ColliderBox>().ToList();
        foreach (var colliderBox in cols)
        {
            var bl = colliderBox.gameObject.transform.position;
            
            colliderBox.collider.min = colliderBox.gameObject.transform.position - .5f *
                                       colliderBox.gameObject.transform.localScale *
                                       (1f / Mathf.Cos(Mathf.Deg2Rad *
                                                       (45 - Mathf.Abs(
                                                            45 - colliderBox.gameObject.transform.localEulerAngles.z %
                                                            90f))));
            
           colliderBox.collider.max = colliderBox.gameObject.transform.position + .5f *
                                       colliderBox.gameObject.transform.localScale *
                                       (1f / Mathf.Cos(Mathf.Deg2Rad *
                                                       (45 - Mathf.Abs(
                                                            45 - colliderBox.gameObject.transform.localEulerAngles.z %
                                                            90f))));

            //Debug.DrawLine(colliderBox.collider.min, colliderBox.collider.max);
            Debug.DrawLine(colliderBox.collider.min, new Vector3(colliderBox.collider.min.x, colliderBox.collider.max.y, 0));
            Debug.DrawLine(new Vector3(colliderBox.collider.min.x, colliderBox.collider.max.y, 0), colliderBox.collider.max);
            Debug.DrawLine(colliderBox.collider.max, new Vector3(colliderBox.collider.max.x, colliderBox.collider.min.y, 0));
            Debug.DrawLine(new Vector3(colliderBox.collider.max.x, colliderBox.collider.min.y, 0), colliderBox.collider.min);

        }


        cols.Sort((a, b) => a.collider.min.x.CompareTo(b.collider.min.x));

        List<AABBPair> pairs = new List<AABBPair>();
        //X Axis
        int i;
        List<ColliderBox> activeList = new List<ColliderBox>();
        activeList.Add(cols[0]);
        for (i = 1; i < cols.Count; i++)
        {
            List<ColliderBox> tempActive = new List<ColliderBox>();
            if (activeList.Count == 0)
                activeList.Add(cols[i - 1]);
            tempActive.AddRange(activeList);

            foreach (var colliderBox in activeList)
            {
                if (colliderBox.collider.max.x < cols[i].collider.min.x)
                {
                    tempActive.Remove(colliderBox);
                }
                else
                {
                    tempActive.Add(cols[i]);
                    AABBPair p = new AABBPair();
                    p.object1 = colliderBox;
                    p.object2 = cols[i];
                    pairs.Add(p);

                }
            }
            activeList.Clear();
            activeList.AddRange(tempActive);
        }
        if (pairs.Count != 0)

        //Y Axis
        //cols.Sort((a, b) => a.collider.min.y.CompareTo(b.collider.min.y));
        {

            List<ColliderBox> axis = new List<ColliderBox>();
            foreach (var pair in pairs)
            {
                if (!axis.Contains(pair.object1))
                    axis.Add(pair.object1);
                if (!axis.Contains(pair.object2))
                    axis.Add(pair.object2);
            }

            axis.Sort((a, b) => a.collider.min.y.CompareTo(b.collider.min.y));
            activeList = new List<ColliderBox>();
            activeList.Add(axis[0]);
            for (i = 1; i < axis.Count; i++)
            {
                List<ColliderBox> tempActive = new List<ColliderBox>();
                if (activeList.Count == 0)
                    activeList.Add(axis[i - 1]);
                tempActive.AddRange(activeList);
                foreach (var colliderBox in activeList)
                {
                    if (colliderBox.collider.max.y < axis[i].collider.min.y)
                    {
                        tempActive.Remove(colliderBox);
                    }
                    else
                    {
                        tempActive.Add(axis[i]);
                        AABBPair p = new AABBPair();
                        p.object1 = colliderBox;
                        p.object2 = axis[i];
                        if (!pairs.Contains(p) && !pairs.Contains(new AABBPair() { object1 = p.object2, object2 = p.object1 }))
                            pairs.Add(p);

                    }
                }
                activeList.Clear();
                activeList.AddRange(tempActive);
            }
        }
        Debug.Log(pairs.Count);
            //    //Z Axis
            //    axis = new List<ColliderBox>();
            //    foreach (var pair in pairs)
            //    {
            //        if (axis.Contains(pair.object1))
            //            axis.Add(pair.object1);
            //        if (axis.Contains(pair.object2))
            //            axis.Add(pair.object2);
            //    }
            //    pairs.Clear();

            //    axis.Sort((a, b) => a.collider.min.z.CompareTo(b.collider.min.z));
            //    activeList = new List<ColliderBox>();
            //    activeList.Add(axis[0]);
            //    for (i = 1; i < cols.Count; i++)
            //    {
            //        foreach (var colliderBox in activeList)
            //        {
            //            if (cols[i].collider.min.z < colliderBox.collider.max.z)
            //            {
            //                activeList.Remove(colliderBox);
            //            }
            //            else
            //            {
            //                activeList.Add(cols[i]);
            //                AABBPair p = new AABBPair();
            //                p.object1 = colliderBox;
            //                p.object2 = cols[i];
            //                pairs.Add(p);

            //            }
            //        }
        
    }
}