using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public enum WallType
    {
        Null,
        Wall,
        Door,
        EmptyDoor,
        Window,
        Entrance,
        Empty
    }

    ///-////////////////////////////////////////////////////////////////////
    ///
    public class BuildingConstructor : MonoBehaviour
    {
        private static int MIN_WIDTH = 2;
        private static int MIN_HEIGHT = 2;

        [SerializeField] private Vector2Int widthRange;
        [SerializeField] private Vector2Int heightRange;
        [SerializeField] private Vector2Int floorRange;
        [SerializeField] private float roomSize;

        BuildingGenerator building;
        private GameObject buildingObject;

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void Start()
        {
            ConstructBuilding();
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public void ConstructBuilding()
        {
            // GENERATE BUILDING
            int buildingWidth = Random.Range(widthRange.x, widthRange.y + 1);
            int buildingHeight = Random.Range(heightRange.x, heightRange.y + 1);
            int floorCount = Random.Range(floorRange.x, floorRange.y + 1);
            building = new BuildingGenerator(buildingWidth, buildingHeight, floorCount);

            // CONSTRUCT BUILDING
            buildingObject = new GameObject("Building");
            buildingObject.transform.position = transform.position;
            for (int i = 0; i < building.floors.Length; i++)
            {
                GameObject floorObject = new GameObject("Floor_" + i);
                floorObject.transform.parent = buildingObject.transform;
                floorObject.transform.localPosition += new Vector3(0, i * 5, 0);

                foreach (RoomNode room in building.floors[i].rooms)
                {
                    Mesh wallsMesh;
                    GameObject roomObject = ConstructRoomComponents(room, floorObject, out wallsMesh);

                    /////////////////////////
                    ///
                    /// BUILD UP THE WALLS
                    /// 

                    RoomNode adjacentRoom;

                    adjacentRoom = building.floors[i].GetAdjacentRoom(Direction.North, room.floorPos);
                    ConstructWallType(room, adjacentRoom, Direction.North, wallsMesh);
                    adjacentRoom = building.floors[i].GetAdjacentRoom(Direction.South, room.floorPos);
                    ConstructWallType(room, adjacentRoom, Direction.South, wallsMesh);
                    adjacentRoom = building.floors[i].GetAdjacentRoom(Direction.East, room.floorPos);
                    ConstructWallType(room, adjacentRoom, Direction.East, wallsMesh);
                    adjacentRoom = building.floors[i].GetAdjacentRoom(Direction.West, room.floorPos);
                    ConstructWallType(room, adjacentRoom, Direction.West, wallsMesh);
                }
            }
        }

        #region Determine Wall Type
        ///-////////////////////////////////////////////////////////////////////
        ///
        private WallType DetermineWallType(RoomNode currentRoom, RoomNode adjacentRoom, Direction argDirection)
        {
            if (currentRoom == building.rootNode && argDirection == Direction.South)
            {
                return WallType.Entrance;
            }
                bool isConnected = false;
            if (currentRoom.IsConnectedTo(adjacentRoom))
            {
                isConnected = true;
            }

            switch (currentRoom.roomType)
            {
                case RoomType.Bedroom:
                    return DetermineBedroomWallType(isConnected, currentRoom, adjacentRoom); ;
                case RoomType.Bathroom:
                    return DetermineBathroomWallType(isConnected, currentRoom, adjacentRoom);
                case RoomType.Kitchen:

                    break;
                case RoomType.DiningRoom:

                    break;
                case RoomType.LivingRoom:

                    break;
                case RoomType.Office:
                    break;
                case RoomType.Hallway:

                    break;
                case RoomType.Garage:
                    break;
                case RoomType.Stairs:
                    break;
                case RoomType.Empty:
                    break;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private WallType DetermineBedroomWallType(bool isConnected, RoomNode currentRoom, RoomNode adjacentRoom)
        {
            if (adjacentRoom.roomType == RoomType.Office)
            {
                return WallType.Empty;
            }
            else if (currentRoom.HasDoor())
            {
                return WallType.Wall;
            }
            else
            {
                return WallType.Door;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private WallType DetermineBathroomWallType(bool isConnected, RoomNode currentRoom, RoomNode adjacentRoom)
        {
            if (isConnected) return WallType.Door;
            if (currentRoom.HasDoor() || adjacentRoom.roomType == RoomType.Stairs)
            {
                return WallType.Wall;
            }
            return WallType.Door;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineKitchenWallType(bool isConnected, RoomNode currentRoom, RoomNode adjacentRoom)
        {
            if (currentRoom.HasDoor())
            {
                return WallType.Wall;
            }
            else if (currentRoom.IsConnectedTo(adjacentRoom))
            {

            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineDiningRoomWallType(bool isConnected, RoomNode argCurrentRoom, RoomNode adjacentRoom)
        {
            switch (adjacentRoom.roomType)
            {
                case RoomType.Bedroom:
                    if (adjacentRoom.HasDoor() && !argCurrentRoom.IsConnectedTo(adjacentRoom)) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Bathroom:
                    if (adjacentRoom.HasDoor() && !argCurrentRoom.IsConnectedTo(adjacentRoom)) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Kitchen:
                    return WallType.Empty;
                case RoomType.DiningRoom:
                    return WallType.Empty;
                case RoomType.LivingRoom:
                    return WallType.Empty;
                case RoomType.Office:
                    if (adjacentRoom.HasDoor()) return WallType.Wall;
                    return WallType.Door;
                case RoomType.Hallway:
                    return WallType.EmptyDoor;
                case RoomType.Garage:
                    return WallType.Door;
                case RoomType.Stairs:
                    return WallType.Door;
                case RoomType.Empty:
                    return WallType.Door;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineLivingRoomWallType(bool isConnected, RoomNode argCurrentRoom, RoomNode adjacentRoom)
        {

        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineOfficeWallType(bool isConnected, RoomNode argCurrentRoom, RoomNode adjacentRoom)
        {

        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineHallwayWallType(bool isConnected, RoomNode argCurrentRoom, RoomNode adjacentRoom)
        {

        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineStairsWallType(bool isConnected, RoomNode argCurrentRoom, RoomNode adjacentRoom)
        {

        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public WallType DetermineEmptyWallType(bool isConnected, RoomNode argCurrentRoom, RoomNode adjacentRoom)
        {

        }

        #endregion // Determine Wall Type

        ///-////////////////////////////////////////////////////////////////////
        ///
        private GameObject ConstructRoomComponents(RoomNode room, GameObject floorObject, out Mesh mesh)
        {
            // Construct Each Room
            GameObject roomObject = new GameObject(room.roomType.ToString());
            mesh = new Mesh();
            roomObject.AddComponent<MeshFilter>().sharedMesh = mesh;
            roomObject.transform.parent = floorObject.transform;
            roomObject.transform.localPosition = new Vector3(room.floorPos.x, 0, room.floorPos.y) * roomSize;

            return roomObject;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void ConstructWallType(RoomNode currentRoom, RoomNode adjacentRoom, Direction argDirection, Mesh mesh)
        {
            WallType wallType = DetermineWallType(currentRoom, adjacentRoom, argDirection);
            currentRoom.AddConnection(adjacentRoom, wallType);
            adjacentRoom.AddConnection(currentRoom, wallType);

            switch (wallType)
            {
                case WallType.Door:
                    ConstructDoor(argDirection, mesh);
                    break;
                case WallType.Wall:
                    
                    break;
                case WallType.EmptyDoor:

                case WallType.Empty:
                    break;
                case WallType.Window:
                    break;
            }
        }
        ///-////////////////////////////////////////////////////////////////////
        ///
        private void ConstructDoor(Direction argDirection, Mesh mesh)
        {

        }
    }
}