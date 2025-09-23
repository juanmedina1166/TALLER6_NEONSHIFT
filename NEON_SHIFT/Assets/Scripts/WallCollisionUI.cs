using UnityEngine;

public class WallCollisionUI : MonoBehaviour
{
    [Header("UI al chocar con muro")]
    public GameObject wallPanel;

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
        // Asegurarse que chequeamos el layer correcto
        if (hit.gameObject.layer != LayerMask.NameToLayer("Wall"))
            return;

        // Si la forma es Strong/Fast y quieres destruir la pared:
        if (tm != null && (tm.IsStrong() || tm.IsFast()))
        {
            // Si usas el sistema de respawn para restaurar muros, registra en lugar de destruir:
            // RespawnManager.RegisterDestroyedObject(hit.gameObject);
            hit.gameObject.SetActive(false);
            if (GameState.Instance != null && !GameState.Instance.collectedSinceCheckpoint.Contains(hit.gameObject))
                GameState.Instance.collectedSinceCheckpoint.Add(hit.gameObject);

            return;
        }

        // Si está transformado (por ejemplo volando), NO reproducir animación de muerte.
        if (tm != null && tm.IsTransformed())
        {
            // Solo ocultar botones para que no se spamee la transformación mientras está el panel
            tm.DisablePowerButtons();

            ShowPanelImmediate();
            return;
        }

        // Si NO está transformado => reproducir animación de muerte y luego mostrar panel
        // (si quieres que los botones también se oculten antes de morir, lo haces en ResetPowers)
        tm?.ResetPowers();

        if (player != null)
        {
            player.Die(() =>
            {
                ShowPanelImmediate();
            });
        }
        else
        {
            ShowPanelImmediate();
        }
    }

    private void ShowPanelImmediate()
    {
        if (wallPanel != null)
            wallPanel.SetActive(true);

        Time.timeScale = 0f;
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
