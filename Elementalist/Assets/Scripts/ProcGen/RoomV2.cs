using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomV2
{
    public bool completed { get; set; }
    
    public enum RoomType
    {
        Empty,
        Enemy,
        Reward,
        Start,
        End
    }

    public RoomType roomType { get; set; }

    public Dictionary<string, bool> connections = new Dictionary<string, bool>()
    {
        { "top", false },
        { "left", false },
        { "bottom", false },
        { "right", false }
    };

    public (bool, bool, bool, bool) GetAllConnections()
    {
        return (connections["top"], connections["left"], connections["bottom"], connections["right"]);
    }

    public void SetAllConnections((bool,bool,bool,bool) connectionsToSet)
    {
        connections["top"] = connectionsToSet.Item1;
        connections["left"] = connectionsToSet.Item2;
        connections["bottom"] = connectionsToSet.Item3;
        connections["right"] = connectionsToSet.Item4;
    }
}
