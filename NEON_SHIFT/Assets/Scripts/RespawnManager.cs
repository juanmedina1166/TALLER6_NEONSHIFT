using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private Vector3 respawnPoint;
    private CharacterController controller;

    [Header("UI")]
    public GameObject fallPanel; 

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // primer checkpoint en la escena
        GameObject checkpoint = GameObject.FindGameObjectWithTag("Checkpoint");
        if (checkpoint != null)
        {
            respawnPoint = checkpoint.transform.position;
        }
        else
        {
            respawnPoint = transform.position; // posición inicial si no hay checkpoint
        }

        if (fallPanel != null)
            fallPanel.SetActive(false); // que arranque oculto
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Abismo"))
        {
            ShowFallPanel();
        }
        else if (other.CompareTag("Checkpoint"))
        {
            respawnPoint = other.transform.position; // actualizar checkpoint dinámico
        }
    }

    private void ShowFallPanel()
    {
        if (fallPanel != null)
        {
            fallPanel.SetActive(true);
            Time.timeScale = 0f; // pausa el juego
        }
    }

    public void RetryFromCheckpoint()
    {
        if (fallPanel != null)
            fallPanel.SetActive(false);

        // Reposicionar al jugador en el último checkpoint
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = respawnPoint;
            controller.enabled = true;
        }
        else
        {
            transform.position = respawnPoint;
        }

        Time.timeScale = 1f; // reanudar el juego
    }
}
