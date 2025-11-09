using UnityEngine.SceneManagement;
using UnityEngine;


public class PauseManager : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    public PanelAnimator pausePanel; // arrastra el panel del Canvas
    public GameObject pauseButton;
    private bool gameEnded = false;

    private void Start()
    {
        
    }

    public void PauseGame()
    {
        if (gameEnded) return; // si ya terminó, no permitir pausar

        if (pausePanel != null)
        {
            // Llamamos a ShowPanel y le pasamos todo lo que
            // debe pasar DESPUÉS de que la animación termine.
            pausePanel.ShowPanel(() =>
            {
                // -- INICIO DEL CÓDIGO DEL CALLBACK --
                Time.timeScale = 0f;

                // Pausar todos los animators
                Animator[] animators = Object.FindObjectsByType<Animator>(FindObjectsSortMode.None);
                foreach (var anim in animators)
                    anim.speed = 0f;
                // -- FIN DEL CÓDIGO DEL CALLBACK --
            });
        }
    }

    public void ResumeGame()
    {
        if (gameEnded) return; // si ya terminó, no permitir reanudar

        if (pausePanel != null)
        {
            // Hacemos lo mismo para reanudar
            pausePanel.HidePanel(() =>
            {
                // -- INICIO DEL CÓDIGO DEL CALLBACK --
                Time.timeScale = 1f;

                // Reanudar todos los animators
                Animator[] animators = Object.FindObjectsByType<Animator>(FindObjectsSortMode.None);
                foreach (var anim in animators)
                    anim.speed = 1f;
                // -- FIN DEL CÓDIGO DEL CALLBACK --
            });
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (GameState.Instance != null)
        {
            GameState.Instance.coins = 0;
            GameState.Instance.checkpointReached = false;
            GameState.Instance.lastCheckpoint = Vector3.zero;
            GameState.Instance.destroyedObjects.Clear();
            GameState.Instance.collectedSinceCheckpoint.Clear(); // Asegúrate de limpiar esto también
        }

        if (PlayerCoins.Instance != null)
        {
            PlayerCoins.Instance.ResetSession();
        }

        // Cargar desde el inicio
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (PlayerCoins.Instance != null)
        {
            PlayerCoins.Instance.ResetSession();
        }
        SceneManager.LoadScene("MainMenu");
    }
    public void EndGame()
    {
        gameEnded = true;

        if (pauseButton != null)
            pauseButton.SetActive(false);

        // opcional: desactivar panel de pausa si estaba abierto
        if (pausePanel != null)
            pausePanel.HidePanel();
        // asegurarse que el tiempo quede en 0 si así lo manejas
        Time.timeScale = 0f;
    }
}
