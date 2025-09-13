using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    public GameObject pausePanel; // arrastra el panel del Canvas
    public GameObject pauseButton;
    private bool gameEnded = false;

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false); // empieza oculto
    }

    public void PauseGame()
    {
        if (gameEnded) return; // si ya terminó, no permitir pausar

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("Juego pausado");

        // Pausar todos los animators
        Animator[] animators = Object.FindObjectsByType<Animator>(FindObjectsSortMode.None);
        foreach (var anim in animators)
            anim.speed = 0f;
    }

    public void ResumeGame()
    {
        if (gameEnded) return; // si ya terminó, no permitir reanudar

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;

        // Reanudar todos los animators
        Animator[] animators = Object.FindObjectsByType<Animator>(FindObjectsSortMode.None);
        foreach (var anim in animators)
            anim.speed = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        // ?? Resetear datos del GameState antes de recargar escena
        GameState.Instance.coins = 0;
        GameState.Instance.checkpointReached = false;
        GameState.Instance.lastCheckpoint = Vector3.zero;

        for (int i = 0; i < GameState.Instance.specialCoins.Length; i++)
        {
            GameState.Instance.specialCoins[i] = false;
        }
        GameState.Instance.destroyedObjects.Clear();

        // Cargar desde el inicio
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void EndGame()
    {
        gameEnded = true;

        if (pauseButton != null)
            pauseButton.SetActive(false);

        // opcional: desactivar panel de pausa si estaba abierto
        if (pausePanel != null)
            pausePanel.SetActive(false);
        // asegurarse que el tiempo quede en 0 si así lo manejas
        Time.timeScale = 0f;
    }
}
