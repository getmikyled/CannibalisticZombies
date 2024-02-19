using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public class BuildingConstructor: MonoBehaviour
    {
        private static int MIN_WIDTH = 2;
        private static int MIN_HEIGHT = 2;

        private void Start()
        {
            BuildingGenerator newBuilding = new BuildingGenerator(2, 2, 2);
        }
    }

}