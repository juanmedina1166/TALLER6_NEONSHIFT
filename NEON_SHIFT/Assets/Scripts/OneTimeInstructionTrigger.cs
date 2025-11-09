using UnityEngine;

public class OneTimeInstructionTrigger : MonoBehaviour
{
    [Header("UI de instrucciones")]
    public PanelAnimator instructionPanel;

    [Header("ID único del trigger")]
    public string triggerID; // Ejemplo: "IntroPanel_01"

    private bool hasShown = false;
    private bool isActive = false;

    void Start()
    {
       

        // Si ya se mostró antes, no volver a mostrar
        if (GameState.Instance != null && GameState.Instance.IsTriggerShown(triggerID))
            hasShown = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasShown) return;

        ShowPanel();
    }

    private void ShowPanel()
    {
        if (instructionPanel != null)
        {
            instructionPanel.ShowPanel(() =>
            {
                // Este código se ejecuta CUANDO la animación termina:
                Time.timeScale = 0f;
                isActive = true;
            });
        }

        hasShown = true;

        if (GameState.Instance != null)
            GameState.Instance.MarkTriggerShown(triggerID);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isActive = false;
    }

    public void HidePanel()
    {
        if (instructionPanel != null && isActive)
        {
            instructionPanel.HidePanel(() =>
            {
                // Este código se ejecuta CUANDO la animación termina:

                // Tu lógica inteligente para no reanudar si está en pausa:
                PauseManager pauseManager = Object.FindFirstObjectByType<PauseManager>();

                // ¡Esta línea es correcta! Comprueba si el PanelAnimator de pausa está activo
                bool isPauseActive = (pauseManager != null && pauseManager.pausePanel.gameObject.activeSelf);

                if (!isPauseActive)
                    Time.timeScale = 1f;

                isActive = false;
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
            HidePanel();
    }
}
