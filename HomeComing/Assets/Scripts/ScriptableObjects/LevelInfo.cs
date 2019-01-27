using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelInfo", menuName = "Assets/LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public DiscoveryData discoveryData;
}


[System.Serializable]
public class DiscoveryData
{
    public List<string> levelPath;
    public List<string> levelDialogues;
    public List<string> conclusionLines;
}
