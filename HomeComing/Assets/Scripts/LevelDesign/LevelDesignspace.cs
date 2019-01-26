using Gamespace;
using System.Collections.Generic;

namespace LevelDesignspace
{
    [System.Serializable]
    public class MapData
    {
        public int spawnPoint;
        public int endPoint;

        public List<Room> mapInfo;

        public MapData(int _spawnPt, int _endPt, List<TileComponent> _mapLayout)
        {
            mapInfo = new List<Room>();

            endPoint = _endPt;
            spawnPoint = _spawnPt;

            for (int i0 = 0; i0 < _mapLayout.Count; i0++)
            {
                Room _room = new Room();
                _room.storySnippet = i0.ToString();

                int _northIndex = 0;
                int _southIndex = 0;
                int _westIndex = 0;
                int _eastIndex = 0;

                if(_mapLayout[i0].northBlocker.color.a == 0)
                {
                    _northIndex = GAMECONSTANTS.CONNECTION_BLOCKED;
                }
                else
                {
                    _northIndex = _mapLayout[i0].northTileComponent.tileIndex;
                }

                if (_mapLayout[i0].southBlocker.color.a == 0)
                {
                    _southIndex = GAMECONSTANTS.CONNECTION_BLOCKED;

                }
                else
                {
                    _southIndex = _mapLayout[i0].southTileComponent.tileIndex;

                }

                if (_mapLayout[i0].westBlocker.color.a == 0)
                {
                    _westIndex = GAMECONSTANTS.CONNECTION_BLOCKED;

                }
                else
                {
                    _westIndex = _mapLayout[i0].westTileComponent.tileIndex;

                }

                if (_mapLayout[i0].eastBlocker.color.a == 0)
                {
                    _eastIndex = GAMECONSTANTS.CONNECTION_BLOCKED;

                }
                else
                {
                    _eastIndex = _mapLayout[i0].eastTileComponent.tileIndex;

                }

                _room.connectors = new RConnectionData(_northIndex,_southIndex,_eastIndex,_westIndex);


                mapInfo.Add(_room);
            }
        }

    }

}