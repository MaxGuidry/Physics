using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cloth;
using UnityEngine;
using UnityEngine.Audio;
using Cloth = UnityEngine.Cloth;

public class TestSpringDriver : MonoBehaviour
{
    public GameObject particlePrefab;
    public float rowscols;
    private float currentRowsCols;
    public float ks;

    public float RestCoefficient;
    public float kd;
    public float Windx, Windy, Windz;
    [HideInInspector]
    public Vector3 Wind;
    public bool GustyWind;
    private List<SpringDamperBehavior> sdbs = new List<SpringDamperBehavior>();
    private List<ParticleBehavior> parts = new List<ParticleBehavior>();
    private List<Triangle> tris = new List<Triangle>();

    public Material cloth;

    private GameObject clothGO;
    // Use this for initialization
    void Start()
    {
        currentRowsCols = rowscols;
        CreateCloth();
        Triangles();
        mesh();
    }

    public void ResetCloth()
    {
        foreach (var particleBehavior in parts)
        {
            DestroyImmediate(particleBehavior.gameObject);
        }
        foreach (var sd in sdbs)
        {
            DestroyImmediate(sd.gameObject);
        }
        tris.Clear();
        parts.Clear();
        sdbs.Clear();
        CreateCloth();

        Triangles();
        foreach (var sd in sdbs)
        {
            sd.Init();
        }
        mesh();
        currentRowsCols = rowscols;

    }
    void Triangles()
    {
        sdbs = FindObjectsOfType<SpringDamperBehavior>().ToList();
        for (int i = 0; i < ((int)(int)rowscols * (int)(int)rowscols) - (int)(int)rowscols; i++)
        {
            if (i % (int)(int)rowscols == 0)
            {
                tris.Add(new Triangle(parts[i].p, parts[i + 1].p, parts[i + (int)(int)rowscols].p));
            }
            else if ((i + 1) % (int)(int)rowscols == 0)
            {
                tris.Add(new Triangle(parts[i].p, parts[i + (int)(int)rowscols].p, parts[i + (int)(int)rowscols - 1].p));
            }
            else
            {
                tris.Add(new Triangle(parts[i].p, parts[i + (int)(int)rowscols].p, parts[i + (int)(int)rowscols - 1].p));
                tris.Add(new Triangle(parts[i].p, parts[i + (int)(int)rowscols].p, parts[i + (int)(int)rowscols + 1].p));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Wind.x = Windx;
        Wind.y = Windy;
        Wind.z = Windz;
        foreach (var sd in sdbs)
        {
            SpringDamperBehavior.springConstant = ks;
            sd.restCoefficient = RestCoefficient;
            sd.Kd = kd;
        }
        foreach (var pb in parts)
        {
            if (pb.gravity)
                pb.p.AddForce(new Vector3(0, -9.81f, 0) * 1f);
        }
        Vector3 wind = (GustyWind) ? GustWind() : Wind;
        foreach (var triangle in tris)
        {
            if (GustyWind)
                triangle.AreodynamicForce(wind);
            else
                triangle.AreodynamicForce(wind);
        }
        List<SpringDamperBehavior> removes = new List<SpringDamperBehavior>();
        foreach (var sdb in sdbs)
        {
            sdb.spring(ks, sdb.sd.l);
            // if (sdb.Break())
            //  {
            //     removes.Add(sdb);
            // }
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
        UpdateMesh();
    }

    void UpdateMesh()
    {
        Mesh m = clothGO.GetComponent<MeshFilter>().mesh;
        var vertices = new Vector3[(int)currentRowsCols * (int)currentRowsCols];
        var colors = new Color[(int)currentRowsCols * (int)currentRowsCols];

        for (int i = 0; i < (int)currentRowsCols * (int)currentRowsCols; ++i)
        {
            vertices[i] = parts[i].p.position;
            colors[i] = Color.blue;
        }
        m.vertices = vertices;
        m.colors = colors;
        m.RecalculateNormals();
        m.RecalculateTangents();
    }
    void CreateCloth()
    {

        ParticleBehavior[] ps = new ParticleBehavior[(int)((int)(int)rowscols * (int)(int)rowscols)];
        for (int i = 0; i < (int)(int)rowscols; i++)
        {
            for (int j = 0; j < (int)(int)rowscols; j++)
            {
                GameObject go =
                    GameObject.Instantiate(particlePrefab, new Vector3((float)i * RestCoefficient, (float)j * RestCoefficient,0f), Quaternion.identity);
                if (go == null)
                    Debug.Break();
                DestroyImmediate(go.GetComponent<Renderer>());
                ParticleBehavior pb = go.GetComponent<ParticleBehavior>();
                if (pb == null)
                    Debug.Break();

                ps[(i * (int)(int)rowscols) + j] = pb;
            }
        }
        parts.AddRange(ps);
        // for (int i = 0; i < (((int)(int)rowscols * (int)(int)rowscols) + ((int)(int)rowscols * ((int)(int)rowscols - 2))); i++)
        for (int i = 0; i < ((int)(int)rowscols * (int)(int)rowscols) - 1; i++)
        {
              if (i < (int)(int)rowscols)
                 parts[i].isAnchor = true;
            //parts[0].isAnchor = true;
            //parts[(int)(int)rowscols - 1].isAnchor = true;
            //parts[(int)(int)rowscols * (int)(int)rowscols - (int)(int)rowscols].isAnchor = true;
            //parts[(int)(int)rowscols * (int)(int)rowscols - 1].isAnchor = true;
            if (i > ((int)(int)rowscols * ((int)(int)rowscols - 1)) - 1)
            {
                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + 1];
            }
            else if (i % (int)(int)rowscols == 0)
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
                sdb2.b = parts[i + 1 + (int)(int)rowscols];
                GameObject go3 = new GameObject();
                SpringDamperBehavior sdb3 = go3.AddComponent<SpringDamperBehavior>();
                //sdb3.dot = true;
                sdb3.a = parts[i];
                sdb3.b = parts[i + (int)(int)rowscols];
            }
            else if ((i + 1) % (int)(int)rowscols == 0)
            {
                //parts[i].isAnchor = true;
                // parts[i].anchorx = true;

                // parts[i].anchorz = true;

                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + (int)(int)rowscols];
                GameObject go2 = new GameObject();
                SpringDamperBehavior sdb2 = go2.AddComponent<SpringDamperBehavior>();
                //sdb2.dot = true;
                sdb2.a = parts[i];
                sdb2.b = parts[i - 1 + (int)(int)rowscols];
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
                sdb2.b = parts[i + 1 + (int)(int)rowscols];
                GameObject go3 = new GameObject();
                SpringDamperBehavior sdb3 = go3.AddComponent<SpringDamperBehavior>();
                //sdb3.dot = true;
                sdb3.a = parts[i];
                sdb3.b = parts[i + (int)(int)rowscols];
                GameObject go4 = new GameObject();
                SpringDamperBehavior sdb4 = go4.AddComponent<SpringDamperBehavior>();
                //sdb4.dot = true;
                sdb4.a = parts[i];
                sdb4.b = parts[i + (int)(int)rowscols - 1];
            }
            //sdb.a = parts[i % rc];
            //sdb.b = parts[i];
            if ((i + 2 < (int)(int)rowscols * (int)(int)rowscols) && (i + 2) % (int)(int)rowscols > i % (int)(int)rowscols)
            {
                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + 2];
            }
            if ((i + (int)(int)rowscols * 2f < (int)(int)rowscols * (int)(int)rowscols))
            {
                GameObject go = new GameObject();
                SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
                //sdb.dot = true;
                sdb.a = parts[i];
                sdb.b = parts[i + (int)(int)rowscols * 2];
            }
        }
        //parts[parts.Count - 1].isAnchor = true;
        //parts[parts.Count - 1].anchorz = true;
        //parts[parts.Count - 1].anchorx = true;
    }

    private bool Gust;
    private float counter = 1;
    public Vector3 GustWind()
    {
        Vector3 wind = Wind;
        float r = Random.Range(1, 301);
        if (r > 299 && !Gust)
        {
            Gust = true;
            //Debug.Log("Gust");
        }
        if (Gust)
        {

            counter += Time.deltaTime;
        }
        else if (counter > 1)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            counter = 1;
        }
        //wind += new Vector3(Random.Range(-2, 2), Random.Range(-1, 1), Random.Range(-2, 2)) * .5f;
        wind *= Random.Range(1, 2) * counter;

        if (counter > 2f)
        {
            Gust = false;
            //Debug.Log("Stop Gust");
        }
        return wind;

    }
    void mesh()
    {
        if (clothGO)
            DestroyImmediate(clothGO);
        clothGO = new GameObject();
        clothGO.name = "Cloth";
        Mesh m = new Mesh();
        m.name = "Max";

        var vertices = new Vector3[(int)rowscols * (int)rowscols];
        var colors = new Color[(int)rowscols * (int)rowscols];

        for (int i = 0; i < (int)rowscols * (int)rowscols; ++i)
        {
            vertices[i] = parts[i].p.position;
            colors[i] = Color.blue;
        }
        m.vertices = vertices;
        m.colors = colors;
        int index = 0;
        var triangles = new int[((int)rowscols - 1) * ((int)rowscols - 1) * 6];

        for (int r = 0; r < (rowscols - 1); ++r)
            for (int c = 0; c < (rowscols - 1); ++c)
            {
                triangles[index++] = (r + 1) * (int)rowscols + (c + 1);
                triangles[index++] = (r + 1) * (int)rowscols + c;
                triangles[index++] = r * (int)rowscols + c;

                triangles[index++] = r * (int)rowscols + (c + 1);
                triangles[index++] = (r + 1) * (int)rowscols + (c + 1);
                triangles[index++] = r * (int)rowscols + c;
            }


        m.triangles = triangles;
        var normals = new Vector3[m.vertices.Length];

        for (int i = 0; i < m.vertices.Length; i++)
        {
            normals[i] = new Vector3(1, 0, 0);
        }
        m.normals = normals;

        index = 0;
        var uv = new Vector2[(int)rowscols * (int)rowscols];
        for (int i = 0; i < rowscols; i++)
            for (int j = 0; i < rowscols; i++)
            {
                uv[index++] = -new Vector2((float)i / rowscols, (float)j / rowscols);
            }
        m.uv = uv;
        MeshRenderer mr = clothGO.AddComponent<MeshRenderer>();
        MeshFilter mf = clothGO.AddComponent<MeshFilter>();
        mr.material = cloth;
        mf.mesh = m;


    }
}
