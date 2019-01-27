using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour
{

    [SerializeField]
    Button homebtn;

    [SerializeField]
    float btnListernLoadTime;

    [SerializeField]
    float buttonLoadTime;

    private void Start()
    {
        StartCoroutine(FadingInHomeBtn());
    }

    private YieldInstruction fadeInstruction = new YieldInstruction();

    IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < buttonLoadTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / buttonLoadTime);
            image.color = c;
        }
    }

    IEnumerator FadingInHomeBtn()
    {
        StartCoroutine(FadeIn(homebtn.GetComponent<Image>()));
        yield return new WaitForSeconds(buttonLoadTime);
        StartCoroutine(AddListnerHomeBtn());
    }

    IEnumerator AddListnerHomeBtn()
    {
        yield return new WaitForSeconds(btnListernLoadTime);
        homebtn.onClick.AddListener(() => OnClickHome());
    }

    public void OnClickHome()
    {
        SceneManager.LoadScene(Gamespace.GAMECONSTANTS.HOME_SCENE);
    }

}
