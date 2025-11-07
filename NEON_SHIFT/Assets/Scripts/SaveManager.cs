using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("Configuración del Juego")]
    public int totalLevelsInGame = 10;
    public int specialCoinsPerLevel = 3;

    private const string KEY_NORMAL_COINS = "TotalNormalCoins";
    private const string KEY_HIGHEST_LEVEL = "HighestLevelUnlocked";
    private const string KEY_SPECIAL_COIN_PREFIX = "SpecialCoin_";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnsureInitialized() { }

    // =================================================
    // MÉTODOS DE PARTIDA ("Continuar" / "Nueva Partida")
    // =================================================

    // --- ¡MÉTODO MODIFICADO! ---
    public bool HasSaveData()
    {
        // Una partida "existe" si el nivel más alto desbloqueado es al menos 1.
        // Si devuelve 0 (el nuevo default), no hay partida guardada.
        return GetHighestUnlockedLevel() > 0;
    }

    public void NewGame()
    {
        // (Tu lógica de NewGame está bien, pero usa ResetProgress que es lo importante)
        // ...
        // Por consistencia, la lógica de borrado debe estar en ResetProgress
        ResetProgress();

        // Cargar el primer nivel después de crear la partida
        SceneManager.LoadScene("Level_1"); // (O el nombre de tu escena Tutorial)
    }

    public void ResetProgress()
    {
        Debug.LogWarning("¡¡¡FUNCIÓN RESETPROGRESS EJECUTADA!!!");

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
        // (Tu lógica aquí está bien)

        // 4. Ponemos los valores por defecto
        PlayerPrefs.SetInt(KEY_HIGHEST_LEVEL, 1); // Desbloquea el Nivel 1
        PlayerPrefs.SetInt(KEY_NORMAL_COINS, 0);

        // 5. Guardar cambios
        PlayerPrefs.Save();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("LevelSelectMenu");
    }

    // ============================
    // MÉTODOS DE MONEDAS NORMALES
    // ============================

    public void AddNormalCoins(int amount)
    {
        int currentCoins = GetNormalCoins();
        int newTotal = currentCoins + amount;
        PlayerPrefs.SetInt(KEY_NORMAL_COINS, newTotal);

        MostrarMonedasGlobales display = FindObjectOfType<MostrarMonedasGlobales>();
        if (display != null)
        {
            display.UpdateCoinDisplay();
        }
    }

    public void SetTotalNormalCoins(int total)
    {
        PlayerPrefs.SetInt(KEY_NORMAL_COINS, total);
    }

    public int GetNormalCoins()
    {
        return PlayerPrefs.GetInt(KEY_NORMAL_COINS, 0);
    }

    // =============================
    // MÉTODOS DE MONEDAS ESPECIALES
    // =============================

    public bool CollectSpecialCoin(int levelIndex, int coinIndex)
    {
        string coinKey = KEY_SPECIAL_COIN_PREFIX + levelIndex + "_" + coinIndex;
        if (PlayerPrefs.GetInt(coinKey, 0) == 1) return false;
        PlayerPrefs.SetInt(coinKey, 1);
        return true;
    }

    public bool IsSpecialCoinCollected(int levelIndex, int coinIndex)
    {
        string coinKey = KEY_SPECIAL_COIN_PREFIX + levelIndex + "_" + coinIndex;
        return PlayerPrefs.GetInt(coinKey, 0) == 1;
    }

    public int GetTotalSpecialCoins(int totalLevels, int coinsPerLevel)
    {
        int totalCount = 0;
        for (int i = 1; i <= totalLevels; i++)
        {
            for (int j = 0; j < coinsPerLevel; j++)
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
        int currentHighest = GetHighestUnlockedLevel();
        if (levelIndex >= currentHighest)
        {
            PlayerPrefs.SetInt(KEY_HIGHEST_LEVEL, levelIndex);
        }
    }

    // --- ¡MÉTODO MODIFICADO! ---
    public int GetHighestUnlockedLevel()
    {
        // Devuelve el nivel más alto. Si no hay (default), devuelve 0.
        return PlayerPrefs.GetInt(KEY_HIGHEST_LEVEL, 0);
    }

    public void SaveAllData()
    {
        PlayerPrefs.Save();
        Debug.LogWarning("¡¡DATOS GUARDADOS MANUALMENTE EN DISCO!!");
    }
}