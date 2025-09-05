using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    public GameObject pausePanel; // arrastra el panel del Canvas

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false); // empieza oculto
    }

    public void PauseGame()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
