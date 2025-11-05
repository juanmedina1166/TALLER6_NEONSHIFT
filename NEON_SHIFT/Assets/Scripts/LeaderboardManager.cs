using UnityEngine;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    // La clave base para guardar en PlayerPrefs
    private const string LEADERBOARD_KEY_PREFIX = "Leaderboard_";
    // La clave para el nombre del jugador actual
    private const string PLAYER_NAME_KEY = "CurrentPlayerName";

    // Límite de entradas por tabla
    private const int MAX_ENTRIES = 10;

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

    // --- Métodos del Nombre de Jugador ---

    public void SetCurrentPlayerName(string name)
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, name);
        PlayerPrefs.Save();
    }

    public string GetCurrentPlayerName()
    {
        return PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player"); // Devuelve "Player" si no hay
    }

    // --- Métodos del Leaderboard ---

    /// <summary>
    /// Obtiene la tabla de posiciones para un nivel.
    /// </summary>
    public Leaderboard GetLeaderboard(int levelIndex)
    {
        string key = LEADERBOARD_KEY_PREFIX + levelIndex;
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<Leaderboard>(json);
        }
        return new Leaderboard(); // Devuelve una tabla vacía
    }

    /// <summary>
    /// Intenta registrar un nuevo puntaje.
    /// </summary>
    public void SubmitScore(int levelIndex, string playerName, int score)
    {
        // 1. Obtener la tabla actual
        Leaderboard lb = GetLeaderboard(levelIndex);

        // 2. Crear la nueva entrada
        ScoreEntry newEntry = new ScoreEntry { playerName = playerName, score = score };
        lb.entries.Add(newEntry);

        // 3. Ordenar la lista (de mayor a menor)
        lb.entries.Sort((a, b) => b.score.CompareTo(a.score));

        // 4. Recortar la lista a solo 10 entradas
        if (lb.entries.Count > MAX_ENTRIES)
        {
            lb.entries.RemoveRange(MAX_ENTRIES, lb.entries.Count - MAX_ENTRIES);
        }

        // 5. Guardar la lista actualizada como JSON
        string json = JsonUtility.ToJson(lb);
        PlayerPrefs.SetString(LEADERBOARD_KEY_PREFIX + levelIndex, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// (Opcional) Un método para borrar todas las tablas si lo necesitas.
    /// </summary>
    public void ClearAllLeaderboards(int totalLevels)
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            PlayerPrefs.DeleteKey(LEADERBOARD_KEY_PREFIX + i);
        }
        PlayerPrefs.Save();
    }
}