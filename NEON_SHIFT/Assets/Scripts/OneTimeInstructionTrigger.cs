using UnityEngine;

public class OneTimeInstructionTrigger : MonoBehaviour
{
    [Header("UI de instrucciones")]
    public GameObject instructionPanel;

    [Header("ID único del trigger")]
    public string triggerID; // Ejemplo: "IntroPanel_01"

    private bool hasShown = false;
    private bool isActive = false;

    void Start()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(false);

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
            instructionPanel.SetActive(true);
            Time.timeScale = 0f;
            isActive = true;
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
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);

            // Solo reanuda si el panel de pausa NO está activo
            PauseManager pauseManager = Object.FindFirstObjectByType<PauseManager>();
            bool isPauseActive = (pauseManager != null && pauseManager.pausePanel.activeSelf);

            if (!isPauseActive)
                Time.timeScale = 1f;
        }

        isActive = false;
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
