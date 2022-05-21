using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VVR
{
    public class SphereInverted : MonoBehaviour
    {
        public static void InvertSphere(ref GameObject Sphere)
        {
            Vector3[] normals = Sphere.GetComponent<MeshFilter>().mesh.normals;

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i] * -1;
            }

            Sphere.GetComponent<MeshFilter>().sharedMesh.normals = normals;

            int[] triangles = Sphere.GetComponent<MeshFilter>().sharedMesh.triangles;

            for (int i = 0; i < triangles.Length; i+=3)
            {
                int temp = triangles[i];
                triangles[i] = triangles[i + 2];
                triangles[i + 2] = temp;
            }

            Sphere.GetComponent<MeshFilter>().sharedMesh.triangles = triangles;
        }
    }
}