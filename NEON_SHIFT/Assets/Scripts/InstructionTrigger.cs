using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    [Header("UI de instrucciones")]
    public GameObject instructionPanel; // arrastra aquí el panel del Canvas en el Inspector

    private bool isActive = false;

    private void Start()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(false); // asegurarse de que arranque oculto
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

    public void HideInstructions()
    {
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
            Time.timeScale = 1f; // reanuda el juego
        }
    }
}
