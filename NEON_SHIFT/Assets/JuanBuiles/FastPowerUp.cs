using UnityEngine;

public class FastPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Intentar encontrar el componente en el Player, en sus padres o en sus hijos
            TransformationManager tm = other.GetComponent<TransformationManager>();

            if (tm == null)
                tm = other.GetComponentInParent<TransformationManager>();

            if (tm == null)
                tm = other.GetComponentInChildren<TransformationManager>();

            if (tm != null)
            {
                Debug.Log("✅ PowerUp recogido, desbloqueando Fast");
                tm.UnlockFast();
            }
            else
            {
                Debug.LogWarning("⚠️ No encontré TransformationManager en el Player ni en su jerarquía");
            }

            // Destruir el objeto para que no se recoja de nuevo
            Destroy(gameObject);
        }
    }
}
