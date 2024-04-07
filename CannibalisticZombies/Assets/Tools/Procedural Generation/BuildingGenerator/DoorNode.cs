using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies.ProceduralGeneration
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public enum DoorOrientation
    {
        Horizontal,
        Vertical
    }

    ///-////////////////////////////////////////////////////////////////////
    ///
    public class DoorNode : Node
    {
        public Vector2 position;
        public RoomNode room1;
        public RoomNode room2;
        public DoorOrientation orientation;

        public DoorNode(RoomNode room1, RoomNode room2)
        {
            this.room1 = room1;
            this.room2 = room2;

            position = new Vector2(((float)room1.floorPos.x + (float)room2.floorPos.x) / 2, ((float)room1.floorPos.y + (float)room2.floorPos.y) / 2);

            float xDifference = room1.floorPos.x - room2.floorPos.x;
            float yDifference = room1.floorPos.y - room2.floorPos.y;
            if (Mathf.Abs(xDifference) > 0)
            {
                orientation = DoorOrientation.Horizontal;
            }
            else if (Mathf.Abs(yDifference) > 0)
            {
                orientation = DoorOrientation.Vertical;
            }
            else
            {
                Debug.LogError("Unable to establish door orientation");
            }
        }

        public bool CheckConnection(RoomNode node1, RoomNode node2)
        {
            return (room1 == node1 || room1 == node2) && (room2 == node1 || room2 == node2);
        }
    }

}