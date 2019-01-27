using Gamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTypewriterEffect : MonoBehaviour
{
    [SerializeField]
    float playSpeed = 0.125f;
    [SerializeField]
    bool bPlayOnAwake = false;
    [SerializeField]
    bool bIsPlaying = false;

    [SerializeField]
    bool bIsComplete = false;

    [SerializeField]
    Button nextBtn;


    Text txt;
    string story;

    private void Awake()
    {
        bIsComplete = false;

        if (bPlayOnAwake)
        {
            StartPlayEffectRoutine();
        }
    }

    public void StartPlayEffectRoutine()
    {
        if (!bIsPlaying)
        {
            AudioBank.audioInstance.Play(GAMECONSTANTS.TYPE_EFFECT_STRING);

            txt = GetComponent<Text>();
            story = txt.text;
            txt.text = "";

            StartCoroutine("PlayText");
            bIsPlaying = true;
        }
        
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(playSpeed);
            if(txt.text == story)
            {
                OnComplete();
            }
        }
    }

    public void OnSkipped()
    {
        Debug.Log("SKIPPED");

    }

    public void OnComplete()
    {
        bIsComplete = true;

        for (int i0 = 0; i0 < AudioBank.audioInstance.audioSources.Count; i0++)
        {
            if (AudioBank.audioInstance.audioSources[i0].isPlaying
                && AudioBank.audioInstance.audioSources[i0].clip
                == AudioBank.audioInstance.typewriterEffect_Audio)
            {
                AudioBank.audioInstance.audioSources[i0].Stop();
            }
        }

        Debug.Log("COMPLETE");
    }


}


