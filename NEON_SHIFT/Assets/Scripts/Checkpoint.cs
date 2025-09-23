using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameState.Instance.lastCheckpoint = transform.position;
            GameState.Instance.checkpointReached = true;

            // ? Guardar cantidad de monedas actual como la del checkpoint
            if (PlayerCoins.Instance != null)
                PlayerCoins.Instance.SaveCheckpointCoins();

            Debug.Log(" Checkpoint alcanzado en " + transform.position +
                      " | Monedas guardadas: " + GameState.Instance.coins);
        }
    }
}
