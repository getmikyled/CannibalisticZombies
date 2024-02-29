using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
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
                }
            }
        }

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
        private void ConstructDoor(Direction argDirection, Mesh mesh)
        {

        }
    }
}