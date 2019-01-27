using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LoadLevels : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(Gamespace.GAMECONSTANTS.INGAME_SCENE);
    }
  }
