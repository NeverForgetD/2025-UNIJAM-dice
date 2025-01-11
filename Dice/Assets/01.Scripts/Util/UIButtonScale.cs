using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleFactor = 1.05f; // 커질 배율 (기본값 1.1)
    [SerializeField] private float animationDuration = 0.08f; // 애니메이션 지속 시간
    private Vector3 originalScale; // 원래 크기 저장

    private void Awake()
    {
        // 원래 크기 저장
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스 오버 시 크기를 키움
        StopAllCoroutines(); // 중복 애니메이션 방지
        StartCoroutine(ScaleTo(originalScale * scaleFactor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 벗어났을 때 원래 크기로 복귀
        StopAllCoroutines(); // 중복 애니메이션 방지
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

        // 최종 크기 보정
        transform.localScale = targetScale;
    }
}
