using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video; 


namespace VVR
{
    public class Player : MonoBehaviour
    {
        public static GameObject Sphere;
        private Renderer Rend;
        private Shader Shader;

        [SerializeField] private Material Mat;
        
        //Happens before the first frame. So, before the first frame of the game, we want to intialize all of the 
        //nodes that we need. Then, with the video Manager script, we initialize all of the video players for the 
        //nodes. So, we would need an adjacency list for the amount of player nodes, and a static array for the cameras
        //Could probably create a flag to show which node we are on. 
        private void Awake()
        {
            //Assure that we are attached to a camera 
            //need to do a fix to switching between cameras for different nodes 
            //For n videos we will need n cameras, enable and disable them depending 
            //on our location
            var Cam = gameObject.GetComponent<Camera>();
            Cam.transform.localPosition = Vector3.zero;
            Cam.transform.localRotation = Quaternion.identity;
            Cam.fieldOfView = 90.0f; 
            
            //Initalize our Sphere Object 
            Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Sphere.transform.position = Vector3.zero;
            Sphere.transform.localScale = new Vector3(15, 15, 15);

            //Invert our sphere
            VVR.SphereInverted.InvertSphere(ref Sphere);

            //Initialize the renderer to our sphere object, and find the shader script to
            //flip the normals of our sphere object. 
            Rend = Sphere.GetComponent<Renderer>();
            Shader = Shader.Find("Flipping Normals");
            Rend.material.shader = Shader;
            Rend.material = Mat;
        }

        public void ChangeTexture(string texture){
            Rend.material.mainTexture = Resources.Load<Texture>(texture) as Texture;
        }

        public void ResetToDefaultMaterial(){
            Rend.material = Mat;
        }
   }
}
