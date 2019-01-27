using Gamespace;
using LevelDesignspace;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesigner : MonoBehaviour
{
    public List<TileComponent> roomTiles;

    public int startPt;
    public int endPt;

    [SerializeField]
    TileComponent tilePrefab;

    [SerializeField]
    GameObject gridObject;

    [SerializeField]
    Color _tileColor;

    [SerializeField]
    Sprite _spawnPointSprite;

    [SerializeField]
    Sprite _endPointSprite;

    [SerializeField]
    InputField gridRows;

    [SerializeField]
    InputField gridColumns;

    public static bool bStartPointPlaced = true;
    public static bool bEndPointPlaced = true;

    public void GenerateGrid()
    {
        roomTiles = new List<TileComponent>();
        int _rows = int.Parse(gridRows.text);
        int _columns = int.Parse(gridColumns.text);

        gridObject.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridObject.GetComponent<GridLayoutGroup>().constraintCount = _columns;


        int _totalSize = _rows * _columns;

        // Generate Grids
        for(int i0=0;i0< _totalSize; i0++)
        {
            TileComponent _tileComponent = GameObject.Instantiate(tilePrefab, gridObject.transform);
            _tileComponent.levelDesigner = this;
            _tileComponent.tileIndex = i0;
            roomTiles.Add(_tileComponent);
        }

        GridReferencing(_rows, _columns);
    }

    public void GridReferencing(int _gridRows, int _gridColumns)
    {
        //for(int i0 = 0; i0 < roomTiles.Count; i0++)
        //{
        //    int _northPointer = 0;
        //    int _westPointer = 0;
        //    int _southPointer = 0;
        //    int _eastPointer = 0;

        //    if (i0 - _gridLength < 0)
        //    {
        //        _northPointer = i0 - _gridLength + (_gridLength * _gridLength);
        //    }
        //    else
        //    {
        //        _northPointer = i0 - _gridLength;
        //    }

        //    if(i0 + _gridLength>=(_gridLength * _gridLength))
        //    {
        //       _southPointer = i0 + _gridLength - (_gridLength * _gridLength);
        //    }
        //    else
        //    {
        //        _southPointer = i0 + _gridLength;
        //    }

        //    if((i0 + 1)%_gridLength == 0)
        //    {
        //        _eastPointer = i0 + 1 - _gridLength;
        //    }
        //    else
        //    {
        //        _eastPointer = i0 + 1;
        //    }

        //    if((i0 % _gridLength) == 0)
        //    {
        //        _westPointer = i0 - 1 + _gridLength;
        //    }
        //    else
        //    {
        //        _westPointer = i0 - 1;
        //    }

        //    roomTiles[i0].northTileComponent = roomTiles[_northPointer];
        //    roomTiles[i0].southTileComponent = roomTiles[_southPointer];
        //    roomTiles[i0].eastTileComponent = roomTiles[_eastPointer];
        //    roomTiles[i0].westTileComponent = roomTiles[_westPointer];
        //}

        for (int i0 = 0; i0 < roomTiles.Count; i0++)
        {
            int _northPointer = 0;
            int _westPointer = 0;
            int _southPointer = 0;
            int _eastPointer = 0;

            if (i0 - _gridRows < 0)
            {
                _northPointer = i0 - _gridRows + (_gridRows * _gridColumns);
            }
            else
            {
                _northPointer = i0 - _gridRows;
            }

            if (i0 + _gridRows >= (_gridRows * _gridColumns))
            {
                _southPointer = i0 + _gridRows - (_gridRows * _gridColumns);
            }
            else
            {
                _southPointer = i0 + _gridRows;
            }

            if ((i0 + 1) % _gridColumns == 0)
            {
                _eastPointer = i0 + 1 - _gridColumns;
            }
            else
            {
                _eastPointer = i0 + 1;
            }

            if ((i0 % _gridColumns) == 0)
            {
                _westPointer = i0 - 1 + _gridColumns;
            }
            else
            {
                _westPointer = i0 - 1;
            }

            roomTiles[i0].northTileComponent = roomTiles[_northPointer];
            roomTiles[i0].southTileComponent = roomTiles[_southPointer];
            roomTiles[i0].eastTileComponent = roomTiles[_eastPointer];
            roomTiles[i0].westTileComponent = roomTiles[_westPointer];
        }

    }

    public Sprite PlaceSpecialSprite(int _tileIndex)
    {
        if (!bStartPointPlaced)
        {
            startPt = _tileIndex;
            bStartPointPlaced = true;
            return _spawnPointSprite;
        }
        else if (!bEndPointPlaced)
        {
            endPt = _tileIndex;
            bEndPointPlaced = true;
            return _endPointSprite;
        }
        else
        {
            return null;
        }
    }
    public void PlaceStartPoint()
    {
        bStartPointPlaced = false;
        bEndPointPlaced = true;

        for(int i0 = 0; i0 < roomTiles.Count; i0++)
        {
            if(roomTiles[i0].specialPoint.sprite == _spawnPointSprite)
            {
                roomTiles[i0].specialPoint.sprite = null;
            }
        }
    }

#if UNITY_EDITOR
    public void SaveMap()
    {
        MapData _mapData = new MapData(startPt, endPt, roomTiles);
        string _jsonContent = JsonUtility.ToJson(_mapData);
        string _savePath = EditorUtility.SaveFilePanel("Save MapData", Application.dataPath, "newMap", "json");
        File.WriteAllText(_savePath, _jsonContent);
    }

#endif
    public void PlaceEndPoint()
    {
        bStartPointPlaced = true;
        bEndPointPlaced = false;

        for (int i0 = 0; i0 < roomTiles.Count; i0++)
        {
            if (roomTiles[i0].specialPoint.sprite == _endPointSprite)
            {
                roomTiles[i0].specialPoint.sprite = null;
            }
        }
    }
}
