using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (bgmSource1 == null)
        {
            bgmSource1 = gameObject.AddComponent<AudioSource>();
            bgmSource1.loop = true;
            bgmSource2 = gameObject.AddComponent<AudioSource>();
            bgmSource2.loop = true;
            bgmSource3 = gameObject.AddComponent<AudioSource>();
            bgmSource3.loop = true;
            bgmSource4 = gameObject.AddComponent<AudioSource>();
            bgmSource4.loop = true;
        }

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }
    #endregion

    #region Privates
    AudioSource bgmSource1, bgmSource2, bgmSource3, bgmSource4;
    AudioSource sfxSource;

    bool isBGMPlaying = false;
    float bgmFadeDuration = 0.5f;

    #endregion

    #region Dependencies
    [Header("Sound Database")]
    [SerializeField] SoundDB soundDB;
    #endregion

    #region Sound Play Util
    // MP3 Player   -> AudioSource
    // MP3 ����   -> AudioClip
    // ����(��)    -> AudioListner

    /// <summary>
    /// BGM ���
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName)
    {
        SoundData bgm = FindSound(soundDB.bgmList, bgmName); // �˻�

        if (isBGMPlaying){
            if(bgm.soundName == "BGM1"){
                StartCoroutine(FadeVolume(bgmSource1, bgm.volume));
                StartCoroutine(FadeVolume(bgmSource2, 0));
                StartCoroutine(FadeVolume(bgmSource3, 0));
                StartCoroutine(FadeVolume(bgmSource4, 0));
            }
            else if(bgm.soundName == "BGM3"){
                StartCoroutine(FadeVolume(bgmSource3, bgm.volume));
                StartCoroutine(FadeVolume(bgmSource1, 0));
            }
            else if(bgm.soundName == "BGM4"){
                StartCoroutine(FadeVolume(bgmSource4, bgm.volume));
                StartCoroutine(FadeVolume(bgmSource3, 0));
            }
            else if(bgm.soundName == "BGM2"){
                StartCoroutine(FadeVolume(bgmSource2, bgm.volume));
                StartCoroutine(FadeVolume(bgmSource4, 0));
            }
            else return;
        }

        
        else if (bgm != null)
        {
            bgmSource1.clip = FindSound(soundDB.bgmList, "BGM1").audioClip;
            bgmSource1.volume = 0;
            bgmSource1.Play();
            bgmSource2.clip = FindSound(soundDB.bgmList, "BGM2").audioClip;
            bgmSource2.volume = 0;
            bgmSource2.Play();
            bgmSource3.clip = FindSound(soundDB.bgmList, "BGM3").audioClip;
            bgmSource3.volume = 0;
            bgmSource3.Play();
            bgmSource4.clip = FindSound(soundDB.bgmList, "BGM4").audioClip;
            bgmSource4.volume = 0;
            bgmSource4.Play();

            if(bgm.soundName == "BGM1"){
                bgmSource1.volume = bgm.volume;
            }
            else if(bgm.soundName == "BGM2"){
                bgmSource2.volume = bgm.volume;
            }
            else if(bgm.soundName == "BGM3"){
                bgmSource3.volume = bgm.volume;
            }
            else if(bgm.soundName == "BGM4"){
                bgmSource4.volume = bgm.volume;
            }
        }
        else
        {
            Debug.Log($"Failed to find sound data_BGM : {bgmName}");
        }
        Debug.Log(bgm.soundName);
        isBGMPlaying = true;
    }


    public void StopBGM()
    {
        if (!isBGMPlaying)
            return;
        bgmSource1.Stop();
        bgmSource2.Stop();
        bgmSource3.Stop();
        bgmSource4.Stop();
        isBGMPlaying = false;
    }

    /// <summary>
    /// SFX ���
    /// </summary>
    /// <param name="sfxName"></param>
    public void PlaySFX(string sfxName)
    {
        SoundData sfx = FindSound(soundDB.sfxList, sfxName); // �˻�
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
        SoundData sfx = FindSound(soundDB.sfxList, sfxName); // �˻�
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
    /// SoundData �迭���� �ش� �̸��� sound data�� �˻�
    /// </summary>
    /// <param name="soundList"></param>
    /// <param name="soundName"></param>
    /// <returns></returns>
    private SoundData FindSound(SoundData[] soundList, string soundName)
    {
        foreach (SoundData sound in soundList)
        {
            if (sound.soundName == soundName) // �̸� ��
            {
                return sound;
            }
        }
        return null;
    }

    /*
    /// <summary>
    /// SFX ��� ���� ������ ���
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

    /// <summary>
    /// AudioSource의 볼륨을 서서히 변경
    /// </summary>
    /// <param name="audioSource">변경할 AudioSource</param>
    /// <param name="targetVolume">목표 볼륨 (0.0 ~ 1.0)</param>
    /// <param name="duration">볼륨 변경에 걸리는 시간 (초)</param>
    /// <returns>코루틴</returns>
    public IEnumerator FadeVolume(AudioSource audioSource, float targetVolume)
    {
        float duration = 0.5f;
        
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource가 null입니다.");
            yield break;
        }

        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 볼륨을 선형적으로 변화
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 볼륨 설정
        audioSource.volume = targetVolume;
    }

    #endregion
}