using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamespace;
using LevelDesignspace;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public CameraController cameraController;

    public int _currentRoomIndex;
    public int _playerTracks;

    public Text _playerTrack;

    [SerializeField]
    InputController inputController;

    [SerializeField] float fadeTime;

    public LevelData levelData;

    [SerializeField] Image fadeImage;

    [SerializeField] GameObject northBlocker;
    [SerializeField] GameObject southBlocker;
    [SerializeField] GameObject westBlocker;
    [SerializeField] GameObject eastBlocker;

    [SerializeField] GameObject homeObject;
    [SerializeField] GameObject playerObject;

    void DressRoom(Room _roomSettings)
    {
        if (_roomSettings.connectors.NorthRoomIndex != GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            northBlocker.gameObject.SetActive(false);
        }

        if (_roomSettings.connectors.SouthRoomIndex != GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            southBlocker.gameObject.SetActive(false);
        }

        if (_roomSettings.connectors.WestRoomIndex != GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            westBlocker.gameObject.SetActive(false);
        }

        if (_roomSettings.connectors.EastRoomIndex != GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            eastBlocker.gameObject.SetActive(false);
        }

        playerObject.gameObject.SetActive(true);
    }

    // Intrinsic roomIndex :: not to be confused with dynamic index of the room
    void EnterRoom(int _roomIndex)
    {
        _currentRoomIndex = _roomIndex;

        GAMECONSTANTS.PrintConsole("Welcome to Room: " + _roomIndex);

        Room _currentRoom = new Room();
        _currentRoom = levelData.listRooms[_roomIndex];

        DressRoom(_currentRoom);

        if (GAMECONSTANTS.IsLastRoom(_roomIndex))
        {
            homeObject.gameObject.SetActive(true);
            GAMECONSTANTS.PrintConsole("insert code for win event");
        }

        if (_currentRoom.dynamicIndex == GAMECONSTANTS.UNVISITED_INDEX)
        {
            _currentRoom.SetRoomDynamicIndex();
            GAMECONSTANTS.PrintConsole("insert code for entering a new room event");
        }
        else
        {
            GAMECONSTANTS.SetTrackerIndex(_currentRoom.dynamicIndex);
            levelData.ResetRoomsToIndex(GAMECONSTANTS.PLAYER_PROGRESS_TRACK);
            GAMECONSTANTS.PrintConsole("insert code for returning to a room that has been traversed already event");
        }
    }

    public void ClearRoom()
    {
        homeObject.gameObject.SetActive(false);
        playerObject.gameObject.SetActive(false);
        northBlocker.gameObject.SetActive(true);
        southBlocker.gameObject.SetActive(true);
        westBlocker.gameObject.SetActive(true);
        eastBlocker.gameObject.SetActive(true);
    }

    public void LoadFirstRoom()
    {
        StartCoroutine(PlayEnteringRoomCue(levelData.startRoomIndex));
    }

    void HeadNorth()
    {
        if(levelData.listRooms[_currentRoomIndex].connectors.NorthRoomIndex 
            != GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            StartCoroutine(PlayExitingRoomCue(levelData.listRooms[_currentRoomIndex].connectors.NorthRoomIndex));
            cameraController.PanCamera(EPanningDirection.UP);
        }
    }
    void HeadSouth()
    {
        if (levelData.listRooms[_currentRoomIndex].connectors.SouthRoomIndex
            != GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            StartCoroutine(PlayExitingRoomCue(levelData.listRooms[_currentRoomIndex].connectors.SouthRoomIndex));
            cameraController.PanCamera(EPanningDirection.DOWN);
        }
    }
    void HeadEast()
    {
        if (levelData.listRooms[_currentRoomIndex].connectors.EastRoomIndex
            != GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            StartCoroutine(PlayExitingRoomCue(levelData.listRooms[_currentRoomIndex].connectors.EastRoomIndex));
            cameraController.PanCamera(EPanningDirection.RIGHT);
        }
    }
    void HeadWest()
    {
        if (levelData.listRooms[_currentRoomIndex].connectors.WestRoomIndex
            != GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            StartCoroutine(PlayExitingRoomCue(levelData.listRooms[_currentRoomIndex].connectors.WestRoomIndex));
            cameraController.PanCamera(EPanningDirection.LEFT);
        }
    }

    public void DirectTo(EDirection _direction)
    {
        switch (_direction)
        {
            case EDirection.North:
                HeadNorth();
                break;
            case EDirection.South:
                HeadSouth();
                break;
            case EDirection.East:
                HeadEast();
                break;
            case EDirection.West:
                HeadWest();
                break;
            default:
                GAMECONSTANTS.PrintConsole("PLEASE CLICK ON A DIRECTION KEY TO CONTINUE");
                break;
        }
    }

    IEnumerator PlayExitingRoomCue(int _nextRoomIndex)
    {
        StartCoroutine(FadeIn(fadeImage));
        yield return new WaitForSeconds(fadeTime);
        ClearRoom();
        StartCoroutine(PlayEnteringRoomCue(_nextRoomIndex));
    }
    IEnumerator PlayEnteringRoomCue(int _enteredRoomIndex)
    {
        inputController.bDisableController = true;
        EnterRoom(_enteredRoomIndex);
        StartCoroutine(FadeOut(fadeImage));
        yield return new WaitForSeconds(fadeTime);
        _playerTrack.text = (GAMECONSTANTS.PLAYER_PROGRESS_TRACK).ToString();
        inputController.bDisableController = false;
    }

    private YieldInstruction fadeInstruction = new YieldInstruction();
    IEnumerator FadeOut(Image image)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
            image.color = c;
        }
    }

    IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeTime);
            image.color = c;
        }
    }



    IEnumerator FadingIn(int _nextRoomIndex)
    {
        StartCoroutine(FadeIn(fadeImage));
        yield return new WaitForSeconds(fadeTime);
        ClearRoom();
        StartCoroutine(PlayEnteringRoomCue(_nextRoomIndex));
    }


    IEnumerator FadingOut(int _enteredRoomIndex)
    {
        inputController.bDisableController = true;
        EnterRoom(_enteredRoomIndex);
        StartCoroutine(FadeOut(fadeImage));
        yield return new WaitForSeconds(fadeTime);
        _playerTrack.text = (GAMECONSTANTS.PLAYER_PROGRESS_TRACK).ToString();
        inputController.bDisableController = false;
    }



}
