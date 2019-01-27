using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConclusionScript : MonoBehaviour
{
    private int charges = 0;
    [SerializeField]
    LevelInfo levelInfo;
    [SerializeField]
    float waitTime = 0.4f;
    [SerializeField]
    private bool clickReady = true;


    public void OnClickNext()
    {
        if (clickReady)
        {
            if (charges < levelInfo.discoveryData.conclusionLines.Count)
            {
                palletText.text = levelInfo.discoveryData.conclusionLines[charges];
                palletText.GetComponent<TextTypewriterEffect>().StartPlayEffectRoutine();
                StartCoroutine(blockInput());
                charges++;
            }
            else
            {
                SceneManager.LoadScene(Gamespace.GAMECONSTANTS.CREDITS_SCENE);
            }
        }
    }

    private IEnumerator blockInput()
    {
        clickReady = false;
        yield return new WaitForSeconds(waitTime);
        clickReady = true;
    }

    public Text palletText;
    public Image palletImage;
    public Button palletChevron;

    private void Start()
    {
        clickReady = true;
        OnClickNext();
    }

}
