using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public class BuildingConstructor: MonoBehaviour
    {
        private static int MIN_WIDTH = 2;
        private static int MIN_HEIGHT = 2;

        [SerializeField] private Vector2Int widthRange;
        [SerializeField] private Vector2Int heightRange;
        [SerializeField] private Vector2Int floorRange;

        [SerializeField] GameObject baseRoom;

        private void Start()
        {
            ConstructBuilding();
        }

        public void ConstructBuilding()
        {
            // GENERATE BUILDING
            int buildingWidth = Random.Range(widthRange.x, widthRange.y + 1);
            int buildingHeight = Random.Range(heightRange.x, heightRange.y + 1);
            int floorCount = Random.Range(floorRange.x, floorRange.y + 1);
            BuildingGenerator building = new BuildingGenerator(buildingWidth, buildingHeight, floorCount);

            GameObject buildingObject = new GameObject("Building");
            buildingObject.transform.position = transform.position;
            for (int i = 0; i < building.floors.Length; i++)
            {
                GameObject floorObject = new GameObject("Floor" + i);
                floorObject.transform.parent = buildingObject.transform;
                floorObject.transform.localPosition += new Vector3(0, i * 5, 0);
                foreach(RoomNode room in building.floors[i].rooms)
                {
                    GameObject roomObject = Instantiate(baseRoom);
                    roomObject.name = room.roomType.ToString();
                    roomObject.transform.parent = floorObject.transform;
                    roomObject.transform.localPosition = new Vector3(room.floorPos.x, 0, room.floorPos.y) * 10;
                }
            }
        }
    }

}