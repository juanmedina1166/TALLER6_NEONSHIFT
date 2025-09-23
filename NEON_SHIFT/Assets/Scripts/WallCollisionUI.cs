using UnityEngine;

public class WallCollisionUI : MonoBehaviour
{
    [Header("UI al chocar con muro")]
    public GameObject wallPanel; // Panel que se mostrará al chocar con un muro

    private RespawnManager respawnManager;
    private PlayerController player;
    private TransformationManager tm;

    private void Start()
    {
        if (wallPanel != null)
            wallPanel.SetActive(false);

        respawnManager = GetComponent<RespawnManager>();
        player = GetComponent<PlayerController>();
        tm = GetComponent<TransformationManager>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (tm != null && (tm.IsStrong() || tm.IsFast()))
            {
                // Destruye la pared si es fuerte/rápido
                hit.gameObject.SetActive(false);
                if (GameState.Instance != null &&
                    !GameState.Instance.collectedSinceCheckpoint.Contains(hit.gameObject))
                {
                    GameState.Instance.collectedSinceCheckpoint.Add(hit.gameObject);
                }
            }
            else
            {
                // ?? Detenemos transformaciones activas
                tm?.ResetPowers();

                // ?? Ejecutamos animación de muerte y mostramos panel al terminar
                if (player != null)
                {
                    player.Die(() =>
                    {
                        if (wallPanel != null)
                            wallPanel.SetActive(true);

                        Time.timeScale = 0f; // pausa el juego SOLO después de la animación
                    });
                }
                else
                {
                    // Si no hay PlayerController, mostrar panel inmediato
                    if (wallPanel != null)
                        wallPanel.SetActive(true);

                    Time.timeScale = 0f;
                }
            }
        }
    }

    public void HidePanel()
    {
        if (wallPanel != null)
            wallPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OnRetryButton()
    {
        if (respawnManager != null)
            respawnManager.Respawn();
        else
            Debug.LogWarning("RespawnManager no encontrado en el jugador.");

        HidePanel();
    }
}
