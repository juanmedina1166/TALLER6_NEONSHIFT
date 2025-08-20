using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    [Header("UI de instrucciones")]
    public GameObject instructionPanel; // Panel de instrucciones en el Canvas
    [Header("Botón/UI a mostrar")]
    public GameObject buttonToShow;     // Cualquier botón u objeto oculto en el Canvas

    private bool isActive = false;

  
    private void Start()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(false); // arranca oculto

        if (buttonToShow != null)
            buttonToShow.SetActive(false); // arranca oculto
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

        if (buttonToShow != null)
        {
            buttonToShow.SetActive(true); // mostrar el botón u objeto UI
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
