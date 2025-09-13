using UnityEngine;

public class WallCollisionUI : MonoBehaviour
{
    [Header("UI al chocar con muro")]
    public GameObject wallPanel; // Panel que se mostrará al chocar con un muro

    private RespawnManager respawnManager;

    private void Start()
    {
        if (wallPanel != null)
            wallPanel.SetActive(false); // arranca oculto

        respawnManager = GetComponent<RespawnManager>();
    }

    // Este se usa con CharacterController
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Verifica si el objeto pertenece al layer "Wall"
        if (hit.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            TransformationManager tm = GetComponent<TransformationManager>();

            if (tm != null && (tm.IsStrong() || tm.IsFast()))
            {
                // En formas especiales destruye la pared
                Destroy(hit.gameObject);
            }
            else
            {
                // Mostrar panel de muerte y pausar
                if (wallPanel != null)
                    wallPanel.SetActive(true);

                Time.timeScale = 0f; // pausa el juego
            }
        }
    }

    // Método para cerrar el panel y reanudar el juego
    public void HidePanel()
    {
        if (wallPanel != null)
            wallPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // Este método lo conectas al botón "Reintentar" del panel
    public void OnRetryButton()
    {
        if (respawnManager != null)
        {
            respawnManager.Respawn();
        }
        else
        {
            Debug.LogWarning("RespawnManager no encontrado en el jugador.");
        }

        HidePanel();
    }
}
