using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Serialized Fields
    [Header("Color Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color pressedColor = Color.gray;

    [Header("Scale Settings")]
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 hoverScale = Vector3.one * 1.1f;
    [SerializeField] private Vector3 pressedScale = Vector3.one * 0.9f;

    [Header("Target Settings")]
    [SerializeField] private Image targetImage; // 버튼의 색상을 변경할 대상
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // 기본 설정
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        ResetToNormal();
    }
    #endregion

    #region Pointer Event Handlers
    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplyHoverEffect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetToNormal();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ApplyPressedEffect();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ApplyHoverEffect(); // 마우스가 여전히 버튼 위에 있을 경우
    }
    #endregion

    #region Effect Methods
    private void ApplyHoverEffect()
    {
        if (targetImage != null)
            targetImage.color = hoverColor;

        transform.localScale = hoverScale;
    }

    private void ApplyPressedEffect()
    {
        if (targetImage != null)
            targetImage.color = pressedColor;

        transform.localScale = pressedScale;
    }

    private void ResetToNormal()
    {
        if (targetImage != null)
            targetImage.color = normalColor;

        transform.localScale = normalScale;
    }
    #endregion
}
