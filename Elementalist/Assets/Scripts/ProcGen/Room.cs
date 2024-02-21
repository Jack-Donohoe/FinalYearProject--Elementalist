using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private int id;
    private bool completed = false;
    
    public enum RoomType
    {
        Empty,
        Enemy,
        Reward,
        Start,
        End
    }

    private RoomType roomType;

    public int GetID()
    {
        return id;
    }

    public void SetID(int id)
    {
        this.id = id;
    }

    public bool GetCompleted()
    {
        return completed;
    }

    public void SetCompleted(bool completed)
    {
        this.completed = completed;
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }

    public void SetRoomType(RoomType roomType)
    {
        this.roomType = roomType;
    }
}
