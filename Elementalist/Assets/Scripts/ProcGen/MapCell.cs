using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCell
{
    public Vector2Int MapPosition { get; private set; }
    public List<MapModuleState> States { get; private set; }
    public List<Vector2Int> AdjacentTilesPositions { get; private set; }

    private Map _map;
    private Dictionary<MapCell, MapModuleState[]> _mapCellCache = new Dictionary<MapCell, MapModuleState[]>();

    public MapCell(Map map, Vector2Int mapPosition, List<MapModuleState> states)
    {
        _map = map;
        MapPosition = mapPosition;
        AdjacentTilesPositions = GetAdjacentTilesPositions(map);
        States = states;
    }

    List<Vector2Int> GetAdjacentTilesPositions(Map map)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        if (MapPosition.x - 1 >= 0) { tiles.Add(new Vector2Int (MapPosition.x - 1, MapPosition.y)); }
        if (MapPosition.x + 1 < map.RowsCount) { tiles.Add(new Vector2Int(MapPosition.x + 1, MapPosition.y)); }
        if (MapPosition.y - 1 >= 0) { tiles.Add(new Vector2Int(MapPosition.x, MapPosition.y - 1)); }
        if (MapPosition.y + 1 < map.ColumnsCount) { tiles.Add(new Vector2Int(MapPosition.x, MapPosition.y + 1)); }
        return tiles;
    }
}
