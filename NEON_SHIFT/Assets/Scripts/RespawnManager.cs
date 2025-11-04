using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Si ya existe una instancia (por ejemplo, en el jugador)
            // y este script está en otro objeto (como RespawnManager),
            // destruye este componente para evitar duplicados.
            Destroy(this);
        }
    }
    void Start()
    {

        //  Verifica si el checkpoint pertenece a otra escena
        string currentScene = SceneManager.GetActiveScene().name;
        if (GameState.Instance != null && GameState.Instance.checkpointReached)
        {
            // Si el checkpoint fue guardado en otra escena, se limpia
            if (GameState.Instance.lastCheckpointScene != currentScene)
            {
                GameState.Instance.checkpointReached = false;
                GameState.Instance.lastCheckpoint = Vector3.zero;
            }
        }
        //  Solo restaurar monedas si es un respawn dentro de la MISMA escena
        if (PlayerCoins.Instance != null)
        {
            if (GameState.Instance != null && GameState.Instance.lastCheckpointScene == SceneManager.GetActiveScene().name)
            {
                PlayerCoins.Instance.coins = GameState.Instance.coins;
                UICoinManager.Instance.UpdateCoins(GameState.Instance.coins);
                Debug.Log(" Monedas restauradas del checkpoint en la misma escena: " + GameState.Instance.coins);
            }
            else
            {
                // ?? Si es una nueva escena, restauramos las monedas globales
                int globalCoins = SaveManager.Instance != null ? SaveManager.Instance.GetNormalCoins() : 0;
                PlayerCoins.Instance.coins = globalCoins;
                UICoinManager.Instance.UpdateCoins(globalCoins);
                Debug.Log(" Monedas globales restauradas al iniciar nuevo nivel: " + globalCoins);
            }
        }


        // ? Restaurar monedas especiales en la UI
        if (SpecialCoinTracker.Instance != null)
        {
            for (int i = 0; i < GameState.Instance.specialCoins.Length; i++)
            {
                if (GameState.Instance.specialCoins[i])
                {
                    SpecialCoinTracker.Instance.collectedCoins[i] = true;
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

            CameraFlyBob flyBob = FindObjectOfType<CameraFlyBob>();
            if (flyBob != null)
            {
                flyBob.SetFlying(false); // vuelve al estado base
                Debug.Log("?? Cámara restaurada tras respawn.");
            }
        }
        else
        {

            if (GameState.Instance != null)
            {

                var shownTriggers = new System.Collections.Generic.HashSet<string>(GameState.Instance.shownInstructionTriggers);


                GameState.Instance.coins = 0;
                GameState.Instance.checkpointReached = false;
                GameState.Instance.lastCheckpoint = Vector3.zero;

                for (int i = 0; i < GameState.Instance.specialCoins.Length; i++)
                    GameState.Instance.specialCoins[i] = false;

                GameState.Instance.destroyedObjects.Clear();
                GameState.Instance.shownInstructionTriggers = shownTriggers;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (LevelProgressUI.Instance != null)
            LevelProgressUI.Instance.ResetAllCheckpointIcons();

    }
}
