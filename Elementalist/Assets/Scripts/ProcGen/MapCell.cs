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

    public delegate MapModuleState GetModuleAction(List<MapModuleState> modules);
    
    public bool TrySelectState(GetModuleAction action)
    {
        AddOrUpdateToMapCellCache(this);
        var states = new List<MapModuleState>(States);
        while (states.Count > 0)
        {
            var selectState = action(states);
            States = new List<MapModuleState> { selectState };
            if (!TryUpdateAdjacentTiles(this))
            {
                states.Remove(selectState);
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    delegate bool TryUpdateAction();

    bool TryUpdateAdjacentTiles(MapCell tileWithSelectedModule)
    {
        List<TryUpdateAction> updateAdjacentTilesActions = new List<TryUpdateAction>();
        bool updateSuccess = AdjacentTilesPositions.All(tilePos =>
        {
            return _map.MapCellsMatrix[tilePos.x, tilePos.y].TryUpdateStates(this, tileWithSelectedModule, updateAdjacentTilesActions);
        });
        if (!updateSuccess)
        {
            ReverseStates(tileWithSelectedModule);
            return false;
        }
        else
        {
            return updateAdjacentTilesActions.All(action => action.Invoke());
        }
    }

    bool TryUpdateStates(MapCell otherTile, MapCell tileWithSelectedState, List<TryUpdateAction> updateAdjacentTilesActions)
    {
        AddOrUpdateToMapCellCache(tileWithSelectedState);

        int removeModuleCount = States.RemoveAll(thisState =>
        {
            var directionToPreviousTile = otherTile.MapPosition - MapPosition;
            return !otherTile.States.Any(otherState => thisState.IsMatchingModules(otherState, directionToPreviousTile));
        });

        if (States.Count == 0)
        {
            return false;
        }

        if (removeModuleCount > 0)
        {
            updateAdjacentTilesActions.Add(() => TryUpdateAdjacentTiles(tileWithSelectedState));
        }

        return true;
    }

    void AddOrUpdateToMapCellCache(MapCell originalTile)
    {
        if (_mapCellCache.ContainsKey(originalTile))
        {
            _mapCellCache[originalTile] = States.ToArray();
        }
        else
        {
            _mapCellCache.Add(originalTile, States.ToArray());
        }
    }

    public void ReverseStates(MapCell originalTile)
    {
        if (_mapCellCache.ContainsKey(originalTile))
        {
            States = new List<MapModuleState>(_mapCellCache[originalTile]);
            _mapCellCache.Remove(originalTile);

            foreach (var tilePos in AdjacentTilesPositions)
            {
                _map.MapCellsMatrix[tilePos.x, tilePos.y].ReverseStates(originalTile);
            }
        }
    }
}
