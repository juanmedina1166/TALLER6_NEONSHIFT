using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [Header("UI de Victoria")]
    public GameObject victoryPanel; // Panel de victoria en el Canvas

    private bool isActive = false;

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false); // arranca oculto
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            ShowVictoryPanel();
        }
    }

    private void ShowVictoryPanel()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
            isActive = true;
        }
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


}
