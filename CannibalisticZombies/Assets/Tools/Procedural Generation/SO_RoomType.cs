using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    [CreateAssetMenu]
    public class SO_RoomType : ScriptableObject
    {
        public RoomType roomType;
        public Material wallMaterial;
        public Material floorMaterial;

        public GameObject[] genericRoomPresets;

        public GameObject[] oneDoorRoomPresets;
        public GameObject[] twoDoorRoomPresets;
        public GameObject[] threeDoorRoomPresets;
        public GameObject[] fourDoorRoomPresets;
    }

}