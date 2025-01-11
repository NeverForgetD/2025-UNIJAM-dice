using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleFactor = 1.05f; // Ŀ�� ���� (�⺻�� 1.1)
    [SerializeField] private float animationDuration = 0.08f; // �ִϸ��̼� ���� �ð�
    private Vector3 originalScale; // ���� ũ�� ����

    private void Awake()
    {
        // ���� ũ�� ����
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���콺 ���� �� ũ�⸦ Ű��
        StopAllCoroutines(); // �ߺ� �ִϸ��̼� ����
        StartCoroutine(ScaleTo(originalScale * scaleFactor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ���콺�� ����� �� ���� ũ��� ����
        StopAllCoroutines(); // �ߺ� �ִϸ��̼� ����
        StartCoroutine(ScaleTo(originalScale));
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startingScale = transform.localScale;

        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ũ�� ����
        transform.localScale = targetScale;
    }
}
