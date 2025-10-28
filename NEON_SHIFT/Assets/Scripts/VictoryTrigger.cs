using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    public int currentLevelIndex;
    [Header("UI de Victoria")]
    public GameObject victoryPanel; // Panel de victoria en el Canvas
    public SpecialCoinManager coinManager;

    private bool isActive = false;

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false); // arranca oculto
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null && !isActive)
        {
            isActive = true; // Muévelo aquí para evitar doble trigger

            // --- ¡LÓGICA DE GUARDADO AL GANAR! ---
            if (SaveManager.Instance != null)
            {
                // 1. (¡RESTAURADO!) Guarda las monedas de esta sesión
                if (PlayerCoins.Instance != null)
                {
                    SaveManager.Instance.AddNormalCoins(PlayerCoins.Instance.coins);
                }

                // 2. Desbloquea el SIGUIENTE nivel
                SaveManager.Instance.UnlockLevel(currentLevelIndex + 1);
            }
            // -------------------------------------

            ShowVictoryPanel();
        }
    }

    private void ShowVictoryPanel()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (coinManager != null)
            coinManager.UpdateUI(); // actualizar las monedas recogidas

        Time.timeScale = 0f; // Pausa el juego
            isActive = true;
        
    }

    public void HideVictoryPanel()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
            Time.timeScale = 1f; // Reanuda el juego cuando desaparece el panel
            isActive = false;
        }
    }
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Asegúrate de reanudar el tiempo
        // Carga la siguiente escena basada en el índice actual + 1
        SceneManager.LoadScene(currentLevelIndex + 1);
    }
}
