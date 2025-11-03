using UnityEngine;
using TMPro; // ¡Necesitas esta línea para usar TextMeshPro!

public class MostrarMonedasGlobales : MonoBehaviour
{
    // Arrastra el componente TextMeshPro que creaste en el inspector
    public TextMeshProUGUI coinText;

    void OnEnable()
    {
        // Esto asegura que el texto se actualice CADA VEZ que el jugador ve este panel.
        UpdateCoinDisplay();
    }

    // Método que puedes llamar cada vez que las monedas cambien
    public void UpdateCoinDisplay()
    {
        // 1. Obtener la cantidad de monedas usando tu SaveManager
        if (SaveManager.Instance != null && coinText != null)
        {
            int currentCoins = SaveManager.Instance.GetNormalCoins();

            // 2. Formatear y asignar el texto.
            // Puedes usar un emoji o un icono de moneda al principio.
            coinText.text = currentCoins.ToString();

            // O con formato:
            // coinText.text = "💰 " + currentCoins.ToString("N0"); // "N0" para separadores de miles
        }
    }
}