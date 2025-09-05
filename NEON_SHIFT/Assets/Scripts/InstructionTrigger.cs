using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    [Header("UI de instrucciones")]
    public GameObject instructionPanel; // Panel de instrucciones en el Canvas

    private bool isActive = false;

    private void Start()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(false); // arranca oculto
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            ShowInstructions();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false; // Ahora solo se puede volver a mostrar si el jugador salió
        }
    }

    public void ShowInstructions()
    {
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            Time.timeScale = 0f; // pausa el juego
            isActive = true;
        }
    }
    private void OnEnable()
    {
        SwipeManager.OnTap += HandleTap;
    }

    private void OnDisable()
    {
        SwipeManager.OnTap -= HandleTap;
    }

    private void HandleTap()
    {
        if (isActive)
            HideInstructions();
    }

    public void HideInstructions()
    {
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);

            // Solo reanuda si no está el panel de pausa activo
            PauseManager pauseManager = Object.FindFirstObjectByType<PauseManager>();
            bool isPauseActive = (pauseManager != null && pauseManager.pausePanel.activeSelf);

            if (!isPauseActive)
            {
                Time.timeScale = 1f;
            }
        }

    
    }

}
