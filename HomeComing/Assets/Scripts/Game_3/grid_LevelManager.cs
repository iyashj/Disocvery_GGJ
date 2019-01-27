using Gamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class grid_LevelManager : MonoBehaviour
{

    public LineRenderer playerTrackRenderer;

    public List<int> playerTrack;

    public float moveDuration = 0.50f;
    public float moveDisplacement = 30f;
    public int playerCurretIndex = -1;
    public int playerPreviousIndex = -1;

    public float lightingDuration = 1f;

    public Color transparentColor;
    public Color opaqueColor;

    public Text Text_currentProgressTracker;
    public Image currentProgressBar;

    public LevelData levelData;

    [SerializeField]
    public Sprite EndSprite;

    [SerializeField]
    GameObject targetGridParent;

    [SerializeField]
    grid_TileComponent grid_tilePrefab;

    public List<grid_TileComponent> _roomLists;

    public bool btweening = false;

    public void StartLevel()
    {
        LayoutGrid();
        playerCurretIndex = levelData.startRoomIndex;
        playerTrack.Add(playerCurretIndex);
        NextRoom();
        Text_currentProgressTracker.gameObject.SetActive(true);
        
    }

    public void ShowPlayerTrack()
    {

    }
    private float _nextPointWaitDuration = 0.5f;
    private List<int> _indices;

    private List<int> indicesAlreadyTraced;

    private void ResetAllButGoal()
    {
        for(int i0 = 0; i0 < _roomLists.Count; i0++)
        {
            if(i0!= levelData.lastRoomIndex)
            {
                _roomLists[i0].RoomLeftEvent();
            }
        }
    }

    private grid_TileComponent grid_tileComponent;
    private int currentPoint;
    private int maxPoints;

    public  IEnumerator PlayLine()
    {
        if (playerTrack.Count > 0)
        {
            playerTrack.RemoveAt(playerTrack.Count - 1);
        }

        maxPoints = playerTrack.Count;

        foreach (int _tileIndex in playerTrack)
        {
            currentPoint++;
            grid_tileComponent = _roomLists[_tileIndex];
            indicesAlreadyTraced.Add(_tileIndex);

            yield return new WaitForSeconds(_nextPointWaitDuration);
            ResetAllButGoal();
            FancyLightUpRoom(grid_tileComponent);

            if (currentPoint == maxPoints)
            {
                TrailComplete();
            }
        }
    }

    IEnumerator LoadNewLevel()
    {
        GAMECONSTANTS.LEVEL_INDEX++;
        if (GAMECONSTANTS.LEVEL_INDEX == GAMECONSTANTS.MAX_LEVEL)
        {
            GAMECONSTANTS.dialogueCharges = 2;
            yield return new WaitForSeconds(GAMECONSTANTS.levelChangeWait);
            SceneManager.LoadScene(GAMECONSTANTS.CONCLUSION_SCENE);
        }
        else
        {
            yield return new WaitForSeconds(GAMECONSTANTS.levelChangeWait);
            SceneManager.LoadScene(GAMECONSTANTS.INGAME_SCENE);

        }
    }

    public void TrailComplete()
    {
        StartCoroutine(LoadNewLevel());
    }

    public Vector3[] EquivalentVec3Indices(int[] indicesArray)
    {
        Vector3[] eqvVec3 = new Vector3[indicesArray.Length];
        for(int i0 = 0; i0 < indicesArray.Length; i0++)
        {
            Vector3 vecIndex = new Vector3(_roomLists[i0].playerObject.transform.position.x, _roomLists[i0].playerObject.transform.position.y, 10);
            eqvVec3[i0] = vecIndex;
        }

        return eqvVec3;
    }

    public void LayoutGrid()
    {
        // TO DO: Store size of the grid while generating levelData from levelDesigner
        // TO DO: Remove the hacky code and use the stored size of the grid

        if (GAMECONSTANTS.LEVEL_INDEX == 0)
        {
            targetGridParent.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            targetGridParent.GetComponent<GridLayoutGroup>().constraintCount = 3;
        }

        for (int i0=0;i0< levelData.listRooms.Count; i0++)
        {
           grid_TileComponent _gridTileComponent = GameObject.Instantiate(grid_tilePrefab, targetGridParent.transform);
            _gridTileComponent.grid_levelManager = this;
            _gridTileComponent.nativeIndex = i0;
            _roomLists.Add(_gridTileComponent);
        }

        Referencing();
    }

    public void Referencing()
    {
        for(int i0 = 0; i0 < _roomLists.Count; i0++)
        {
            if (!(levelData.listRooms[i0].connectors.NorthRoomIndex<0))
            {
                _roomLists[i0].northNeighbor = _roomLists[levelData.listRooms[i0].connectors.NorthRoomIndex];
            }

            if (!(levelData.listRooms[i0].connectors.SouthRoomIndex < 0))
            {
                _roomLists[i0].southNeighbor = _roomLists[levelData.listRooms[i0].connectors.SouthRoomIndex];
            }

            if (!(levelData.listRooms[i0].connectors.WestRoomIndex < 0))
            {
                _roomLists[i0].westNeighbor = _roomLists[levelData.listRooms[i0].connectors.WestRoomIndex];
            }

            if (!(levelData.listRooms[i0].connectors.EastRoomIndex < 0))
            {
                _roomLists[i0].eastNeighbor = _roomLists[levelData.listRooms[i0].connectors.EastRoomIndex];
            }

        }
    }

    public int _nextPointer;

    public void PreviousRoom()
    {
        _roomLists[playerCurretIndex].RoomLeftEvent();
    }

    public void ResetRoomsTo(int _resetIndex)
    {
        for (int i0 = 0; i0 < _roomLists.Count; i0++)
        {
            if (_roomLists[i0].dynamicIndex > _resetIndex)
            {
                _roomLists[i0].dynamicIndex = GAMECONSTANTS.UNVISITED_INDEX;
            }
        }
    }

    private void Start()
    {
        _indices = new List<int>();
        indicesAlreadyTraced = new List<int>();
    }

    void FancyLightUpRoom(grid_TileComponent _gridTile)
    {
        _gridTile.DressGridTile();
        StartCoroutine(_gridTile.FadeOut(_gridTile.levelLighting));
    }

    public void NextRoom()
    {
        _roomLists[playerCurretIndex].StartCoroutine(_roomLists[playerCurretIndex].FadingOut());
    }

    public void MoveTowards(Gamespace.EDirection _moveDirection)
    {
        grid_TileComponent gridTileComponent = _roomLists[playerCurretIndex];

        Hashtable hashtable = new Hashtable();
        Vector3 targetPos = Vector3.zero;

        switch (_moveDirection)
        {
            case EDirection.North:
                if(levelData.listRooms[playerCurretIndex].connectors.NorthRoomIndex!= GAMECONSTANTS.CONNECTION_BLOCKED)
                {
                    //hashtable.Add("position", locNorth);
                    PreviousRoom();
                    playerCurretIndex = levelData.listRooms[playerCurretIndex].connectors.NorthRoomIndex;
                    playerTrack.Add(playerCurretIndex);
                    AudioBank.audioInstance.Play(GAMECONSTANTS.WALK_CORRECT_EFFECT_STRING);

                }
                else
                {
                    AudioBank.audioInstance.Play(GAMECONSTANTS.WALK_WRONG_EFFECT_STRING);
                    return;
                }
                break;
            case EDirection.South:
                if (levelData.listRooms[playerCurretIndex].connectors.SouthRoomIndex != GAMECONSTANTS.CONNECTION_BLOCKED)
                {
                    //hashtable.Add("position", locSouth);

                    PreviousRoom();

                    playerCurretIndex = levelData.listRooms[playerCurretIndex].connectors.SouthRoomIndex;
                    playerTrack.Add(playerCurretIndex);
                    AudioBank.audioInstance.Play(GAMECONSTANTS.WALK_CORRECT_EFFECT_STRING);

                }
                else
                {
                    AudioBank.audioInstance.Play(GAMECONSTANTS.WALK_WRONG_EFFECT_STRING);

                    return;
                }
                break;
            case EDirection.East:
                if (levelData.listRooms[playerCurretIndex].connectors.EastRoomIndex != GAMECONSTANTS.CONNECTION_BLOCKED)
                {
                    //hashtable.Add("position", locEast);
                    PreviousRoom();
                    playerCurretIndex = levelData.listRooms[playerCurretIndex].connectors.EastRoomIndex;
                    playerTrack.Add(playerCurretIndex);
                    AudioBank.audioInstance.Play(GAMECONSTANTS.WALK_CORRECT_EFFECT_STRING);

                }
                else
                {
                    AudioBank.audioInstance.Play(GAMECONSTANTS.WALK_WRONG_EFFECT_STRING);

                    return;
                }
                break;
            case EDirection.West:
                if (levelData.listRooms[playerCurretIndex].connectors.WestRoomIndex != GAMECONSTANTS.CONNECTION_BLOCKED)
                {
                    //hashtable.Add("position", locWest);
                    PreviousRoom();
                    playerCurretIndex = levelData.listRooms[playerCurretIndex].connectors.WestRoomIndex;
                    playerTrack.Add(playerCurretIndex);
                    AudioBank.audioInstance.Play(GAMECONSTANTS.WALK_CORRECT_EFFECT_STRING);

                }
                else
                {
                    AudioBank.audioInstance.Play(GAMECONSTANTS.WALK_WRONG_EFFECT_STRING);

                    return;
                }
                break;
            default:
                break;




        }

            hashtable.Add("islocal", true);
            hashtable.Add("time", moveDuration);

            hashtable.Add("oncomplete", "NextRoom");
            hashtable.Add("oncompletetarget", this.gameObject);
            hashtable.Add("easetype", iTween.EaseType.linear);
            iTween.MoveTo(gridTileComponent.playerObject.gameObject, hashtable);
            btweening = true;

       

        // check if move possible

    }

    public Vector3 position;
    public GameObject selectedObject;


    private void Update()
    {if(selectedObject != null)
        {
            position = selectedObject.transform.localPosition;

        }
    }

}
