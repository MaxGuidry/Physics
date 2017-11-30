using System.Collections;
using System.Collections.Generic;
using Cloth;
using UnityEngine;

public class test : MonoBehaviour
{

    public int numberparts;

    public GameObject particlePrefab;
    // Use this for initialization
    void Start()
    {
        ParticleBehavior[][] parts = new ParticleBehavior[(int) Mathf.Sqrt(numberparts)][];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject go =
                    GameObject.Instantiate(particlePrefab, new Vector3(i / 10f, j / 10f, 0), Quaternion.identity);
                parts[i][j]=(go.GetComponent<Cloth.ParticleBehavior>());
            }
        }
      
        for (int i = 0; i < ((numberparts * numberparts) + (numberparts * (numberparts - 2))); i++)
        {
            int rc = (int)Mathf.Sqrt(numberparts);
            GameObject go = new GameObject();
            SpringDamperBehavior sdb = go.AddComponent<SpringDamperBehavior>();
            //sdb.a = parts[i];

            //sdb.b = parts[i];

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
