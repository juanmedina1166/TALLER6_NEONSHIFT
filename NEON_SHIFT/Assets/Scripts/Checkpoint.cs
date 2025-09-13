using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameState.Instance.lastCheckpoint = transform.position;
            GameState.Instance.checkpointReached = true;
            Debug.Log("? Checkpoint alcanzado en " + transform.position);
        }
    }
}
