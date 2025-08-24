using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Fast,
        Strong,
        Fly
    }

    [Header("Configuración del Power Up")]
    public PowerUpType type; // Seleccionas en el Inspector el tipo de power up

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Buscar el componente TransformationManager
            TransformationManager tm = other.GetComponent<TransformationManager>();

            if (tm == null)
                tm = other.GetComponentInParent<TransformationManager>();

            if (tm == null)
                tm = other.GetComponentInChildren<TransformationManager>();

            if (tm != null)
            {
                Debug.Log($"✅ PowerUp recogido: {type}");

                switch (type)
                {
                    case PowerUpType.Fast:
                        tm.UnlockFast();
                        break;

                    case PowerUpType.Strong:
                        tm.UnlockStrong();
                        break;

                    case PowerUpType.Fly:
                        tm.UnlockFly();
                        break;
                }
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
