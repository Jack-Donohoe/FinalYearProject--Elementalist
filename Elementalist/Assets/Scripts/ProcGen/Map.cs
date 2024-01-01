using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    [SerializeField] Vector2Int _mapSize = new Vector2Int(5, 5);
    [SerializeField] float _cellSize;
    [SerializeField] MapModule[] _mapModules;
    [SerializeField] List<MapModuleContact> _contactTypes = new List<MapModuleContact>();
    public MapCell[,] MapCellsMatrix;
    public int RowsCount => MapCellsMatrix.GetLength(0);
    public int ColumnsCount => MapCellsMatrix.GetLength(1);
    private MapCell[] _mapCellsArray;

    private void Start()
    {
        InitializeMap();
        FillTiles();
        CreateMap();
    }

    void InitializeMap()
    {
        MapCellsMatrix = new MapCell[_mapSize.x, _mapSize.y];

        var modules = GetMapModules();
        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j =  0; j < _mapSize.y; j++)
            {
                MapCellsMatrix[i, j] = new MapCell(this, new Vector2Int(i, j), new List<MapModuleState>(modules));
                Debug.Log(MapCellsMatrix[i, j].States.Count());
            }
        }
        _mapCellsArray = MapCellsMatrix.Cast<MapCell>().ToArray();
    }

    void FillTiles()
    {
        MapCell tile = null;

        do {
            var tilesWithUnselectedState = _mapCellsArray.Where(t => t.States.Count > 1).ToArray();

            if (tilesWithUnselectedState.Length == 0)
                return;

            var minStatesCount = tilesWithUnselectedState.Min(t => t.States.Count);

            tile = tilesWithUnselectedState.First(t => t.States.Count == minStatesCount);
        } while (tile.TrySelectState(states => states[Random.Range(0, states.Count)]));
    }

    void CreateMap()
    {
        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                Debug.Log(i + " " + j);
                var currentPosition = new Vector3(i * _cellSize, 0, j * _cellSize);
                Debug.Log(MapCellsMatrix[i, j].States.Count());
                MapCellsMatrix[i, j].States[0].InstantiatePrefab(this, currentPosition);
            }
        }
    }

    List<MapModuleState> GetMapModules()
    {
        List<MapModuleState> mapModules = new List<MapModuleState>();
        foreach (var module in _mapModules)
        {
            mapModules.AddRange(module.GetMapModulesFromPrefab());
        }
        return mapModules;
    }

    public MapModuleContact GetContact(string type)
    {
        return _contactTypes.First(contact => contact.ContactType == type);
    }
}
