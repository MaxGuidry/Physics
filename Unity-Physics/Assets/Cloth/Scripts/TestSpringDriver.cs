using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cloth;
using UnityEngine;
using UnityEngine.Audio;

public class TestSpringDriver : MonoBehaviour
{
    public GameObject particlePrefab;
    public int rowscols;
    public float ks;

    public float RestCoefficient;
    public float kd;
    public Vector3 Wind;
    public bool GustyWind;
    private List<SpringDamperBehavior> sdbs = new List<SpringDamperBehavior>();
    private List<ParticleBehavior> parts = new List<ParticleBehavior>();
    private List<Triangle> tris = new List<Triangle>();
    // Use this for initialization
    void Start()
    {
        CreateCloth();
        sdbs = FindObjectsOfType<SpringDamperBehavior>().ToList();
        for (int i = 0; i < (rowscols * rowscols) - rowscols; i++)
        {
            if (i % rowscols == 0)
            {
                tris.Add(new Triangle(parts[i].p, parts[i + 1].p, parts[i + rowscols].p));
            }
            else if ((i + 1) % rowscols == 0)
            {
                tris.Add(new Triangle(parts[i].p, parts[i + rowscols].p, parts[i + rowscols - 1].p));
            }
            else
            {
                tris.Add(new Triangle(parts[i].p, parts[i + rowscols].p, parts[i + rowscols - 1].p));
                tris.Add(new Triangle(parts[i].p, parts[i + rowscols].p, parts[i + rowscols + 1].p));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var sd in sdbs)
        {
            sd.springConstant = ks;
            sd.restCoefficient = RestCoefficient;
            sd.Kd = kd;
        }
        foreach (var pb in parts)
        {
            if (pb.gravity)
                pb.p.AddForce(new Vector3(0, -9.81f, 0) * 1f);
        }
        foreach (var triangle in tris)
        {
            if (GustyWind)
                triangle.AreodynamicForce(GustWind());
            else
                triangle.AreodynamicForce(Wind);
        }
        List<SpringDamperBehavior> removes = new List<SpringDamperBehavior>();
        foreach (var sdb in sdbs)
        {
            sdb.spring(ks, sdb.sd.l);
            if (sdb.Break())
            {
                removes.Add(sdb);
            }
        }
        foreach (var r in removes)
        {
            if (sdbs.Contains(r))
                sdbs.Remove(r);
            DestroyImmediate(r.gameObject);
        }
        foreach (var triangle in tris)
        {

            triangle.CheckCollision();
        }
        foreach (var pb in parts)
        {
            pb.UpdateParticle();
        }

    }

    void CreateCloth()
    {

        ParticleBehavior[] ps = new ParticleBehavior[(int)(rowscols * rowscols)];
        for (int i = 0; i < rowscols; i++)
        {
            for (int j = 0; j < rowscols; j++)
            {
                GameObject go =
                    GameObject.Instantiate(particlePrefab, new Vector3((float)i * RestCoefficient, 10, (float)j * RestCoefficient), Quaternion.identity);
                if (go == null)
                    Debug.Break();
                DestroyImmediate(go.GetComponent<Renderer>());
                ParticleBehavior pb = go.GetComponent<ParticleBehavior>();
                if (pb == null)
                    Debug.Break();

                ps[(i * rowscols) + j] = pb;
            }
        }
        parts.AddRange(ps);
        // for (int i = 0; i < ((rowscols * rowscols) + (rowscols * (rowscols - 2))); i++)
        for (int i = 0; i < (rowscols * rowscols) - 1; i++)
        {
            if (i < rowscols)
                parts[i].isAnchor = true;
            //parts[0].isAnchor = true;
            //parts[rowscols - 1].isAnchor = true;
            //parts[rowscols * rowscols - rowscols].isAnchor = true;
            //parts[rowscols * rowscols - 1].isAnchor = true;
            if (i > (rowscols * (rowscols - 1)) - 1)
            {
                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + 1];
            }
            else if (i % rowscols == 0)
            {
                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + 1];
                GameObject go2 = new GameObject();
                SpringDamperBehavior sdb2 = go2.AddComponent<SpringDamperBehavior>();
                //sdb2.dot = true;
                sdb2.a = parts[i];
                sdb2.b = parts[i + 1 + rowscols];
                GameObject go3 = new GameObject();
                SpringDamperBehavior sdb3 = go3.AddComponent<SpringDamperBehavior>();
                //sdb3.dot = true;
                sdb3.a = parts[i];
                sdb3.b = parts[i + rowscols];
            }
            else if ((i + 1) % rowscols == 0)
            {
                // parts[i].isAnchor = true;
                // parts[i].anchorx = true;

                // parts[i].anchorz = true;

                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + rowscols];
                GameObject go2 = new GameObject();
                SpringDamperBehavior sdb2 = go2.AddComponent<SpringDamperBehavior>();
                //sdb2.dot = true;
                sdb2.a = parts[i];
                sdb2.b = parts[i - 1 + rowscols];
            }
            else
            {

                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + 1];
                GameObject go2 = new GameObject();
                SpringDamperBehavior sdb2 = go2.AddComponent<SpringDamperBehavior>();
                //sdb2.dot = true;
                sdb2.a = parts[i];
                sdb2.b = parts[i + 1 + rowscols];
                GameObject go3 = new GameObject();
                SpringDamperBehavior sdb3 = go3.AddComponent<SpringDamperBehavior>();
                //sdb3.dot = true;
                sdb3.a = parts[i];
                sdb3.b = parts[i + rowscols];
                GameObject go4 = new GameObject();
                SpringDamperBehavior sdb4 = go4.AddComponent<SpringDamperBehavior>();
                //sdb4.dot = true;
                sdb4.a = parts[i];
                sdb4.b = parts[i + rowscols - 1];
            }
            //sdb.a = parts[i % rc];
            //sdb.b = parts[i];
            if ((i + 2 < rowscols * rowscols) && (i + 2) % rowscols > i % rowscols)
            {
                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + 2];
            }
            if ((i + rowscols * 2f < rowscols * rowscols))
            {
                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + rowscols * 2];
            }
        }
        //parts[parts.Count - 1].isAnchor = true;
        //parts[parts.Count - 1].anchorz = true;
        //parts[parts.Count - 1].anchorx = true;
    }

    private bool Gust;
    private float counter = 0;
    public Vector3 GustWind()
    {
        Vector3 wind = Wind;
        float r = Random.Range(1, 1001);
        if (r > 999)
            Gust = true;
        if (Gust)
        {

            wind *= Random.Range(3, 12);
            counter += Time.deltaTime;
        }

        wind += new Vector3(Random.Range(-2, 2), Random.Range(-1, 1), Random.Range(-2, 2)) * .5f;
        if (counter > .5f)
        {
            counter = 0;
            Gust = false;
        }
        return wind;

    }
}
