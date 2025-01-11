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
        // �ƹ� Ű�� ������ ��
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }

        timer += Time.deltaTime;

        // �ֱ������� Ȱ��ȭ/��Ȱ�� ���
        if (timer >= blinkInterval)
        {
            text.SetActive(!text.activeSelf); // Ȱ��/��Ȱ�� ��ȯ
            timer = 0f; // Ÿ�̸� �ʱ�ȭ
        }
    }
}
