using System.Collections.Generic;
using UnityEngine;
using GetMikyled;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public class BuildingConstructor : MonoBehaviour
    {
        [SerializeField] private Vector2Int widthRange;
        [SerializeField] private Vector2Int heightRange;
        [SerializeField] private Vector2Int floorRange;
        [SerializeField] private float roomSize = 15f;
        [SerializeField] private float roomHeight = 5f;
        [SerializeField] private float wallThickness = 0.5f;

        [Space]
        [Header("Door Properties")]
        [SerializeField] private GameObject doorPrefab;
        [SerializeField] private float doorPosition = 0;
        [SerializeField] private float doorWidth = 1.5f;
        [SerializeField] private float doorHeight = 2f;

        [Space]
        [SerializeField] private RoomTypeSO defaultPreset;
        [SerializeField] private RoomTypeSO bedroomPreset;
        [SerializeField] private RoomTypeSO bathroomPreset;
        [SerializeField] private RoomTypeSO kitchenPreset;
        [SerializeField] private RoomTypeSO diningRoomPreset;
        [SerializeField] private RoomTypeSO livingRoomPreset;
        [SerializeField] private RoomTypeSO officePreset;
        [SerializeField] private RoomTypeSO hallwayPreset;
        [SerializeField] private RoomTypeSO stairwayPreset;
        [SerializeField] private RoomTypeSO basementPreset;

        BuildingGenerator building;
        private GameObject buildingObject;
        private GameObject roomObject;

        bool finishedBuilding = false;

        private Mesh floorMesh;
        Vector3[] floorNormals = new Vector3[]
            {
                Vector3.up,
                Vector3.up,
                Vector3.up,
                Vector3.up,
                Vector3.down,
                Vector3.down,
                Vector3.down,
                Vector3.down,
            };

        private Vector3 centerPoint;
        private Vector3[] vertices;
        private int[] triangles;
        int vertIndex = 0;
        int triIndex = 0;

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
            ConstructFloorMesh();
            for (int i = 0; i < building.floors.Length; i++)
            {
                GameObject floorObject = new GameObject("Floor_" + i);
                floorObject.transform.parent = buildingObject.transform;
                floorObject.transform.localPosition += new Vector3(0, i * roomHeight, 0);
                floorObject.layer = 7;

                // Create Doors
                GameObject doorsObject = new GameObject("Doors");
                doorsObject.transform.localPosition = new Vector3(0, i * roomHeight, 0);
                foreach (DoorNode door in building.floors[i].doors)
                {
                    GameObject doorObject = Instantiate(doorPrefab);
                    doorObject.transform.parent = doorsObject.transform;
                    doorObject.transform.name = "Door";
                    doorObject.transform.localPosition = new Vector3(door.position.x * (roomSize + wallThickness), 0, door.position.y * (roomSize + wallThickness));

                    // Rotate door and position accordingly based on orientation
                    if (door.orientation == DoorOrientation.Horizontal)
                    {
                        doorObject.transform.Rotate(new Vector3(0, 90, 0));
                        doorObject.transform.localPosition -= new Vector3(0, 0, -(-roomSize / 2 + doorPosition + doorWidth / 2));
                    }
                    else
                    {
                        doorObject.transform.localPosition -= new Vector3(-(-roomSize / 2 + doorPosition + doorWidth / 2), 0, 0);
                    }
                }

                foreach (RoomNode room in building.floors[i].rooms)
                {
                    // Construct Room
                    roomObject = new GameObject(room.roomType.ToString());
                    roomObject.layer = 7;

                    // Create the room's furniture
                    RoomTypeSO roomPreset = GetRoomPreset(room.roomType);
                    if (roomPreset != null)
                    {
                        GameObject interiorObject = DetermineRoomInterior(roomPreset);
                        if (interiorObject != null)
                        {
                            interiorObject = Instantiate(interiorObject);
                            interiorObject.name = room.roomType + " Interior";
                            interiorObject.transform.parent = roomObject.transform;
                            interiorObject.transform.localPosition = Vector3.zero;
                        }
                    }

                    // Create the room's flooring
                    if (room.roomType != RoomType.Empty)
                    {
                        GameObject roomFloorObject = new GameObject("Floor");
                        roomFloorObject.transform.parent = roomObject.transform;
                        roomFloorObject.transform.localPosition = Vector3.zero;
                        roomFloorObject.layer = 7;
                        roomFloorObject.AddComponent<MeshFilter>().sharedMesh = floorMesh;
                        roomFloorObject.AddComponent<MeshRenderer>().sharedMaterial = roomPreset.floorMaterial;
                        BoxCollider floorCollider = roomFloorObject.AddComponent<BoxCollider>();
                        floorCollider.enabled = true;

                        roomFloorObject.layer = 10;              
                    }

                    Mesh wallsMesh = new Mesh();
                    roomObject.AddComponent<MeshFilter>().sharedMesh = wallsMesh;
                    roomObject.AddComponent<MeshRenderer>().sharedMaterial = roomPreset.wallMaterial;
                    MeshCollider collider = roomObject.AddComponent<MeshCollider>();
                    collider.convex = false;
                    collider.sharedMesh = wallsMesh;
                    collider.enabled = true;
                    roomObject.transform.parent = floorObject.transform;
                    roomObject.transform.localPosition = new Vector3(room.floorPos.x * (roomSize + wallThickness), 0, room.floorPos.y * (roomSize + wallThickness));
                    centerPoint = roomObject.transform.localPosition;

                    vertices = new Vector3[room.GetVerticesCount()];
                    triangles = new int[room.GetTrianglesCount() * 3];
                    vertIndex = 0;
                    triIndex = 0;

                    // Create the room's walls
                    foreach (RoomNode adjacentRoom in room.adjacentRooms.Keys)
                    {
                        WallType wallType = room.adjacentRooms[adjacentRoom];
                        ConstructWallType(room.GetAdjacentRoomDirection(adjacentRoom), wallsMesh, wallType);
                    }

                    wallsMesh.vertices = vertices;
                    wallsMesh.triangles = triangles;
                    wallsMesh.RecalculateNormals();
                }
            }

            finishedBuilding = true;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void ConstructWallType(Direction argDirection, Mesh mesh, WallType argWallType)
        {
            switch(argWallType)
            {
                case WallType.Wall:
                    ConstructWall(argDirection, mesh);
                    break;
                case WallType.Door:
                    ConstructDoor(argDirection, mesh);
                    break;
                case WallType.SecondaryDoor:
                    ConstructDoor(argDirection, mesh);
                    break;
                case WallType.Empty:
                    return;
                case WallType.Entrance:
                    ConstructDoor(argDirection, mesh);
                    break;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void ConstructDoor(Direction argDirection, Mesh mesh)
        {
            
            float roomWalLength = roomSize + wallThickness * 2;
            float calculatedDoorPosition = doorPosition - roomSize / 2;

            switch (argDirection)
            {
                case Direction.North:
                    vertices[vertIndex] = new Vector3(calculatedDoorPosition, centerPoint.y, roomSize / 2);
                    vertices[vertIndex + 1] = new Vector3(calculatedDoorPosition, centerPoint.y + doorHeight, roomSize / 2);
                    vertices[vertIndex + 2] = new Vector3(calculatedDoorPosition + doorWidth, centerPoint.y + doorHeight, roomSize / 2);
                    vertices[vertIndex + 3] = new Vector3(calculatedDoorPosition + doorWidth, centerPoint.y, roomSize / 2);
                    vertices[vertIndex + 4] = new Vector3(roomWalLength / 2, centerPoint.y, roomSize / 2);
                    vertices[vertIndex + 5] = new Vector3(roomWalLength / 2, centerPoint.y + roomHeight, roomSize / 2);
                    vertices[vertIndex + 6] = new Vector3(0, centerPoint.y + roomHeight, roomSize / 2);
                    vertices[vertIndex + 7] = new Vector3(-roomWalLength / 2, centerPoint.y + roomHeight, roomSize / 2);
                    vertices[vertIndex + 8] = new Vector3(-roomWalLength / 2, centerPoint.y, roomSize / 2);
                    break;
                case Direction.South:
                    vertices[vertIndex] = new Vector3(calculatedDoorPosition + doorWidth, centerPoint.y, -roomSize / 2);
                    vertices[vertIndex + 1] = new Vector3(calculatedDoorPosition + doorWidth, centerPoint.y + doorHeight, -roomSize / 2);
                    vertices[vertIndex + 2] = new Vector3(calculatedDoorPosition, centerPoint.y + doorHeight, -roomSize / 2);
                    vertices[vertIndex + 3] = new Vector3(calculatedDoorPosition, centerPoint.y, -roomSize / 2);
                    vertices[vertIndex + 4] = new Vector3(-roomWalLength / 2, centerPoint.y, -roomSize / 2);
                    vertices[vertIndex + 5] = new Vector3(-roomWalLength / 2, centerPoint.y + roomHeight, -roomSize / 2);
                    vertices[vertIndex + 6] = new Vector3(0, centerPoint.y + roomHeight, -roomSize / 2);
                    vertices[vertIndex + 7] = new Vector3(roomWalLength / 2, centerPoint.y + roomHeight, -roomSize / 2);
                    vertices[vertIndex + 8] = new Vector3(roomWalLength / 2, centerPoint.y, -roomSize / 2);
                    break;
                case Direction.East:
                    vertices[vertIndex] = new Vector3(roomSize / 2, centerPoint.y, calculatedDoorPosition + doorWidth);
                    vertices[vertIndex + 1] = new Vector3(roomSize / 2, centerPoint.y + doorHeight, calculatedDoorPosition + doorWidth);
                    vertices[vertIndex + 2] = new Vector3(roomSize / 2, centerPoint.y + doorHeight, calculatedDoorPosition);
                    vertices[vertIndex + 3] = new Vector3(roomSize / 2, centerPoint.y, calculatedDoorPosition);
                    vertices[vertIndex + 4] = new Vector3(roomSize / 2, centerPoint.y, -roomWalLength / 2);
                    vertices[vertIndex + 5] = new Vector3(roomSize / 2, centerPoint.y + roomHeight, -roomWalLength / 2);
                    vertices[vertIndex + 6] = new Vector3(roomSize / 2, centerPoint.y + roomHeight, 0);
                    vertices[vertIndex + 7] = new Vector3(roomSize / 2, centerPoint.y + roomHeight, roomWalLength / 2);
                    vertices[vertIndex + 8] = new Vector3(roomSize / 2, centerPoint.y, roomWalLength / 2);
                    break;
                case Direction.West:
                    vertices[vertIndex] = new Vector3(-roomSize / 2, centerPoint.y, calculatedDoorPosition);
                    vertices[vertIndex + 1] = new Vector3(-roomSize / 2, centerPoint.y + doorHeight, calculatedDoorPosition);
                    vertices[vertIndex + 2] = new Vector3(-roomSize / 2, centerPoint.y + doorHeight, calculatedDoorPosition + doorWidth);
                    vertices[vertIndex + 3] = new Vector3(-roomSize / 2, centerPoint.y, calculatedDoorPosition + doorWidth);
                    vertices[vertIndex + 4] = new Vector3(-roomSize / 2, centerPoint.y, roomWalLength / 2);
                    vertices[vertIndex + 5] = new Vector3(-roomSize / 2, centerPoint.y + roomHeight, roomWalLength / 2);
                    vertices[vertIndex + 6] = new Vector3(-roomSize / 2, centerPoint.y + roomHeight, 0);
                    vertices[vertIndex + 7] = new Vector3(-roomSize / 2, centerPoint.y + roomHeight, -roomWalLength / 2);
                    vertices[vertIndex + 8] = new Vector3(-roomSize / 2, centerPoint.y, -roomWalLength / 2);
                    break;
            }

            // new triangle on -> //
            triangles[triIndex] = vertIndex;            //
            triangles[triIndex + 1] = vertIndex + 8;
            triangles[triIndex + 2] = vertIndex + 7;
            triangles[triIndex + 3] = vertIndex;        //
            triangles[triIndex + 4] = vertIndex + 7;
            triangles[triIndex + 5] = vertIndex + 1;
            triangles[triIndex + 6] = vertIndex + 1;    //
            triangles[triIndex + 7] = vertIndex + 7;
            triangles[triIndex + 8] = vertIndex + 6;    
            triangles[triIndex + 9] = vertIndex + 1;    //
            triangles[triIndex + 10] = vertIndex + 6;    
            triangles[triIndex + 11] = vertIndex + 2;    
            triangles[triIndex + 12] = vertIndex + 2;    //
            triangles[triIndex + 13] = vertIndex + 6;
            triangles[triIndex + 14] = vertIndex + 5;
            triangles[triIndex + 15] = vertIndex + 2;   //
            triangles[triIndex + 16] = vertIndex + 5;
            triangles[triIndex + 17] = vertIndex + 4;
            triangles[triIndex + 18] = vertIndex + 2;   //
            triangles[triIndex + 19] = vertIndex + 4;
            triangles[triIndex + 20] = vertIndex + 3;

            vertIndex += 9;
            triIndex += 21;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void ConstructWall(Direction argDirection, Mesh mesh)
        {
            float roomWalLength = roomSize + wallThickness * 2;

            switch (argDirection)
            {
                case Direction.North:
                    vertices[vertIndex] = new Vector3(-roomWalLength / 2, centerPoint.y, roomSize / 2);
                    vertices[vertIndex + 1] = new Vector3(-roomWalLength / 2, centerPoint.y + roomHeight, roomSize / 2);
                    vertices[vertIndex + 2] = new Vector3(roomWalLength / 2, centerPoint.y + roomHeight, roomSize / 2);
                    vertices[vertIndex + 3] = new Vector3(roomWalLength / 2, centerPoint.y, roomSize / 2);
                    break;
                case Direction.South:
                    vertices[vertIndex] = new Vector3(roomWalLength / 2, centerPoint.y, -roomSize / 2);
                    vertices[vertIndex + 1] = new Vector3(roomWalLength / 2, centerPoint.y + roomHeight, -roomSize / 2);
                    vertices[vertIndex + 2] = new Vector3(-roomWalLength / 2, centerPoint.y + roomHeight, -roomSize / 2);
                    vertices[vertIndex + 3] = new Vector3(-roomWalLength / 2, centerPoint.y, -roomSize / 2);
                    break;
                case Direction.East:
                    vertices[vertIndex] = new Vector3(roomSize / 2, centerPoint.y, roomWalLength / 2);
                    vertices[vertIndex + 1] = new Vector3(roomSize / 2, centerPoint.y + roomHeight, roomWalLength / 2);
                    vertices[vertIndex + 2] = new Vector3(roomSize / 2, centerPoint.y + roomHeight, -roomWalLength / 2);
                    vertices[vertIndex + 3] = new Vector3(roomSize / 2, centerPoint.y, -roomWalLength / 2);
                    break;
                case Direction.West:
                    vertices[vertIndex] = new Vector3(-roomSize / 2, centerPoint.y, -roomWalLength / 2);
                    vertices[vertIndex + 1] = new Vector3(-roomSize / 2, centerPoint.y + roomHeight, -roomWalLength / 2);
                    vertices[vertIndex + 2] = new Vector3(-roomSize / 2, centerPoint.y + roomHeight, roomWalLength / 2);
                    vertices[vertIndex + 3] = new Vector3(-roomSize / 2, centerPoint.y, roomWalLength / 2);
                    break;
            }
            triangles[triIndex] = vertIndex;
            triangles[triIndex + 1] = vertIndex + 1;
            triangles[triIndex + 2] = vertIndex + 2;
            triangles[triIndex + 3] = vertIndex + 3;
            triangles[triIndex + 4] = vertIndex;
            triangles[triIndex + 5] = vertIndex + 2;

            vertIndex += 4;
            triIndex += 6;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///


        ///-////////////////////////////////////////////////////////////////////
        ///
        private RoomTypeSO GetRoomPreset(RoomType argRoomType)
        {
            switch(argRoomType) {
                case RoomType.Bedroom:
                    return bedroomPreset;
                case RoomType.Bathroom:
                    return bathroomPreset;
                case RoomType.Kitchen:
                    return kitchenPreset;
                case RoomType.DiningRoom:
                    return diningRoomPreset;
                case RoomType.LivingRoom:
                    return livingRoomPreset;
                case RoomType.Hallway:
                    return hallwayPreset;
                case RoomType.Office:
                    return officePreset;
                case RoomType.Stairs:
                    return stairwayPreset;
                case RoomType.Basement:
                    return basementPreset;
                default:
                    return defaultPreset;
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private GameObject DetermineRoomInterior(RoomTypeSO argRoomTypeSO)
        {
            if (argRoomTypeSO.genericRoomPresets.Length > 0)
            {
                return argRoomTypeSO.genericRoomPresets[Random.Range(0, argRoomTypeSO.genericRoomPresets.Length)];
            }
            return null;
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        private void ConstructFloorMesh()
        {
            floorMesh = new Mesh();
            float floorPosition = (roomSize / 2) + (wallThickness / 2);

            Vector3[] floorVertices = new Vector3[]
            {
                new Vector3(-floorPosition, 0, -floorPosition),
                new Vector3(-floorPosition, 0, floorPosition),
                new Vector3(floorPosition, 0, floorPosition),
                new Vector3(floorPosition, 0, -floorPosition),
                new Vector3(-floorPosition, 0, -floorPosition),
                new Vector3(-floorPosition, 0, floorPosition),
                new Vector3(floorPosition, 0, floorPosition),
                new Vector3(floorPosition, 0, -floorPosition)
            };
            int[] floorTriangles = new int[] { 0, 1, 3, 1, 2, 3, 7, 5, 4, 7, 6, 5 };

            floorMesh.vertices = floorVertices;
            floorMesh.triangles = floorTriangles;
            floorMesh.normals = floorNormals;
        }
    }
}