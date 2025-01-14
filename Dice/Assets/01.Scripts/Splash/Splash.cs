using System.Collections;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public void OnStartClick(){
        SceneManager.LoadScene(1);
    }


    [SerializeField] private GameObject howtoplayGameObject; 

    public void OnHowToPlayClick(){
        howtoplayGameObject.SetActive(true);
    }

    public void OnNextClick(int num){
        howtoplayGameObject.transform.GetChild(num - 1).gameObject.SetActive(false);
        howtoplayGameObject.transform.GetChild(num).gameObject.SetActive(true);
    }

    public void OnPrevClick(int num){
        howtoplayGameObject.transform.GetChild(num - 1).gameObject.SetActive(false);
        howtoplayGameObject.transform.GetChild(num - 2).gameObject.SetActive(true);
    }

    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private float fadeDuration = 1f; // ���̵� ��/�ƿ� ���� �ð�
    [SerializeField] private float pauseDuration = 0.5f; // ���̵� ������ ��� �ð�

    private Coroutine fadeCoroutine;

    private void Start()
    {
        if (targetText != null)
        {
            StartFadeLoop(); // ��ũ��Ʈ ���� �� ���̵� ȿ�� ����
        }
        StartCoroutine(ExecuteAfterDelay());

    }

    /// <summary>
    /// ���̵� ȿ���� ������ ����
    /// </summary>
    public void StartFadeLoop()
    {
        if (fadeCoroutine == null)
        {
            fadeCoroutine = StartCoroutine(FadeLoop());
        }
    }

    /// <summary>
    /// ���̵� ȿ���� ����
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
            yield return new WaitForSeconds(pauseDuration); // ���̵� �ƿ� �� ���

            // Fade In
            yield return StartCoroutine(FadeTextAlpha(0f, 1f));
            yield return new WaitForSeconds(pauseDuration); // ���̵� �� �� ���
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

        // ���� ���İ� ����
        targetText.color = new Color(textColor.r, textColor.g, textColor.b, endAlpha);
    }

    private IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(0.2f); // 1�� ���
        SoundManager.Instance.PlayBGM("BGM1");
    }
}
