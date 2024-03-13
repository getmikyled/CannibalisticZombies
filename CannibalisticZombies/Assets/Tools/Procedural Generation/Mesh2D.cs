using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GetMikyled
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    [CreateAssetMenu]
    public class Mesh2D : ScriptableObject
    {
        ///-////////////////////////////////////////////////////////////////////
        ///
        [System.Serializable]
        public class Vertex
        {
            // Vertex Properties
            public Vector2 point;
            public Vector2 normal;
            public float u;
        }

        // MESH
        public Vertex[] vertices;
        public int[] lineIndeces;
    }

}