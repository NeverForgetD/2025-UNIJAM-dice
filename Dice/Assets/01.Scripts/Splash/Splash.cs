using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    [SerializeField] GameObject text;
    private float timer = 0f;
    [SerializeField] private float blinkInterval = 0.5f;

    private void Update()
    {
        // 아무 키나 눌렀을 때
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }

        timer += Time.deltaTime;

        // 주기적으로 활성화/비활성 토글
        if (timer >= blinkInterval)
        {
            text.SetActive(!text.activeSelf); // 활성/비활성 전환
            timer = 0f; // 타이머 초기화
        }
    }
}
