using System.Linq;
using UnityEngine;
using System.Collections.Generic;


public class UpdateCollision : MonoBehaviour
{
    public enum AXIS
    {
        X,
        Y,
        Z,
    }

    // Use this for initialization
    private void Start()
    {
        //for (int i = 0; i < 7000; i++)
        //{
        //    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //    go.transform.position = new Vector3(2 * i, 2 * i, 0);

        //    ColliderBox cb = go.AddComponent<ColliderBox>();
        //    if (cb.collider == null)
        //        cb.collider = new AABB();

        //}
    }
    [System.Serializable]
    public struct AABBPair
    {
        public ColliderBox object1;
        public ColliderBox object2;

    }
    public List<AABBPair> XPair = new List<AABBPair>();
    public List<AABBPair> YPair = new List<AABBPair>();
    public List<AABBPair> ZPair = new List<AABBPair>();
    public List<AABBPair> Collisions = new List<AABBPair>();

    void Update()
    {


    }
    private void FixedUpdate()
    {

        var cols = FindObjectsOfType<ColliderBox>().ToList();
        if (cols.Count == 0)
            return;
        foreach (var colliderBox in cols)
        {


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
            //Debug.DrawLine(colliderBox.collider.min,
            //    new Vector3(colliderBox.collider.min.x, colliderBox.collider.max.y, 0));
            //Debug.DrawLine(new Vector3(colliderBox.collider.min.x, colliderBox.collider.max.y, 0),
            //    colliderBox.collider.max);
            //Debug.DrawLine(colliderBox.collider.max,
            //    new Vector3(colliderBox.collider.max.x, colliderBox.collider.min.y, 0));
            //Debug.DrawLine(new Vector3(colliderBox.collider.max.x, colliderBox.collider.min.y, 0),
            //    colliderBox.collider.min);

        }
        XPair.Clear();
        YPair.Clear();
        //ZPair.Clear();
        XPair = SortAndSweep(cols, AXIS.X);
        List<ColliderBox> objs = new List<ColliderBox>();
        foreach (var pair in XPair)
        {
            if (!objs.Contains(pair.object1))
                objs.Add(pair.object1);
            if (!objs.Contains(pair.object2))
                objs.Add(pair.object2);
        }
        if (objs.Count != 0)

        {
            YPair = SortAndSweep(objs, AXIS.Y);

            List<List<AABBPair>> pairslist = new List<List<AABBPair>>();
            pairslist.Add(XPair);
            pairslist.Add(YPair);
            Collisions = CombineCollisions(pairslist);
        }



        cols = FindObjectsOfType<ColliderBox>().ToList();
        XPair.Clear();
        YPair.Clear();
        XPair = SortAndSweep(cols, AXIS.X);
        YPair = SortAndSweep(cols, AXIS.Y);
        List<List<AABBPair>> pairList = new List<List<AABBPair>>();
        pairList.Add(XPair);
        pairList.Add(YPair);
        Collisions = CombineCollisions(pairList);

        Debug.Log(Collisions.Count);
        #region MyRegion




        /*

        cols.Sort((a, b) => a.collider.min.x.CompareTo(b.collider.min.x));

        List<AABBPair> pairs = new List<AABBPair>();
        //X Axis
        int i;
        List<ColliderBox> activeList = new List<ColliderBox>();
        activeList.Add(cols[0]);
        for (i = 0; i < cols.Count - 1; i++)
        {
            List<ColliderBox> tempActive = new List<ColliderBox>();
            if (activeList.Count == 0)
                activeList.Add(cols[i]);
            tempActive.AddRange(activeList);

            foreach (var colliderBox in activeList)
            {
                if (colliderBox.collider.max.x < cols[i + 1].collider.min.x)
                {
                    tempActive.Remove(colliderBox);
                }
                else
                {
                    tempActive.Add(cols[i + 1]);
                    AABBPair p = new AABBPair();
                    p.object1 = colliderBox;
                    p.object2 = cols[i + 1];
                    if (!pairs.Contains(p) &&
                        !pairs.Contains(new AABBPair() { object1 = p.object2, object2 = p.object1 }))
                        pairs.Add(p);

                }
            }
            activeList.Clear();
            activeList.AddRange(tempActive);
        }

        if (pairs.Count != 0)
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
            for (i = 0; i < axis.Count - 1; i++)
            {
                List<ColliderBox> tempActive = new List<ColliderBox>();
                if (activeList.Count == 0)
                    activeList.Add(axis[i]);
                tempActive.AddRange(activeList);
                foreach (var colliderBox in activeList)
                {
                    if (colliderBox.collider.max.y < axis[i + 1].collider.min.y)
                    {
                        tempActive.Remove(colliderBox);
                    }
                    else
                    {
                        tempActive.Add(axis[i + 1]);
                        AABBPair p = new AABBPair();
                        p.object1 = colliderBox;
                        p.object2 = axis[i + 1];
                        if (pairs.Contains(p) ||
                            pairs.Contains(new AABBPair() { object1 = p.object2, object2 = p.object1 }))
                        {
                            if (MatchedPair.Contains(p) ||
                                MatchedPair.Contains(new AABBPair() { object1 = p.object2, object2 = p.object1 }))
                                MatchedPair.Add(p);
                        }

                    }
                }
                activeList.Clear();
                activeList.AddRange(tempActive);
            }
        }
        Debug.Log(MatchedPair.Count);
        //Z Axis
        axis = new List<ColliderBox>();
        foreach (var pair in pairs)
        {
            if (axis.Contains(pair.object1))
                axis.Add(pair.object1);
            if (axis.Contains(pair.object2))
                axis.Add(pair.object2);
        }
        pairs.Clear();

        axis.Sort((a, b) => a.collider.min.z.CompareTo(b.collider.min.z));
        activeList = new List<ColliderBox>();
        activeList.Add(axis[0]);
        for (i = 1; i < cols.Count; i++)
        {
            foreach (var colliderBox in activeList)
            {
                if (cols[i].collider.min.z < colliderBox.collider.max.z)
                {
                    activeList.Remove(colliderBox);
                }
                else
                {
                    activeList.Add(cols[i]);
                    AABBPair p = new AABBPair();
                    p.object1 = colliderBox;
                    p.object2 = cols[i];
                    pairs.Add(p);

                }
            }

        }
        */

        #endregion
    }

