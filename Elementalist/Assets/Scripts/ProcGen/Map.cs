using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
            }
        }
        _mapCellsArray = MapCellsMatrix.Cast<MapCell>().ToArray();
    }

    void FillTiles()
    {
        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                var currentPosition = new Vector3(i * _cellSize, 0, j * _cellSize);
                MapCellsMatrix[i, j].States[0].InstantiatePrefab(this, currentPosition);
            }
        }
    }
}
