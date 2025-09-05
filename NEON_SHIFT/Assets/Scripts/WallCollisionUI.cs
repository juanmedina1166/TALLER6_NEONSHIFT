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
        if (hit.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            TransformationManager tm = GetComponent<TransformationManager>();

            bool isStrongOrFast = tm != null && (tm.IsStrong() || tm.IsFast());
            bool hasWallTag = hit.gameObject.CompareTag("Wall");

            if (hasWallTag && isStrongOrFast)
            {
                // Solo destruye si cumple ambas condiciones
                Destroy(hit.gameObject);
            }
            else
            {
                // ? Siempre mostrar panel si no se destruye el objeto
                if (wallPanel != null)
                {
                    wallPanel.SetActive(true);
                    Time.timeScale = 0f;
                }
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