    public List<AABBPair> CombineCollisions(List<List<AABBPair>> allPairs)
    {

        List<AABBPair> AllAxis = new List<AABBPair>();

        foreach (var aabbPair in allPairs[0])
        {
            bool all = true;
            foreach (var pair in allPairs)
            {
                if (!pair.Contains(aabbPair) && !pair.Contains(new AABBPair() { object1 = aabbPair.object2, object2 = aabbPair.object1 }))
                    all = false;
            }
            if (all)
                AllAxis.Add(aabbPair);
        }
        return AllAxis;
    }
    public List<AABBPair> SortAndSweep(List<ColliderBox> objs, AXIS axis)
    {
        switch (axis)
        {
            case AXIS.X:
                objs.Sort((a, b) => a.collider.min.x.CompareTo(b.collider.min.x));
                break;
            case AXIS.Y:
                objs.Sort((a, b) => a.collider.min.y.CompareTo(b.collider.min.y));
                break;
                //case AXIS.Z:
                //    objs.Sort((a, b) => a.collider.min.z.CompareTo(b.collider.min.z));
        }


        List<AABBPair> pairs = new List<AABBPair>();
        //X Axis
        int i;
        List<ColliderBox> activeList = new List<ColliderBox>();
        activeList.Add(objs[0]);
        for (i = 0; i < objs.Count - 1; i++)
        {
            var tempActive = new List<ColliderBox>();
            if (activeList.Count == 0)
                activeList.Add(objs[i]);
            tempActive.AddRange(activeList);

            foreach (var colliderBox in activeList)
            {


                bool collision = false;
                switch (axis)
                {
                    case AXIS.X:
                        if (colliderBox.collider.max.x < objs[i + 1].collider.min.x)
                            collision = false;
                        else
                        {
                            collision = true;
                        }
                        break;
                    case AXIS.Y:
                        if (colliderBox.collider.max.y < objs[i + 1].collider.min.y)
                            collision = false;
                        else
                        {
                            collision = true;
                        }
                        break;
                }
                if (!collision)
                {
                    tempActive.Remove(colliderBox);
                }
                else
                {
                    tempActive.Add(objs[i + 1]);
                    AABBPair p = new AABBPair();
                    p.object1 = colliderBox;
                    p.object2 = objs[i + 1];
                    if (!pairs.Contains(p) && !pairs.Contains(new AABBPair() { object1 = p.object2, object2 = p.object1 }))
                        pairs.Add(p);

                }
            }
            activeList.Clear();
            activeList.AddRange(tempActive);
        }
        return pairs;
    }
}