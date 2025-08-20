using UnityEngine;

public class WallCollisionUI : MonoBehaviour
{
    [Header("UI al chocar con muro")]
    public GameObject wallPanel; // Panel que se mostrará al chocar con un muro

    private void Start()
    {
        if (wallPanel != null)
            wallPanel.SetActive(false); // arranca oculto
    }

    // Este se usa con CharacterController
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Verifica si el objeto pertenece al layer "Wall"
        if (hit.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (wallPanel != null)
            {
                wallPanel.SetActive(true);
                Time.timeScale = 0f; // opcional: pausar el juego
            }
        }
    }

    public void HideWallPanel()
    {
        if (wallPanel != null)
        {
            wallPanel.SetActive(false);
            Time.timeScale = 1f; // reanudar el juego
        }
    }
}
