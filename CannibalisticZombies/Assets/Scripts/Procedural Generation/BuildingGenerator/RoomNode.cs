using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public enum WallType
    {
        Wall,
        Door,
        SecondaryDoor,
        EmptyDoor,
        Window,
        Entrance,
        Empty
    }

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

        public Dictionary<RoomNode, WallType> adjacentRooms { get; private set; }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public RoomNode(RoomType argRoomType)
        {
            roomType = argRoomType;
            adjacentRooms = new Dictionary<RoomNode, WallType>();
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

        ///-////////////////////////////////////////////////////////////////////
        ///
        public bool IsAdjacentTo(RoomNode node)
        {
            if (adjacentRooms.ContainsKey(node)) return true;
            return false;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType GetAdjacentRoomWallType(RoomNode room)
        {
            return adjacentRooms[room];
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public void SetAdjacentRoom(RoomNode argRoom, WallType argWallType)
        {
            if (adjacentRooms.ContainsKey(argRoom))
            {
                adjacentRooms[argRoom] = argWallType;
            }
            else
            {
                adjacentRooms.Add(argRoom, argWallType);
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public bool HasDoor()
        {
            int doorCount = 0;
            foreach(RoomNode room in adjacentRooms.Keys)
            {
                if (adjacentRooms[room] == WallType.Door) doorCount++;
            }
            return adjacentRooms.Count > 0;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public bool HasSecondaryDoor()
        {
            int doorCount = 0;
            foreach (RoomNode room in adjacentRooms.Keys)
            {
                if (adjacentRooms[room] == WallType.SecondaryDoor) doorCount++;
            }
            return adjacentRooms.Count > 0;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public bool HasDoorsInAdjacentRooms()
        {
            foreach (RoomNode room in adjacentRooms.Keys)
            {
                if (adjacentRooms[room] == WallType.Empty || adjacentRooms[room] == WallType.SecondaryDoor)
                {
                    if (room.HasDoor()) return true;
                }
            }
            return false;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public bool HasEntrance()
        {
            foreach (WallType wallType in adjacentRooms.Values)
            {
                if (wallType == WallType.Entrance) return true;
            }
            return false;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public bool NeedsConnection()
        {
            if ((roomType == RoomType.Bedroom && HasDoor() == false) ||
                (HasDoor() == false && HasSecondaryDoor() == false))
            {
                return true;
            }
            return false;
        }
    }
}