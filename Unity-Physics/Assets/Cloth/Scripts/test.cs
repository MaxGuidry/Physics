using System.Collections;
using System.Collections.Generic;
using Cloth;
using UnityEditorInternal;
using UnityEngine;

public class test : MonoBehaviour
{

    public int rowscols;

    public GameObject particlePrefab;
    // Use this for initialization
    void Awake()
    {
        ParticleBehavior[] parts = new ParticleBehavior[(int)(rowscols * rowscols)];
        for (int i = 0; i < rowscols; i++)
        {
            for (int j = 0; j < rowscols; j++)
            {
                GameObject go =
                    GameObject.Instantiate(particlePrefab, new Vector3((float)i/.5f, (float)j/.5f , 0), Quaternion.identity);
                if (go == null)
                    Debug.Break();
                //DestroyImmediate(go.GetComponent<Renderer>());
                ParticleBehavior pb = go.GetComponent<ParticleBehavior>();
                if (pb == null)
                    Debug.Break();

                parts[(i * rowscols) + j] = pb;
            }
        }

        // for (int i = 0; i < ((rowscols * rowscols) + (rowscols * (rowscols - 2))); i++)
        for (int i = 0; i < (rowscols * rowscols) - 1; i++)
        {
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
                parts[i].isAnchor = true;
                parts[i].anchorx = true;

                parts[i].anchorz = true;

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
        parts[parts.Length - 1].isAnchor = true;
        parts[parts.Length - 1].anchorz = true;
        parts[parts.Length - 1].anchorx = true;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
