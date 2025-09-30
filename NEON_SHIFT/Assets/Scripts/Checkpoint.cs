using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [Header("Audio")]
    public AudioClip checkpointSound; // sonido al activar checkpoint
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameState.Instance.lastCheckpoint = transform.position;
            GameState.Instance.checkpointReached = true;

            //  Guardar cantidad de monedas actual como la del checkpoint
            if (PlayerCoins.Instance != null)
                PlayerCoins.Instance.SaveCheckpointCoins();

            Debug.Log(" Checkpoint alcanzado en " + transform.position +
                      " | Monedas guardadas: " + GameState.Instance.coins);

            // ?? Reproducir sonido
            if (checkpointSound != null)
            {
                AudioSource playerAudio = other.GetComponentInParent<AudioSource>();
                if (playerAudio != null)
                    playerAudio.PlayOneShot(checkpointSound, volume);
            }
               
        }
    }
}
