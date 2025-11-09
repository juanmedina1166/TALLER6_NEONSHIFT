using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    [Header("UI de instrucciones")]
    public PanelAnimator instructionPanel; // Panel de instrucciones en el Canvas


    [Header("Opciones del Trigger")]
    public bool isEndTrigger = false;

    private bool isActive = false;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            ShowInstructions();
            if (isEndTrigger)
            {
                FindObjectOfType<PauseManager>().EndGame();
            }
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
            instructionPanel.ShowPanel(() =>
            {
                // Este código se ejecuta CUANDO la animación termina:
                Time.timeScale = 0f; // pausa el juego
                isActive = true;
            });
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
        if (instructionPanel != null && isActive)
        {
            instructionPanel.HidePanel(() =>
            {
                
                PauseManager pauseManager = Object.FindFirstObjectByType<PauseManager>();
                bool isPauseActive = (pauseManager != null && pauseManager.pausePanel.gameObject.activeSelf);

                if (!isPauseActive)
                {
                    Time.timeScale = 1f;
                }

               
            });
        }

    
    }

}
