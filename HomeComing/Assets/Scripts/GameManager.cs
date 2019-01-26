using Gamespace;
using LevelDesignspace;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;
        
    void Start()
    {
        string _jsonPath = UnityEditor.EditorUtility.OpenFilePanel("Open Map", Application.dataPath, "json");
        string _jsonContent = File.ReadAllText(_jsonPath);
        MapData _mapData = JsonUtility.FromJson<MapData>(_jsonContent);

        GAMECONSTANTS.LastRoomIndex = _mapData.endPoint;
        levelManager.ClearRoom();
        levelManager.levelData = new Gamespace.LevelData(_mapData);
        levelManager.LoadFirstRoom();
        
    }



}
