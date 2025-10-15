using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SpecialCoinPopup : MonoBehaviour
{
    public static SpecialCoinPopup Instance;

    [Header("UI")]
    public Image coinIcon;
    public TextMeshProUGUI counterText; 
    public float displayTime = 1.2f;

    private Coroutine currentPopup;

    private void Awake()
    {
        Instance = this;
        HidePopupInstant();
    }

    public void ShowPopup(int coinCount)
    {
        if (currentPopup != null)
            StopCoroutine(currentPopup);

        // Actualizar texto y mostrar
        if (counterText != null)
            counterText.text = "x" + coinCount;

        if (coinIcon != null)
            coinIcon.enabled = true;

        if (counterText != null)
            counterText.enabled = true;

        currentPopup = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        HidePopupInstant();
    }

    private void HidePopupInstant()
    {
        if (coinIcon != null)
            coinIcon.enabled = false;

        if (counterText != null)
            counterText.enabled = false;
    }
}
