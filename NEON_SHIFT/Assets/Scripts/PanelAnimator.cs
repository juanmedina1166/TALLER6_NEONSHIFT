using UnityEngine;
using DG.Tweening;
using System; // ¡Importante! Necesario para usar 'Action'

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class PanelAnimator : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    [Header("Configuración de Animación")]
    public float duration = 0.5f;
    public Ease easeType = Ease.OutBack;

    [Header("Estado Inicial (Oculto)")]
    [Tooltip("El offset de la posición inicial. (0, 1000) para arriba, (0, -1000) para abajo.")]
    public Vector2 startPositionOffset = Vector2.zero;

    [Tooltip("La escala inicial. Ej: 0.8 para 'Pop-up'")]
    public float startScale = 0.8f;

    [Tooltip("El alpha inicial. 0 para 'Fade-in'")]
    [Range(0f, 1f)]
    public float startAlpha = 0f;

    // Guardamos los valores originales (visibles)
    private Vector2 originalPosition;
    private Vector3 originalScale;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        // 1. Guarda los valores "finales" (como se ve en el editor)
        originalPosition = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;

        // 2. Establece el estado "inicial" (oculto)
        // Esto prepara el panel antes de que se muestre por primera vez
        canvasGroup.alpha = startAlpha;
        rectTransform.localScale = originalScale * startScale;
        rectTransform.anchoredPosition = originalPosition + startPositionOffset;

        // 3. Empieza desactivado
        gameObject.SetActive(false);
    }

    // ¡NUEVO! Ahora acepta un 'Action' (callback) opcional
    public void ShowPanel(Action onComplete = null)
    {
        gameObject.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Usamos una Secuencia para animar todo junto
        // y poder asignar un OnComplete general.
        Sequence seq = DOTween.Sequence();

        // Anima todo de vuelta a su estado original
        seq.Join(canvasGroup.DOFade(1f, duration));
        seq.Join(rectTransform.DOScale(originalScale, duration));
        seq.Join(rectTransform.DOAnchorPos(originalPosition, duration));

        // Configura la secuencia
        seq.SetEase(easeType);
        seq.SetUpdate(true); // ¡Importante! Ignora Time.timeScale

        // Cuando TODA la secuencia termine, llama al callback 'onComplete'
        seq.OnComplete(() => {
            onComplete?.Invoke(); // '?' significa: si onComplete no es null, ejecútalo
        });
    }

    // ¡NUEVO! También acepta un 'Action' (callback) opcional
    public void HidePanel(Action onComplete = null)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Sequence seq = DOTween.Sequence();

        // Anima todo de vuelta al estado "inicial" (oculto)
        seq.Join(canvasGroup.DOFade(startAlpha, duration));
        seq.Join(rectTransform.DOScale(originalScale * startScale, duration));
        seq.Join(rectTransform.DOAnchorPos(originalPosition + startPositionOffset, duration));

        // Configura la secuencia
        seq.SetEase(easeType); // Puedes usar un Ease diferente para "ocultar"
        seq.SetUpdate(true);

        // Cuando termine, desactiva el objeto y llama al callback
        seq.OnComplete(() => {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }
}