using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Recorder.OutputPath;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public class BuildingGenerator
    {
        // Building Properties
        private int gridWidth;
        private int gridHeight;

        public FloorNode[] floors;

        public RoomType[] roomsList;
        public int roomCount;
        public int roomIndex = 0;
        public RoomNode rootNode;
        
        ///-///////////////////////////////////oot
        public BuildingGenerator(int argGridWidth, int argGridHeight, int argFloorCount)
        {
            gridWidth = argGridWidth;
            gridHeight = argGridHeight;
            floors = new FloorNode[argFloorCount];

            roomCount = argFloorCount * gridWidth * gridHeight - (2 * argFloorCount - 1); // Accounts for placing stairs rooms
            roomsList = new RoomType[roomCount];
            GenerateRoomPool();

            // GENERATE FLOORS
            roomIndex = 0;
            for (int i = 0; i < floors.Length; i++)
            {
                floors[i] = GenerateFloor(i);
            }

            // IDENTIFY ROOT NODE
            bool foundRootNode = false;
            for (int i = 0; i < gridHeight; i++)
            {
                if (foundRootNode) break;
                for (int j = 0; j < gridWidth; j++)
                {
                    if (floors[0].rooms[i, j].roomType != RoomType.Stairs && floors[0].rooms[i, j].roomType != RoomType.Basement)
                    {
                        rootNode = floors[0].rooms[i, j];
                        foundRootNode = true;
                        break;
                    }
                }
            }

            DetermineRoomsWalls();
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private FloorNode GenerateFloor(int argFloorNumber)
        {
            Debug.Log("Generate Floor: " + argFloorNumber);
            FloorNode floor = new FloorNode(gridWidth, gridHeight, argFloorNumber);

            Vector2Int downStairsNode = (argFloorNumber > 0) ? floors[argFloorNumber - 1].stairsNode.floorPos : new Vector2Int(-1, -1);
            bool hasUpstairs = argFloorNumber < floors.Length - 1;

            floor.GenerateRooms(downStairsNode, hasUpstairs, this);

            return floor;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void GenerateRoomPool()
        {
            int shuffleStartIndex = 1;
            if (roomCount == 3)
            {
                AddRoom(RoomType.Kitchen);
                AddRoom(RoomType.Bedroom);
                AddRoom(RoomType.Bathroom);
                return;
            }

            // ADD INITIAL ROOMS
            // Generate Root Node
            List<RoomType> rootRoomTypes = new List<RoomType>()
            {
                RoomType.Kitchen, RoomType.DiningRoom, RoomType.LivingRoom
            };
            int i = Random.Range(0, rootRoomTypes.Count);
            AddRoom(rootRoomTypes[i]);
            rootRoomTypes.RemoveAt(i);

            // Add remaining rooms
            foreach(RoomType roomType in rootRoomTypes)
            {
                AddRoom(roomType);
            }
            AddRoom(RoomType.Bedroom);
            AddRoom(RoomType.Bathroom);

            while (roomIndex < roomCount) {
                AddRoom(RoomNode.RandomRoomType());
            }

            roomsList.Shuffle(shuffleStartIndex);
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void DetermineRoomsWalls()
        {
            foreach (FloorNode floor in floors)
            {
                foreach (RoomNode room in floor.rooms)
                {
                    RoomNode northRoom = floor.GetAdjacentRoom(Direction.North, room.floorPos);
                    SetRoomWalls(Direction.North, room, northRoom);
                    RoomNode southRoom = floor.GetAdjacentRoom(Direction.South, room.floorPos);
                    SetRoomWalls(Direction.South, room, southRoom);
                    RoomNode eastRoom = floor.GetAdjacentRoom(Direction.East, room.floorPos);
                    SetRoomWalls(Direction.East, room, eastRoom);
                    RoomNode westRoom = floor.GetAdjacentRoom(Direction.West, room.floorPos);
                    SetRoomWalls(Direction.West, room, westRoom);
                }

                // Room connection error check
                foreach (RoomNode room in floor.rooms)
                {
                    if (room.CheckForEntranceConnection() == false)
                    {
                        RoomNode northRoom = floor.GetAdjacentRoom(Direction.North, room.floorPos);
                        RoomNode southRoom = floor.GetAdjacentRoom(Direction.South, room.floorPos);
                        RoomNode eastRoom = floor.GetAdjacentRoom(Direction.East, room.floorPos);
                        RoomNode westRoom = floor.GetAdjacentRoom(Direction.West, room.floorPos);

                        if (southRoom != null)
                        {
                            SetConnectionBetweenRooms(WallType.SecondaryDoor, room, southRoom);
                            Debug.Log("FIXED: " + room.roomType.ToString() + " SOUTH");
                        }
                        else if (northRoom != null)
                        {
                            SetConnectionBetweenRooms(WallType.SecondaryDoor, room, northRoom);
                            Debug.Log("FIXED: " + room.roomType.ToString() + " NORTH");
                        }
                        else if (eastRoom != null)
                        {
                            SetConnectionBetweenRooms(WallType.SecondaryDoor, room, eastRoom);
                            Debug.Log("FIXED: " + room.roomType.ToString() + " EAST");
                        }
                        else if (westRoom != null)
                        {
                            SetConnectionBetweenRooms(WallType.SecondaryDoor, room, westRoom);
                            Debug.Log("FIXED: " + room.roomType.ToString() + " WEST");
                        }
                    }
                }

            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void SetRoomWalls(Direction argDirection, RoomNode currentRoom, RoomNode adjacentRoom)
        {
            WallType wallType = DetermineWallType(currentRoom, adjacentRoom);
            if (adjacentRoom != null)
            {
                SetConnectionBetweenRooms(wallType, currentRoom, adjacentRoom);
                //Debug.Log(currentRoom.roomType + " " + adjacentRoom.roomType + " " + wallType + " " + argDirection);
            }
            else
            {
                RoomNode newRoom = new RoomNode(argDirection, currentRoom);
                SetConnectionBetweenRooms(wallType, currentRoom, newRoom);
                //Debug.Log(currentRoom.roomType + " " + newRoom.roomType + " " + wallType + " " + argDirection);
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void SetConnectionBetweenRooms(WallType argWallType, RoomNode room1, RoomNode room2)
        {
            room1.SetAdjacentRoom(room2, argWallType);
            room2.SetAdjacentRoom(room1, argWallType);
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void AddRoom(RoomType argRoomType)
        {
            roomsList[roomIndex] = argRoomType;
            roomIndex++;
        }

        #region Determine Wall Type
        ///-////////////////////////////////////////////////////////////////////
        ///
        private WallType DetermineWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            // If current room is root node and doesnt have the entrance set yet.
            if (currentRoom == rootNode && currentRoom.HasEntrance() == false && adjacentRoom == null)
            {
                currentRoom.connectedToEntrance = true;
                return WallType.Entrance;
            }

            // If adjacent room is boundary
            if (adjacentRoom == null)
            {
                return WallType.Wall;
            }

            // Check if adjacent room is already adjacent
            if (currentRoom.IsAdjacentTo(adjacentRoom))
            {
                return currentRoom.GetAdjacentRoomWallType(adjacentRoom);
            }

            switch (currentRoom.roomType)
            {
                case RoomType.Bedroom:
                    return DetermineBedroomWallType(currentRoom, adjacentRoom);
                case RoomType.Bathroom:
                    return DetermineBathroomWallType(currentRoom, adjacentRoom);
                case RoomType.Kitchen:
                    return DetermineKitchenWallType(currentRoom, adjacentRoom);
                case RoomType.DiningRoom:
                    return DetermineDiningRoomWallType(currentRoom, adjacentRoom);
                case RoomType.LivingRoom:
                    return DetermineLivingRoomWallType(currentRoom, adjacentRoom);
                case RoomType.Office:
                    return DetermineOfficeWallType(currentRoom, adjacentRoom);
                case RoomType.Hallway:
                    return DetermineHallwayWallType(currentRoom, adjacentRoom);
                case RoomType.Stairs:
                    return DetermineStairsWallType(currentRoom, adjacentRoom);
                case RoomType.Empty:
                    return DetermineEmptyWallType(currentRoom, adjacentRoom);
                case RoomType.Basement:
                    return DetermineBasementWallType(currentRoom, adjacentRoom);
                default:
                    return WallType.Wall;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private WallType DetermineBedroomWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            switch (adjacentRoom.roomType)
            {
                case RoomType.Bedroom:
                    return WallType.Empty;
                case RoomType.Bathroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasSecondaryDoor()) return WallType.Wall;
                    return WallType.SecondaryDoor;
                case RoomType.Kitchen:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.DiningRoom:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.LivingRoom:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Hallway:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Office:
                    return WallType.Empty;
                case RoomType.Stairs:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Empty:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Basement:
                    return WallType.Wall;
                default: 
                    return WallType.Wall;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private WallType DetermineBathroomWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            if (currentRoom.HasDoor() || currentRoom.HasSecondaryDoor())
            {
                return WallType.Wall;
            }

            switch (adjacentRoom.roomType)
            {
                case RoomType.Bedroom:
                    return WallType.Empty;
                case RoomType.Bathroom:
                    return WallType.SecondaryDoor;
                case RoomType.Kitchen:
                    return WallType.Door;
                case RoomType.DiningRoom:
                    return WallType.Door;
                case RoomType.LivingRoom:
                    return WallType.Door;
                case RoomType.Hallway:
                    return WallType.Door;
                case RoomType.Office:
                    return WallType.Empty;
                case RoomType.Stairs:
                    return WallType.Door;
                case RoomType.Empty:
                    return WallType.Door;
                case RoomType.Basement:
                    return WallType.Wall;
                default:
                    return WallType.Wall;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineKitchenWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            switch (adjacentRoom.roomType)
            {
                case RoomType.Bedroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Bathroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Kitchen:
                    return WallType.Empty;
                case RoomType.DiningRoom:
                    return WallType.Empty;
                case RoomType.LivingRoom:
                    return WallType.Empty;
                case RoomType.Hallway:
                    return WallType.Empty;
                case RoomType.Office:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Stairs:
                    return WallType.Door;
                case RoomType.Empty:
                    return WallType.Door;
                case RoomType.Basement:
                    if (adjacentRoom.HasSecondaryDoor()) return WallType.Wall;
                    return WallType.SecondaryDoor;
                default:
                    return WallType.Wall;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineDiningRoomWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            switch (adjacentRoom.roomType)
            {
                case RoomType.Bedroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Bathroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Kitchen:
                    return WallType.Empty;
                case RoomType.DiningRoom:
                    return WallType.Empty;
                case RoomType.LivingRoom:
                    return WallType.Empty;
                case RoomType.Office:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Hallway:
                    return WallType.Empty;
                case RoomType.Stairs:
                    return WallType.Door;
                case RoomType.Empty:
                    return WallType.Door;
                case RoomType.Basement:
                    if (adjacentRoom.HasSecondaryDoor()) return WallType.Wall;
                    return WallType.SecondaryDoor;
                default:
                    return WallType.Wall;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineLivingRoomWallType(RoomNode argCurrentRoom, RoomNode adjacentRoom)
        {
            switch (adjacentRoom.roomType)
            {
                case RoomType.Bedroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Bathroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Kitchen:
                    return WallType.Empty;
                case RoomType.DiningRoom:
                    return WallType.Empty;
                case RoomType.LivingRoom:
                    return WallType.Empty;
                case RoomType.Office:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Hallway:
                    return WallType.Empty;
                case RoomType.Stairs:
                    return WallType.Door;
                case RoomType.Empty:
                    return WallType.Door;
                case RoomType.Basement:
                    if (adjacentRoom.HasSecondaryDoor()) return WallType.Wall;
                    return WallType.SecondaryDoor;
                default:
                    return WallType.Wall;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineOfficeWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            switch (adjacentRoom.roomType)
            {
                case RoomType.Bedroom:
                    return WallType.Empty;
                case RoomType.Bathroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.SecondaryDoor;
                case RoomType.Kitchen:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.DiningRoom:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.LivingRoom:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Office:
                    return WallType.Empty;
                case RoomType.Hallway:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Stairs:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Empty:
                    if (currentRoom.HasDoor() || currentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Basement:
                    return WallType.Wall;
                default:
                    return WallType.Wall;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineHallwayWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            switch (adjacentRoom.roomType)
            {
                case RoomType.Bedroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Bathroom:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Kitchen:
                    return WallType.Empty;
                case RoomType.DiningRoom:
                    return WallType.Empty;
                case RoomType.LivingRoom:
                    return WallType.Empty;
                case RoomType.Office:
                    if (adjacentRoom.HasDoor() || adjacentRoom.HasDoorsInAdjacentRooms()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Hallway:
                    return WallType.Empty;
                case RoomType.Stairs:
                    return WallType.Door;
                case RoomType.Empty:
                    return WallType.Door;
                case RoomType.Basement:
                    if (adjacentRoom.HasSecondaryDoor()) return WallType.Wall;
                    return WallType.SecondaryDoor;
                default:
                    return WallType.Wall;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineStairsWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            return WallType.Door;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineEmptyWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            return WallType.Door;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineBasementWallType(RoomNode currentRoom, RoomNode adjacentRoom)
        {
            if (currentRoom.HasSecondaryDoor())
            {
                return WallType.Wall;
            }
            return WallType.SecondaryDoor;
        }

        #endregion // Determine Wall Type
    }

}