using Gamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class grid_TileComponent : MonoBehaviour
{
    public grid_LevelManager grid_levelManager;

    public int nativeIndex;
    public int dynamicIndex = GAMECONSTANTS.UNVISITED_INDEX;

    public grid_TileComponent northNeighbor;
    public grid_TileComponent southNeighbor;
    public grid_TileComponent eastNeighbor;
    public grid_TileComponent westNeighbor;

    public Image northBlock;
    public Image southBlock;
    public Image eastBlock;
    public Image westBlock;
    public Image specialPoint;
    public Image playerObject;

    public Image levelLighting;

    private void Start()
    {
        dynamicIndex = GAMECONSTANTS.UNVISITED_INDEX;
    }

    private void UpdateLightingColor(Color color)
    {
        levelLighting.color = color;
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


    IEnumerator ProceedToNextLevel()
    {
        StartCoroutine(grid_levelManager. PlayLine());
        yield return new WaitForSeconds(5f);

        
    }




    public void DressGridTile()
    {
        if(GAMECONSTANTS.LastRoomIndex == nativeIndex)
        {
            specialPoint.gameObject.SetActive(true);
            specialPoint.sprite = grid_levelManager.EndSprite;

            AudioBank.audioInstance.Play(GAMECONSTANTS.HOME_EFFECT_STRING);

            GAMECONSTANTS.ROUND_WON = true;
            StartCoroutine(ProceedToNextLevel());

        }
        else
        {
            specialPoint.sprite = null;
            specialPoint.gameObject.SetActive(false);
        }

        //playerObject.gameObject.transform.localPosition = grid_levelManager.originalLoc;
        playerObject.gameObject.SetActive(true);


        if (grid_levelManager.levelData.listRooms[nativeIndex].connectors.NorthRoomIndex 
            == GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            northBlock.gameObject.SetActive(false);
        }
        else
        {
            northBlock.gameObject.SetActive(true);
        }

        if (grid_levelManager.levelData.listRooms[nativeIndex].connectors.SouthRoomIndex
           == GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            southBlock.gameObject.SetActive(false);
        }
        else
        {
            southBlock.gameObject.SetActive(true);
        }

        if (grid_levelManager.levelData.listRooms[nativeIndex].connectors.WestRoomIndex
           == GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            westBlock.gameObject.SetActive(false);
        }
        else
        {
            westBlock.gameObject.SetActive(true);
        }

        if (grid_levelManager.levelData.listRooms[nativeIndex].connectors.EastRoomIndex
           == GAMECONSTANTS.CONNECTION_BLOCKED)
        {
            eastBlock.gameObject.SetActive(false);
        }
        else
        {
            eastBlock.gameObject.SetActive(true);
        }

        // TO DO: Fix this hacky code to position player object properly
        // TO DO: Remove the tweening code of player objects

        


  
    }

    void LightsOffEventComplete()
    {
        grid_levelManager.btweening = false;
    }

    void LightsOnEventComplete()
    {
        grid_levelManager.btweening = false;

        // lights are on
    }

    private YieldInstruction fadeInstruction = new YieldInstruction();
    public IEnumerator FadeOut(Image image)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < grid_levelManager.lightingDuration)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / grid_levelManager.lightingDuration);
            image.color = c;
        }
    }

    IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < grid_levelManager.lightingDuration)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / grid_levelManager.lightingDuration);
            image.color = c;
        }
    }



    public IEnumerator FadingIn( )
    {
        StartCoroutine(FadeIn(levelLighting));
        grid_levelManager.btweening = true;
        yield return new WaitForSeconds(grid_levelManager.lightingDuration);
        grid_levelManager.btweening = false;

        LightsOffEventComplete();
    }

    public void RoomLeftEvent()
    {
        levelLighting.color = new Color(0, 0, 0,1);
        levelLighting.gameObject.SetActive(true);
        //playerObject.gameObject.transform.localPosition = grid_levelManager.originalLoc;
    }



    public IEnumerator FadingOut( )
    {
        DressGridTile();
        grid_levelManager.btweening = true;
        StartCoroutine(FadeOut(levelLighting));
        yield return new WaitForSeconds(grid_levelManager.lightingDuration);

        if(this.dynamicIndex == GAMECONSTANTS.UNVISITED_INDEX)
        {
            this.dynamicIndex = GAMECONSTANTS.AssignIndex();
        }

        GAMECONSTANTS.SetTrackerIndex(this.dynamicIndex);
        grid_levelManager.ResetRoomsTo(GAMECONSTANTS.PLAYER_PROGRESS_TRACK);
        grid_levelManager.Text_currentProgressTracker.text = "Step : " + GAMECONSTANTS.PLAYER_PROGRESS_TRACK.ToString();

        grid_levelManager.btweening = false;
        LightsOnEventComplete();
    }

}
