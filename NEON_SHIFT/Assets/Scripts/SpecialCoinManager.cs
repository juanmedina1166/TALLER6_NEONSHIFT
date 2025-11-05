using UnityEngine;
using UnityEngine.UI;

public class SpecialCoinManager : MonoBehaviour
{
    public static SpecialCoinManager Instance;

    [Header("UI")]
    public Image[] coinIcons;
    public Sprite filledSprite;
    public Sprite emptySprite;

    private void Awake()
    {
        Instance = this;
    }

    // --- ¡¡INICIO DEL CAMBIO!! ---
    // Ahora aceptamos el índice del nivel como parámetro
    public void UpdateUI(int levelIndex)
    {
        // Leemos desde SaveManager (persistente) en lugar de SpecialCoinTracker (sesión)
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("SaveManager no encontrado. No se puede actualizar UI de monedas especiales.");
            return;
        }

        for (int i = 0; i < coinIcons.Length; i++)
        {
            // Comprobamos si la moneda 'i' del nivel 'levelIndex' está guardada
            bool collected = SaveManager.Instance.IsSpecialCoinCollected(levelIndex, i);
            coinIcons[i].sprite = collected ? filledSprite : emptySprite;
            coinIcons[i].color = Color.white;
        }
    }
    // --- ¡¡FIN DEL CAMBIO!! ---
}
