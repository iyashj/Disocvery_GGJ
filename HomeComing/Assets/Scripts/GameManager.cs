using Gamespace;
using LevelDesignspace;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float _waitTime = 1.5f;
    private bool bCanNext = true;

    public LevelInfo levelInfo;
    public LevelManager levelManager;
    public grid_LevelManager grid_levelManager;

    public Button nextLineCHevronBtn;
    public Text dialogueLine;
    public Image dialogueBG;

    void Start()
    {
        bCanNext = true;
        dialogueCharges = 1;
        //StartCoroutine(FadingIn(nextLineCHevronBtn.image, _waitTime));
        GetNextLine();

        nextLineCHevronBtn.onClick.AddListener(() => GetNextLine());

    }

    private int dialogueCharges;

    private YieldInstruction fadeInstruction = new YieldInstruction();
    IEnumerator FadeIn(Image image, float _waitTime)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < _waitTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / _waitTime);
            image.color = c;
        }
    }
    public IEnumerator FadingIn(Image _fadeInImage, float _waitDuration)
    {
        StartCoroutine(FadeIn(_fadeInImage, _waitDuration));
        yield return new WaitForSeconds(_waitDuration);
        bCanNext = true;
    }

    void GetNextLine()
    {
        if (bCanNext && dialogueCharges>0)
        {
            dialogueCharges--;
            dialogueLine.text = levelInfo.discoveryData.levelDialogues[Gamespace.GAMECONSTANTS.LEVEL_INDEX];
            dialogueLine.GetComponent<TextTypewriterEffect>().StartPlayEffectRoutine();

            bCanNext = false;
            StartCoroutine(FadingIn(nextLineCHevronBtn.image, _waitTime));
        }
        else if (dialogueCharges <= 0)
        {
            dialogueBG.gameObject.SetActive(false);
            dialogueLine.gameObject.SetActive(false);
            nextLineCHevronBtn.gameObject.SetActive(false);

            StartGame();
        }
    }

    void StartGame()
    {
        grid_levelManager.levelData = new LevelData(GAMECONSTANTS.GetLevelData());
        grid_levelManager.StartLevel();
        GAMECONSTANTS.ROUND_WON = false;
    }
}
