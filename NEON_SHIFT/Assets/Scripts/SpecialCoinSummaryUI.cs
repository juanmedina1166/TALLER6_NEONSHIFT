using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialCoinSummaryUI : MonoBehaviour
{
    public static SpecialCoinSummaryUI Instance;

    [Header("Referencias UI")]
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public Image[] coinIcons; // Asigna 3 imágenes en el inspector
    public Sprite coinCollectedSprite;
    public Sprite coinEmptySprite;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    /// <summary>
    /// Muestra las monedas especiales de un nivel específico.
    /// </summary>
    public void ShowForLevel(int levelIndex)
    {
        if (SaveManager.Instance == null) return;

        panel.SetActive(true);
        titleText.text = $"Monedas especiales del Nivel {levelIndex - 1}";

        for (int i = 0; i < coinIcons.Length; i++)
        {
            bool collected = SaveManager.Instance.IsSpecialCoinCollected(levelIndex, i);
            coinIcons[i].sprite = collected ? coinCollectedSprite : coinEmptySprite;
            coinIcons[i].color = collected ? Color.white : new Color(1, 1, 1, 0.4f);
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
