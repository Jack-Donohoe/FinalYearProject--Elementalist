using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameObject room;
    private bool[] connections;
    
    public enum RoomType
    {
        Empty,
        Enemy,
        Reward,
        Start,
        End
    }

    private RoomType roomType;

    public Room(GameObject room, bool[] connections, RoomType roomType)
    {
        this.room = room;
        this.connections = connections;
        this.roomType = roomType;
    }

    public GameObject GetRoom()
    {
        return room;
    }

    public bool[] GetConnections()
    {
        return connections;
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }
}
