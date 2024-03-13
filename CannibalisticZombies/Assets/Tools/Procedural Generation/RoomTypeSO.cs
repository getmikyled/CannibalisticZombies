using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    [CreateAssetMenu]
    public class RoomTypeSO : ScriptableObject
    {
        public RoomType roomType;
        public Material wallMaterial;
        public Material floorMaterial;

        public GameObject[] genericRoomPresets;

        public GameObject[] oneDoorRoomPresets;
        public GameObject[] threeDoorRoomPresets;
        public GameObject[] fourDoorRoomPresets;
    }

}