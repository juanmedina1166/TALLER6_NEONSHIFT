using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    void Start()
    {
        // ? Restaurar monedas normales desde GameState
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

        // ? Respawn en el checkpoint
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

        // ? Cerrar panel de muerte
        WallCollisionUI wallUI = FindObjectOfType<WallCollisionUI>();
        if (wallUI != null)
            wallUI.HidePanel();

        // ? Resetear poderes ANTES de reposicionar
        TransformationManager tm = GetComponent<TransformationManager>();
        if (tm != null)
            tm.ResetPowers();

        if (GameState.Instance != null && GameState.Instance.checkpointReached)
        {

            // ? Restaurar monedas guardadas en el checkpoint
            if (PlayerCoins.Instance != null)
            {
                PlayerCoins.Instance.coins = GameState.Instance.coins;
                UICoinManager.Instance.UpdateCoins(GameState.Instance.coins);
                Debug.Log("?? Monedas restauradas al valor del checkpoint: " + GameState.Instance.coins);
            }

            // ? Reactivar objetos recogidos desde el checkpoint
            foreach (GameObject obj in GameState.Instance.collectedSinceCheckpoint)
            {
                if (obj != null)
                    obj.SetActive(true);
            }

            // ? Limpiar la lista después de reactivarlos
            GameState.Instance.collectedSinceCheckpoint.Clear();

            // Teletransportar al checkpoint
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            transform.position = GameState.Instance.lastCheckpoint;
            if (cc != null) cc.enabled = true;

            // --- INICIO DE LA MODIFICACIÓN ---
            // AVISAMOS AL TERRAIN MANAGER QUE ACTUALICE EL TERRENO AHORA MISMO
            if (TerrainManager.Instance != null)
                TerrainManager.Instance.UpdateTerrainsOnRespawn(GameState.Instance.lastCheckpoint);
            // --- FIN DE LA MODIFICACIÓN ---

            // Restaurar movimiento del jugador
            PlayerController pc = GetComponent<PlayerController>();
            if (pc != null) pc.RestoreMovement();
        }
        else
        {

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
