using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton Init
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitAudioSource();
    }

    public void InitAudioSource()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;

            bgmSource1 = gameObject.AddComponent<AudioSource>();
            bgmSource1.loop = true;

            bgmSource2 = gameObject.AddComponent<AudioSource>();
            bgmSource2.loop = true;
        }

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }
    #endregion

    #region Privates
    AudioSource bgmSource;
    AudioSource bgmSource1;
    AudioSource bgmSource2;

    AudioSource sfxSource;

    bool isBGMPlaying = false;
    #endregion

    #region Dependencies
    [Header("Sound Database")]
    [SerializeField] SoundDB soundDB;
    #endregion

    #region Sound Play Util
    // MP3 Player   -> AudioSource
    // MP3 음원   -> AudioClip
    // 관객(귀)    -> AudioListner

    /// <summary>
    /// BGM 재생
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName)
    {
        if (isBGMPlaying)
            return;

        SoundData bgm = FindSound(soundDB.bgmList, bgmName); // 검색
        if (bgm != null)
        {
            bgmSource.clip = bgm.audioClip;
            bgmSource.volume = bgm.volume;
            bgmSource.Play();
        }
        else
        {
            Debug.Log($"Failed to find sound data_BGM : {bgmName}");
        }
        isBGMPlaying = true;
    }

    // 테스트 용 여러 BGM, 리팩토링 필수
    public void PlayBGM1(string bgmName)
    {
        SoundData bgm = FindSound(soundDB.bgmList, bgmName); // 검색
        if (bgm != null)
        {
            bgmSource1.clip = bgm.audioClip;
            bgmSource1.volume = bgm.volume;
            bgmSource1.Play();
        }
    }
    public void playBGM2(string bgmName)
    {
        SoundData bgm = FindSound(soundDB.bgmList, bgmName); // 검색
        if (bgm != null)
        {
            bgmSource.clip = bgm.audioClip;
            bgmSource.volume = bgm.volume;
            bgmSource.Play();
        }
    }


    public void StopBGM()
    {
        if (!isBGMPlaying)
            return;
        bgmSource.Stop();
        bgmSource1.Stop();
        bgmSource2.Stop();
        isBGMPlaying = false;
    }

    /// <summary>
    /// SFX 재생
    /// </summary>
    /// <param name="sfxName"></param>
    public void PlaySFX(string sfxName)
    {
        SoundData sfx = FindSound(soundDB.sfxList, sfxName); // 검색
        if (sfx == null)
        {
            Debug.Log($"Failed to find sound data_SFX : {sfxName}");
        }
        else
        {
            sfxSource.pitch = sfx.pitch;
            sfxSource.volume = sfx.volume;
            sfxSource.PlayOneShot(sfx.audioClip);

        }
    }
    public void PlaySFX_RandomPitch(string sfxName, float minPitch, float maxPitch)
    {
        SoundData sfx = FindSound(soundDB.sfxList, sfxName); // 검색
        if (sfx == null)
        {
            Debug.Log($"Failed to find sound data_SFX : {sfxName}");
        }
        else
        {
            sfxSource.pitch = Random.Range(minPitch, maxPitch);
            sfxSource.volume = sfx.volume;
            sfxSource.PlayOneShot(sfx.audioClip);

        }
    }

    /// <summary>
    /// SoundData 배열에서 해당 이름의 sound data를 검색
    /// </summary>
    /// <param name="soundList"></param>
    /// <param name="soundName"></param>
    /// <returns></returns>
    private SoundData FindSound(SoundData[] soundList, string soundName)
    {
        foreach (SoundData sound in soundList)
        {
            if (sound.soundName == soundName) // 이름 비교
            {
                return sound;
            }
        }
        return null;
    }

    /*
    /// <summary>
    /// SFX 재생 끝날 때까지 대시
    /// </summary>
    /// <returns></returns>
    public async Task WaitForSfxEnd()
    {
        while (sfxSource.isPlaying)
        {
            await Task.Yield();
        }
    }
    */
    #endregion
}