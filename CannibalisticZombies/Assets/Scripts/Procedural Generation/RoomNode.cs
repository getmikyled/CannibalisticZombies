using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public enum RoomType
    {
        Bedroom = 0,
        Bathroom = 1,
        Kitchen = 2,
        DiningRoom = 3,
        LivingRoom = 4,
        Office = 5,
        Hallway = 6,
        Garage = 7,
        Stairs = 8,
        Basement = 9,
        Empty = 10
    }

    ///-////////////////////////////////////////////////////////////////////
    ///
    public class RoomNode : Node
    {
        public RoomType roomType;

        public int floorNum { get; private set; }
        public Vector2Int floorPos { get; private set; }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public RoomNode(RoomType argRoomType)
        {
            roomType = argRoomType;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public static RoomType RandomRoomType()
        {
            return (RoomType) Random.Range(0, 7); // Doesn't inckude: Garage
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public void SetPos(Vector2Int argPos)
        {
            floorPos = argPos;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public void SetFloor(int argFloorNum)
        {
            floorNum = argFloorNum;
        }


    }

}