using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    // --- ¡NUEVAS VARIABLES! ---
    // Define aquí los totales de tu juego para poder limpiar las monedas
    // (Asegúrate de que coincidan con los valores que usas en GetTotalSpecialCoins)
    [Header("Configuración del Juego")]
    public int totalLevelsInGame = 10; // (Ej: 10 niveles)
    public int specialCoinsPerLevel = 3; // (3 monedas por nivel)

    // --- Claves para PlayerPrefs ---
    private const string KEY_NORMAL_COINS = "TotalNormalCoins";
    private const string KEY_HIGHEST_LEVEL = "HighestLevelUnlocked";
    private const string KEY_SPECIAL_COIN_PREFIX = "SpecialCoin_";

    void Awake()
    {
        // ----- Configuración del Singleton -----
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ¡Importante!
        }
        else
        {
            Destroy(gameObject);
        }
        // -------------------------------------
    }


    // =================================================
    // MÉTODOS DE PARTIDA ("Continuar" / "Nueva Partida")
    // =================================================

    public bool HasSaveData()
    {
        return PlayerPrefs.HasKey(KEY_HIGHEST_LEVEL);
    }

    // ----- ¡MÉTODO MODIFICADO! -----
    public void NewGame()
    {
        // ¡¡YA NO USAMOS PlayerPrefs.DeleteAll();!!

        // 1. Borramos el progreso de nivel y monedas normales
        PlayerPrefs.DeleteKey(KEY_NORMAL_COINS);
        PlayerPrefs.DeleteKey(KEY_HIGHEST_LEVEL);

        // 2. Borramos TODAS las monedas especiales recogidas, una por una
        for (int i = 1; i <= totalLevelsInGame; i++) // Para cada nivel
        {
            for (int j = 0; j < specialCoinsPerLevel; j++) // Para cada moneda
            {
                string coinKey = KEY_SPECIAL_COIN_PREFIX + i + "_" + j;
                PlayerPrefs.DeleteKey(coinKey);
            }
        }

        // 3. (Opcional) Borramos el progreso del GameState por si acaso
        if (GameState.Instance != null)
        {
            GameState.Instance.checkpointReached = false;
            GameState.Instance.collectedSinceCheckpoint.Clear();
            GameState.Instance.coins = 0;

            GameState.Instance.shownInstructionTriggers.Clear();
        }

        // 4. Ponemos los valores por defecto (igual que antes)
        PlayerPrefs.SetInt(KEY_HIGHEST_LEVEL, 1); // Desbloquea el Nivel 1
        PlayerPrefs.SetInt(KEY_NORMAL_COINS, 0);
        PlayerPrefs.Save(); // Aplica los cambios

        // 5. ¡¡AÑADE ESTA LÍNEA!!
        SceneManager.LoadScene("Level_1"); // (O el nombre de tu escena Tutorial)
    }
    public void ResetProgress()
    {
        // 1. Borramos el progreso de nivel y monedas normales
        PlayerPrefs.DeleteKey(KEY_NORMAL_COINS);
        PlayerPrefs.DeleteKey(KEY_HIGHEST_LEVEL);

        // 2. Borramos TODAS las monedas especiales recogidas
        for (int i = 1; i <= totalLevelsInGame; i++)
        {
            for (int j = 0; j < specialCoinsPerLevel; j++)
            {
                string coinKey = KEY_SPECIAL_COIN_PREFIX + i + "_" + j;
                PlayerPrefs.DeleteKey(coinKey);
            }
        }

        // 3. (Opcional) Borramos el progreso del GameState
        if (GameState.Instance != null)
        {
            GameState.Instance.checkpointReached = false;
            GameState.Instance.collectedSinceCheckpoint.Clear();
            GameState.Instance.coins = 0;

            GameState.Instance.shownInstructionTriggers.Clear();
        }

        // 4. Ponemos los valores por defecto
        PlayerPrefs.SetInt(KEY_HIGHEST_LEVEL, 1); // Desbloquea el Nivel 1
        PlayerPrefs.SetInt(KEY_NORMAL_COINS, 0);
        PlayerPrefs.Save();

        // 5. ¡¡NO CARGAMOS ESCENA!!
    }

    public void ContinueGame()
    {
        // Carga la escena del menú de selección de niveles
        // o carga el último nivel desbloqueado.
        SceneManager.LoadScene("LevelSelectMenu"); // (Recomiendo un menú de niveles)
    }

    // ============================
    // MÉTODOS DE MONEDAS NORMALES
    // ============================

    public void AddNormalCoins(int amount)
    {
        int currentCoins = GetNormalCoins();
        int newTotal = currentCoins + amount;

        PlayerPrefs.SetInt(KEY_NORMAL_COINS, newTotal);
        PlayerPrefs.Save();
        Debug.Log("¡MONEDAS GUARDADAS! Nuevo total en PlayerPrefs: " + newTotal);

        MostrarMonedasGlobales display = FindObjectOfType<MostrarMonedasGlobales>();
        if (display != null)
        {
            display.UpdateCoinDisplay();
        }
    }

    public void SetTotalNormalCoins(int total)
    {
        PlayerPrefs.SetInt(KEY_NORMAL_COINS, total);
        PlayerPrefs.Save();
        Debug.Log("? Total de monedas actualizado a " + total);
    }


    public int GetNormalCoins()
    {
        // Devuelve las monedas guardadas. Si no hay, devuelve 0.
        return PlayerPrefs.GetInt(KEY_NORMAL_COINS, 0);
    }

    // =============================
    // MÉTODOS DE MONEDAS ESPECIALES
    // =============================

    public bool CollectSpecialCoin(int levelIndex, int coinIndex)
    {
        // Genera una clave única para esta moneda, ej: "SpecialCoin_1_0"
        string coinKey = KEY_SPECIAL_COIN_PREFIX + levelIndex + "_" + coinIndex;

        // Comprueba si ya la teníamos
        if (PlayerPrefs.GetInt(coinKey, 0) == 1)
        {
            return false; // Ya la teníamos, no es nueva
        }

        // Si no la teníamos, la marcamos como recogida
        PlayerPrefs.SetInt(coinKey, 1);
        PlayerPrefs.Save();
        return true; // ¡Es una moneda nueva!
    }

    public bool IsSpecialCoinCollected(int levelIndex, int coinIndex)
    {
        string coinKey = KEY_SPECIAL_COIN_PREFIX + levelIndex + "_" + coinIndex;
        return PlayerPrefs.GetInt(coinKey, 0) == 1;
    }

    public int GetTotalSpecialCoins(int totalLevels, int coinsPerLevel)
    {
        // Este método cuenta cuántas monedas tenemos en total
        int totalCount = 0;
        for (int i = 1; i <= totalLevels; i++) // Asumiendo que Nivel 1 es índice 1
        {
            for (int j = 0; j < coinsPerLevel; j++) // Asumiendo que moneda 0, 1, 2
            {
                if (IsSpecialCoinCollected(i, j))
                {
                    totalCount++;
                }
            }
        }
        return totalCount;
    }

    // =======================
    // MÉTODOS DE NIVEL
    // =======================

    public void UnlockLevel(int levelIndex)
    {
        if (levelIndex > GetHighestUnlockedLevel())
        {
            PlayerPrefs.SetInt(KEY_HIGHEST_LEVEL, levelIndex);
            PlayerPrefs.Save();
        }
    }

    public int GetHighestUnlockedLevel()
    {
        // Devuelve el nivel más alto. Si no hay, devuelve 1 (Nivel 1).
        return PlayerPrefs.GetInt(KEY_HIGHEST_LEVEL, 1);
    }
}