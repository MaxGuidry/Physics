using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Cloth
{

    public class DragMouse : MonoBehaviour
    {
        private ParticleBehavior draggingParticle;

        private Vector3 prevPos;

        private Vector3 selectPos;

        private GameObject representation;

        private bool paused;
        // Use this for initialization
        void Start()
        {
            representation = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            representation.name = "THIS ONE";
            representation.transform.localScale = new Vector3(.2f, .2f, .2f);
            Cursor.visible = true;
            paused = true;
        }

        private Vector3 com = new Vector3();
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Cursor.visible = !Cursor.visible;
                paused = !paused;
            }
            if (paused)
                return;
            this.transform.position -= com;
            com = Vector3.zero;

            foreach (var p in TestSpringDriver.particles)
            {
                com += p.p.position;
            }

            if (TestSpringDriver.particles.Count != 0)
                com /= TestSpringDriver.particles.Count;
            Vector3 centerScreen = new Vector3(Screen.width * .5f, Screen.height * .5f, 0f);
            Vector3 move = new Vector3();
            if (TestSpringDriver.particles.Count != 0)
                move += (4 * new Vector3(Input.GetAxis("Mouse X"), 8 * Input.GetAxis("Mouse Y"), 0f)) / (TestSpringDriver.particles.Count / 3f);
            this.transform.position += com;
            this.transform.position += 10 * move.x * Camera.main.transform.right + move.y * Camera.main.transform.up;
            representation.transform.position = this.transform.position - Camera.main.transform.forward * 10f;
            Vector3 dif = this.transform.position - prevPos;

            if (Input.GetKeyDown(KeyCode.Mouse0))
                Select();
            if (Input.GetKey(KeyCode.Mouse0))
                draggingParticle.p.DragParticle(draggingParticle.p.position + dif);
            else if (draggingParticle != null)
            {
                draggingParticle.isAnchor = false;
            }
            Camera.main.transform.LookAt(this.transform.position);
            prevPos = this.transform.position;
        }

        public void Select()
        {


            Particle p;
            List<ParticleBehavior> parts = new List<ParticleBehavior>(TestSpringDriver.particles);//TestSpringDriver.particles;
            parts.Sort((a, b) => Vector3.Distance(this.transform.position, a.p.position)
                .CompareTo(Vector3.Distance(this.transform.position, b.p.position)));
            draggingParticle = parts[0];
            draggingParticle.isAnchor = true;
            selectPos = this.transform.position;
        }
    }
}