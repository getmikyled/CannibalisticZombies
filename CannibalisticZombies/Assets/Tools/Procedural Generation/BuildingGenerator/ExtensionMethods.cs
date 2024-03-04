using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public static class ExtensionMethods
    {
        ///-////////////////////////////////////////////////////////////////////
        ///
        public static Vector2Int Randomize(this Vector2Int vector, int xRange, int yRange)
        {
            xRange = Random.Range(0, xRange);
            yRange = Random.Range(0, yRange);

            return new Vector2Int(xRange, yRange);
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public static Vector3[] Rotate(this Vector3[] vertices, Quaternion rotation, Vector3 center)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = rotation * (vertices[i] - center) + center;
            }
            return vertices;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public static void Shuffle<T>(this T[] list, int startingIndex = 1)
        {
            for (int i = startingIndex; i < list.Length; i++)
            {
                int newIndex = Random.Range(startingIndex, list.Length);
                T temp = list[i];
                list[i] = list[newIndex];
                list[newIndex] = temp;
            }
        }
    }

}