using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBank : MonoBehaviour
{
    public static AudioBank audioInstance;

    public AudioClip typewriterEffect_Audio;
    [SerializeField] AudioClip walkingEffect_Audio;
    [SerializeField] AudioClip homeEffect_Audio;
    [SerializeField] AudioClip wrongDirection_Audio;

    public void Play(string _audioClipName)
    {
        if(_audioClipName == Gamespace.GAMECONSTANTS.TYPE_EFFECT_STRING)
        {
            PlaySound(typewriterEffect_Audio, true);
        }
        else if (_audioClipName == Gamespace.GAMECONSTANTS.WALK_CORRECT_EFFECT_STRING)
        {
            PlaySound(walkingEffect_Audio);

        }
        else if (_audioClipName == Gamespace.GAMECONSTANTS.WALK_WRONG_EFFECT_STRING)
        {
            PlaySound(wrongDirection_Audio);

        }
        else if (_audioClipName == Gamespace.GAMECONSTANTS.HOME_EFFECT_STRING)
        {
            PlaySound(homeEffect_Audio);

        }
    }


    private void Awake()
    {
        if(audioInstance == null)
        {
            audioInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    const int NO_FREE_SOURCE = -1;
    public List<AudioSource> audioSources;

    int GetFreeAudioSourceIndex()
    {
        for (int i0 = 0; i0 < audioSources.Count; i0++)
        {
            if (audioSources[i0].isPlaying == false)
            {
                return i0;
            }
        }

        return NO_FREE_SOURCE;
    }

    public void PlaySound(AudioClip _audioClip, bool bShouldLoop = false)
    {
        int _targetSourceIndex = NO_FREE_SOURCE;
        _targetSourceIndex = GetFreeAudioSourceIndex();

        if (audioSources[_targetSourceIndex] != null)
        {
            audioSources[_targetSourceIndex].clip = _audioClip;
            audioSources[_targetSourceIndex].loop = bShouldLoop;
            audioSources[_targetSourceIndex].Play();
        }
    }

}
