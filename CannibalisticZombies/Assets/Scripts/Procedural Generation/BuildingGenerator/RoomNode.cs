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

        public Dictionary<RoomNode, WallType> connectedRooms { get; private set; }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public RoomNode(RoomType argRoomType)
        {
            roomType = argRoomType;
            connectedRooms = new Dictionary<RoomNode, WallType>();
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

        public bool IsConnectedTo(RoomNode node)
        {
            if (connectedRooms.ContainsKey(node)) return true;
            return false;
        }

        public void AddConnection(RoomNode argRoom, WallType argWallType)
        {
            if (connectedRooms.ContainsKey(argRoom)) return;
            connectedRooms.Add(argRoom, argWallType);
        }

        public bool HasDoor()
        {
            int doorCount = 0;
            foreach(RoomNode room in connectedRooms.Keys)
            {
                if (connectedRooms[room] == WallType.Door) doorCount++;
            }
            return connectedRooms.Count > 0;
        }

    }

}