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

    [Header("Audio")]
    public AudioClip pickupSound; // Sonido al recoger
    public float volume = 1f;

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

            // 🎵 Reproducir sonido en la posición del power up
            if (pickupSound != null)
            {
                GameObject tempAudio = new GameObject("TempPowerUpAudio");
                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
                tempSource.clip = pickupSound;
                tempSource.volume = volume;
                tempSource.spatialBlend = 0f; // 2D (para que siempre se escuche)
                tempSource.Play();
                Destroy(tempAudio, pickupSound.length);
            }

            // Destruir el objeto para que no se recoja de nuevo
            gameObject.SetActive(false);

            // Guardar referencia directa del power up desactivado
            if (GameState.Instance != null && !GameState.Instance.collectedSinceCheckpoint.Contains(gameObject))
            {
                GameState.Instance.collectedSinceCheckpoint.Add(gameObject);
            }
        }
    }
}
