using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    void Start()
    {
        // ? Restaurar monedas normales
        if (PlayerCoins.Instance != null)
        {
            PlayerCoins.Instance.coins = GameState.Instance.coins;
            UICoinManager.Instance.UpdateCoins(GameState.Instance.coins);
        }

        // ? Restaurar monedas especiales en la UI
        if (SpecialCoinManager.Instance != null)
        {
            for (int i = 0; i < GameState.Instance.specialCoins.Length; i++)
            {
                if (GameState.Instance.specialCoins[i])
                {
                    SpecialCoinManager.Instance.CollectCoin(i);
                }
            }
        }

        // ? Respawn en el checkpoint (si existe)
        if (GameState.Instance.checkpointReached)
        {
            transform.position = GameState.Instance.lastCheckpoint;
        }

        // ? Restaurar objetos destruidos
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (GameState.Instance.destroyedObjects.Contains(obj.name))
            {
                Destroy(obj);
            }
        }
    }

    public void Respawn()
    {
        Time.timeScale = 1f; // por si estaba pausado

        // ?? Cerrar panel de muerte si estaba abierto
        WallCollisionUI wallUI = FindObjectOfType<WallCollisionUI>();
        if (wallUI != null)
        {
            wallUI.HidePanel(); // <-- nuevo método de Opción A
        }

        if (GameState.Instance != null && GameState.Instance.checkpointReached)
        {
            // ? Teletransportar al checkpoint
            Debug.Log("Respawn en checkpoint");
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            transform.position = GameState.Instance.lastCheckpoint;
            if (cc != null) cc.enabled = true;
        }
        else
        {
            // ? Reiniciar escena si NO hay checkpoint
            Debug.Log("No hay checkpoint ? reiniciando nivel");
            if (GameState.Instance != null)
            {
                GameState.Instance.coins = 0;
                GameState.Instance.checkpointReached = false;
                GameState.Instance.lastCheckpoint = Vector3.zero;

                for (int i = 0; i < GameState.Instance.specialCoins.Length; i++)
                    GameState.Instance.specialCoins[i] = false;

                GameState.Instance.destroyedObjects.Clear();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
