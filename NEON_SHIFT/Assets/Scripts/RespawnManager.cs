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
        Time.timeScale = 1f;

        // Cerrar panel de muerte
        WallCollisionUI wallUI = FindObjectOfType<WallCollisionUI>();
        if (wallUI != null)
            wallUI.HidePanel();

        if (GameState.Instance != null && GameState.Instance.checkpointReached)
        {
            Debug.Log("Respawn en checkpoint");

            // ?? Reactivar solo los power ups recogidos después del checkpoint
            foreach (GameObject obj in GameState.Instance.collectedSinceCheckpoint)
            {
                if (obj != null)
                    obj.SetActive(true);
            }

            // ?? Limpiar la lista para la próxima vida
            GameState.Instance.collectedSinceCheckpoint.Clear();

            // Teletransportar al checkpoint
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            transform.position = GameState.Instance.lastCheckpoint;
            if (cc != null) cc.enabled = true;

            // ?? Resetear poderes del jugador
            TransformationManager tm = GetComponent<TransformationManager>();
            if (tm != null)
            {
                tm.ResetPowers();
            }
        }
        else
        {
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
