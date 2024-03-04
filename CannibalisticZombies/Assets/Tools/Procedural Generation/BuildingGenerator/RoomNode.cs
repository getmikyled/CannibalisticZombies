using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public enum Direction
    {
        None = 0,
        North = 1,
        South = -1,
        East = 2,
        West = -2
    }

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
        Empty = 10,
        Boundary = 11
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
        public RoomNode(Direction argDirection, RoomNode adjacentRoom)
        {
            roomType = RoomType.Boundary;
            adjacentRooms = new Dictionary<RoomNode, WallType>();
            switch (argDirection)
            {
                case Direction.North:
                    floorPos = new Vector2Int(adjacentRoom.floorPos.x, adjacentRoom.floorPos.y + 1);
                    break;
                case Direction.South:
                    floorPos = new Vector2Int(adjacentRoom.floorPos.x, adjacentRoom.floorPos.y - 1);
                    break;
                case Direction.East:
                    floorPos = new Vector2Int(adjacentRoom.floorPos.x + 1, adjacentRoom.floorPos.y);
                    break;
                case Direction.West:
                    floorPos = new Vector2Int(adjacentRoom.floorPos.x - 1, adjacentRoom.floorPos.y);
                    break;
            }
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
        public Direction GetAdjacentRoomDirection(RoomNode room)
        {
            int xDirection = room.floorPos.x - floorPos.x;
            int yDirection = room.floorPos.y - floorPos.y;

            if (xDirection > 0)
            {
                return Direction.East;
            }
            else if (xDirection < 0)
            {
                return Direction.West;
            }
            else if (yDirection > 0)
            {
                return Direction.North;
            }
            else if (yDirection < 0)
            {
                return Direction.South;
            }
            return Direction.None;
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
            if (argRoom == null) return;
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
            return doorCount > 0;
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
            Debug.Log(HasDoor() + " " + HasDoorsInAdjacentRooms());
            return HasDoor() == false && HasDoorsInAdjacentRooms() == false;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public int GetVerticesCount()
        {
            int vertCount = 0;
            foreach (WallType wallType in adjacentRooms.Values)
            {
                switch (wallType)
                {
                    case WallType.Wall:
                        vertCount += 4;
                        break;
                    case WallType.Door:
                        vertCount += 9;
                        break;
                    case WallType.SecondaryDoor:
                        vertCount += 9;
                        break;
                    case WallType.Empty:
                        break;
                    case WallType.Entrance:
                        vertCount += 9;
                        break;
                }
            }
            return vertCount;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public int GetTrianglesCount()
        {
            int triangleCount = 0;
            foreach (WallType wallType in adjacentRooms.Values)
            {
                switch (wallType)
                {
                    case WallType.Wall:
                        triangleCount += 2;
                        break;
                    case WallType.Door:
                        triangleCount += 7;
                        break;
                    case WallType.SecondaryDoor:
                        triangleCount += 7;
                        break;
                    case WallType.Empty:
                        break;
                    case WallType.Entrance:
                        triangleCount += 7;
                        break;
                }
            }
            return triangleCount;
        }
    }
}