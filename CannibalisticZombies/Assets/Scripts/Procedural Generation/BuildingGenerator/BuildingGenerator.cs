using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public class BuildingGenerator
    {
        [Range(0f, 1f)]
        private const float GARAGE_CHANCE = 0.5f;

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
            // RoomCounts of 3 must have these 3 rooms
            float randomRoomValue = Random.value;
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
        private void AddRoom(RoomType argRoomType)
        {
            roomsList[roomIndex] = argRoomType;
            roomIndex++;
        }
    }

}