using System.Collections;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    private void Update()
    {
        // 아무 키나 눌렀을 때
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
    }

    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private float fadeDuration = 1f; // 페이드 인/아웃 지속 시간
    [SerializeField] private float pauseDuration = 0.5f; // 페이드 사이의 대기 시간

    private Coroutine fadeCoroutine;

    private void Start()
    {
        if (targetText != null)
        {
            StartFadeLoop(); // 스크립트 시작 시 페이드 효과 시작
        }
        StartCoroutine(ExecuteAfterDelay());

    }

    /// <summary>
    /// 페이드 효과를 루프로 시작
    /// </summary>
    public void StartFadeLoop()
    {
        if (fadeCoroutine == null)
        {
            fadeCoroutine = StartCoroutine(FadeLoop());
        }
    }

    /// <summary>
    /// 페이드 효과를 중지
    /// </summary>
    public void StopFadeLoop()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            // Fade Out
            yield return StartCoroutine(FadeTextAlpha(1f, 0f));
            yield return new WaitForSeconds(pauseDuration); // 페이드 아웃 후 대기

            // Fade In
            yield return StartCoroutine(FadeTextAlpha(0f, 1f));
            yield return new WaitForSeconds(pauseDuration); // 페이드 인 후 대기
        }
    }

    private IEnumerator FadeTextAlpha(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color textColor = targetText.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            targetText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 알파값 보정
        targetText.color = new Color(textColor.r, textColor.g, textColor.b, endAlpha);
    }

    private IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(0.2f); // 1초 대기
        SoundManager.Instance.PlayBGM("BGM1");
    }
}
