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
            Time.timeScale = 1f; // reanuda el juego
        }
    }
}
