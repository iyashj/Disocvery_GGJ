using LevelDesignspace;
using System.Collections.Generic;
using UnityEngine;

namespace Gamespace
{
    public static class GAMECONSTANTS
    {
        public static int UNVISITED_INDEX = -1;
        public static int CONNECTION_BLOCKED = -1;

        public static int PLAYER_PROGRESS_TRACK = -1;

        public static int LastRoomIndex;
        public static int AssignIndex()
        {
            PLAYER_PROGRESS_TRACK++;
            return PLAYER_PROGRESS_TRACK;
        }
        public static void SetLastRoomIndex(int _lastRoomIndex)
        {
            LastRoomIndex = _lastRoomIndex;
        }
        public static void SetTrackerIndex(int _PlayerCurrentRoomIndex)
        {

            PLAYER_PROGRESS_TRACK = _PlayerCurrentRoomIndex;

            //_PlayerCurrentRoomIndex--;
            //PLAYER_PROGRESS_TRACK = _PlayerCurrentRoomIndex + 1;
        }
        public static bool IsLastRoom(int _roomIndex)
        {
            return LastRoomIndex == _roomIndex;
        }

        public static void PrintConsole(string _consoleMessage)
        {
            Debug.Log("****************");
            Debug.Log(_consoleMessage);
            Debug.Log("****************");
        }

        public static void PrintConsole(List<string> _consoleMessages)
        {
            Debug.Log("****************");
            for(int i0 = 0; i0 < _consoleMessages.Count; i0++)
            {
                Debug.Log(_consoleMessages[i0]);
            }
            Debug.Log("****************");
        }
    }

    [System.Serializable]
    public class Room
    {
        public int dynamicIndex;
        public RConnectionData connectors;
        public string storySnippet;

        public Room()
        {
            dynamicIndex = GAMECONSTANTS.UNVISITED_INDEX;
            SetRoomConnectors(new RConnectionData(GAMECONSTANTS.CONNECTION_BLOCKED, 
                GAMECONSTANTS.CONNECTION_BLOCKED, GAMECONSTANTS.CONNECTION_BLOCKED, GAMECONSTANTS.CONNECTION_BLOCKED));
        }
        public Room(RConnectionData _roomCoonnectors)
        {
            SetRoomConnectors(_roomCoonnectors);
        }

        public void SetRoomDynamicIndex()
        {
            dynamicIndex = GAMECONSTANTS.AssignIndex();
        }
        void SetRoomConnectors(RConnectionData _roomCoonnectors)
        {
            connectors.NorthRoomIndex = _roomCoonnectors.NorthRoomIndex;
            connectors.SouthRoomIndex = _roomCoonnectors.SouthRoomIndex;
            connectors.EastRoomIndex = _roomCoonnectors.EastRoomIndex;
            connectors.WestRoomIndex = _roomCoonnectors.WestRoomIndex;
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public LevelData(MapData _mapData)
        {
            lastRoomIndex = _mapData.endPoint;
            startRoomIndex = _mapData.spawnPoint;

            listRooms = new List<Room>();

            for (int i0 = 0; i0 < _mapData.mapInfo.Count; i0++)
            {
                listRooms.Add(_mapData.mapInfo[i0]);
            }
        }

        public int lastRoomIndex;
        public int startRoomIndex;
        public List<Room> listRooms;

        public void ResetRoomsToIndex(int _resetIndex)
        {
            for(int i0 = 0; i0 < listRooms.Count; i0++)
            {
                if (listRooms[i0].dynamicIndex > _resetIndex)
                {
                    listRooms[i0].dynamicIndex = GAMECONSTANTS.UNVISITED_INDEX;
                }
            }
        }

    }

    /// <summary>
    /// Store the connection of this room with its neighbors(other rooms around it)
    /// </summary>
    [System.Serializable]
    public struct RConnectionData
    {
        public int NorthRoomIndex;
        public int SouthRoomIndex;
        public int EastRoomIndex;
        public int WestRoomIndex;

        public RConnectionData (int _northRoomIndex, int _southRoomIndex,
            int _eastRoomIndex, int _westRoomIndex)
        {
            NorthRoomIndex = _northRoomIndex;
            SouthRoomIndex = _southRoomIndex;
            EastRoomIndex = _eastRoomIndex;
            WestRoomIndex = _westRoomIndex;
        }
    }

    public enum EDirection
    {
        North,
        South,
        East,
        West,
    }

}