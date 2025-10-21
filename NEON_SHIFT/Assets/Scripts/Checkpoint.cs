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

            // ?? Reproducir sonido
            if (checkpointSound != null)
            {
                AudioSource playerAudio = other.GetComponentInParent<AudioSource>();
                if (playerAudio != null)
                    playerAudio.PlayOneShot(checkpointSound, volume);
            }
            if (CheckpointUI.Instance != null)
                CheckpointUI.Instance.ShowCheckpointIcon(2f);

            if (LevelProgressUI.Instance != null)
            {
                for (int i = 0; i < LevelProgressUI.Instance.checkpoints.Length; i++)
                {
                    if (LevelProgressUI.Instance.checkpoints[i] == transform)
                    {
                        LevelProgressUI.Instance.UpdateCheckpointIcon(i);
                        break;
                    }
                }
            }
        }
    }
}
