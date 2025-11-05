using UnityEngine;
using TMPro; // ¡Necesitas esta línea para usar TextMeshPro!

public class MostrarMonedasGlobales : MonoBehaviour
{
    // Arrastra el componente TextMeshPro que creaste en el inspector
    public TextMeshProUGUI normalCoinText;
    public TextMeshProUGUI specialCoinText;

    void OnEnable()
    {
        // Esto asegura que el texto se actualice CADA VEZ que el jugador ve este panel.
        UpdateCoinDisplay();
    }

    // Método que puedes llamar cada vez que las monedas cambien
    public void UpdateCoinDisplay()
    {
        // 1. Obtener la cantidad de monedas usando tu SaveManager
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("SaveManager no encontrado. No se puede actualizar UI de monedas.");
            return;
        }

        // --- 1. Actualizar Monedas Normales ---
        if (normalCoinText != null)
        {
            int currentNormalCoins = SaveManager.Instance.GetNormalCoins();
            normalCoinText.text = currentNormalCoins.ToString();
        }

        // --- 2. Actualizar Monedas Especiales ---
        if (specialCoinText != null)
        {
            // Obtenemos los totales del SaveManager
            int totalLevels = SaveManager.Instance.totalLevelsInGame;
            int coinsPerLevel = SaveManager.Instance.specialCoinsPerLevel;

            int currentSpecialCoins = SaveManager.Instance.GetTotalSpecialCoins(totalLevels, coinsPerLevel);
            specialCoinText.text = currentSpecialCoins.ToString();
        }
    }
}