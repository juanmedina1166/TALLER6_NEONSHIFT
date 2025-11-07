using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ¡¡Añade esta línea para poder usar "Button"!!
using TMPro; // ¡¡Añade esta línea para el texto de requisitos!!

public class VictoryTrigger : MonoBehaviour
{
    public int currentLevelIndex;

    [Header("UI de Victoria")]
    public GameObject victoryPanel; // Panel de victoria en el Canvas
    public SpecialCoinManager coinManager;

    // --- INICIO DE CAMBIOS ---

    [Header("Botón Siguiente Nivel")]
    public Button nextLevelButton; // Arrastra tu botón "Siguiente Nivel" aquí
    public TextMeshProUGUI requirementsText; // Un texto para mostrar si el botón está bloqueado

    [Header("Requisitos del SIGUIENTE Nivel")]
    public int specialCoinsRequired; // Requisitos de monedas especiales para el PRÓXIMO nivel
    public int normalCoinsRequired; // Requisitos de monedas normales para el PRÓXIMO nivel

    // --- FIN DE CAMBIOS ---

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
            isActive = true;

            // --- ¡LÓGICA DE GUARDADO AL GANAR! ---
            if (SaveManager.Instance != null)
            {
                // ¡¡CAMBIO!! Guardamos solo las monedas de la SESIÓN (el bolsillo)
                if (PlayerCoins.Instance != null)
                {
                    SaveManager.Instance.AddNormalCoins(PlayerCoins.Instance.sessionCoins);
                }

                // 2. Desbloquea el SIGUIENTE nivel
                SaveManager.Instance.UnlockLevel(currentLevelIndex + 1);
            }

            // ¡¡AÑADE ESTO!! Resetea el puntaje de la sesión
            if (PlayerScore.Instance != null && LeaderboardManager.Instance != null)
            {
                string playerName = LeaderboardManager.Instance.GetCurrentPlayerName();
                int sessionScore = PlayerScore.Instance.currentScore;

                // ¡Enviamos el puntaje al manager!
                LeaderboardManager.Instance.SubmitScore(currentLevelIndex, playerName, sessionScore);
            }
            // Ahora llamamos a la función que mostrará el panel
            // Y también comprobará los requisitos.
            ShowVictoryPanel();
        }
    }

    private void ShowVictoryPanel()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (coinManager != null)
            coinManager.UpdateUI(currentLevelIndex); ; // actualizar las monedas recogidas

        // --- INICIO DE LÓGICA DE REQUISITOS ---

        // Asegurarnos de que el SaveManager existe
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager no encontrado. No se puede comprobar requisitos.");
            Time.timeScale = 0f; // Pausa el juego
            return;
        }

        // Obtenemos los NUEVOS totales de monedas (los que acabamos de guardar)
        int totalSpecial = SaveManager.Instance.GetTotalSpecialCoins(
            SaveManager.Instance.totalLevelsInGame,
            SaveManager.Instance.specialCoinsPerLevel);

        int totalNormal = SaveManager.Instance.GetNormalCoins();

        // Comprobamos si el jugador CUMPLE los requisitos para el siguiente nivel
        bool hasSpecial = (totalSpecial >= specialCoinsRequired);
        bool hasNormal = (totalNormal >= normalCoinsRequired);

        if (hasSpecial && hasNormal)
        {
            // ¡Desbloqueado!
            if (nextLevelButton != null)
                nextLevelButton.interactable = true;

            if (requirementsText != null)
                requirementsText.gameObject.SetActive(false);
        }
        else
        {
            // Bloqueado
            if (nextLevelButton != null)
                nextLevelButton.interactable = false; // Desactiva el botón

            // Muestra el texto de requisitos
            if (requirementsText != null)
            {
                requirementsText.gameObject.SetActive(true);
                requirementsText.text = $"REQUISITOS NIVEL {currentLevelIndex + 1}:\n{specialCoinsRequired} Monedas Esp.\n{normalCoinsRequired} Monedas";
            }
        }

        // --- FIN DE LÓGICA DE REQUISITOS ---

        Time.timeScale = 0f; // Pausa el juego
        isActive = true;
    }

    // (Esta función no cambia)
    public void HideVictoryPanel()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
            Time.timeScale = 1f; // Reanuda el juego
            isActive = false;
        }
    }

    // (Esta función no cambia, el botón la llamará si está activo)
    public void LoadNextLevel()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveAllData();
        }
        Time.timeScale = 1f;

        // Resetea el estado para el nuevo nivel
        if (PlayerCoins.Instance != null)
            PlayerCoins.Instance.ResetSession();

        if (PlayerScore.Instance != null)
            PlayerScore.Instance.ResetSessionScore();

        if (GameState.Instance != null)
        {
            GameState.Instance.checkpointReached = false;
            GameState.Instance.lastCheckpoint = Vector3.zero;
            GameState.Instance.collectedSinceCheckpoint.Clear();
            GameState.Instance.destroyedObjects.Clear();
            GameState.Instance.coins = 0;
        }

        SceneManager.LoadScene(currentLevelIndex + 1);
    }
}