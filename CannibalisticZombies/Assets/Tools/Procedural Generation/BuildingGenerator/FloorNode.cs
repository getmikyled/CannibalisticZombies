using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public class FloorNode : Node
    {
        private int gridWidth;
        private int gridHeight;
        private int floorNumber;

        public RoomNode[,] rooms;
        public RoomNode stairsNode;

        ///-////////////////////////////////////////////////////////////////////
        ///
        public FloorNode(int argGridWidth, int argGridHeight, int argFloorNumber)
        {
            floorNumber = argFloorNumber;
            gridWidth = argGridWidth;
            gridHeight = argGridHeight;

            rooms = new RoomNode[gridWidth, gridHeight];
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public void GenerateRooms(Vector2Int downStairsPos, bool hasUpstairs, BuildingGenerator building)
        {
            Vector2Int randPos = new Vector2Int().Randomize(gridWidth, gridHeight);
            // CREATE STAIRS
            if (downStairsPos.x == -1 && downStairsPos.y == -1)
            {
                // Prevents basement stairs from being in the first slot
                while (randPos.x == 0 && randPos.y == 0 )
                {
                    randPos = new Vector2Int().Randomize(gridWidth, gridHeight);
                }

                rooms[randPos.x, randPos.y] = SetRoom(RoomType.Basement, randPos);
            }
            else
            {
                // Prevents stairs from being in the first slot
                while (randPos.x == 0 && randPos.y == 0)
                {
                    randPos = new Vector2Int().Randomize(gridWidth, gridHeight);
                }
                rooms[downStairsPos.x, downStairsPos.y] = SetRoom(RoomType.Empty, downStairsPos);
            }

            if (hasUpstairs)
            {
                while (true)
                {
                    randPos = new Vector2Int().Randomize(gridWidth, gridHeight);
                    if (rooms[randPos.x, randPos.y] == null)
                    {
                        stairsNode = SetRoom(RoomType.Stairs, randPos);
                        rooms[randPos.x, randPos.y] = stairsNode;
                        break;
                    }
                }
            }

            // FILL IN REST OF ROOMS
            for (int i = 0; i < rooms.GetLength(0); i++)
            {
                for (int j = 0; j < rooms.GetLength(1); j++)
                {
                    if (rooms[i, j] == null)
                    {
                        rooms[i, j] = SetRoom(building.roomsList[building.roomIndex], new Vector2Int(i, j));
                        building.roomIndex++;
                    }
                }
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public RoomNode SetRoom(RoomType argRoomType, Vector2Int argPos) {
            RoomNode newRoom = new RoomNode(argRoomType);
            newRoom.SetPos(argPos);
            newRoom.SetFloor(floorNumber);

            return newRoom;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public RoomNode GetAdjacentRoom(Direction argDirection, Vector2Int argRoomPos)
        {
            switch (argDirection)
            {
                case Direction.North:
                    if (argRoomPos.y + 1 >= gridHeight) break;
                    return rooms[argRoomPos.x, argRoomPos.y + 1];
                case Direction.South:
                    if (argRoomPos.y - 1 < 0) break;
                    return rooms[argRoomPos.x, argRoomPos.y - 1];

                case Direction.East:
                    if (argRoomPos.x + 1 >= gridWidth) break;
                    return rooms[argRoomPos.x + 1, argRoomPos.y];
                case Direction.West:
                    if (argRoomPos.x - 1 < 0) break;
                    return rooms[argRoomPos.x - 1, argRoomPos.y];
            }
            return null;
        }
    }

}