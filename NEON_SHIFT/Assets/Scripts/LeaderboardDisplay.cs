using UnityEngine;
using TMPro;

public class LeaderboardDisplay : MonoBehaviour
{
    [Header("Configuración")]
    public int levelIndexToShow; // ¡IMPORTANTE! Pon aquí el nivel (ej: 1 para Nivel 1)

    [Header("Referencias UI (10 por cada)")]
    // Arrastra tus 10 textos de "Rank" (ej: "1.", "2.", etc.)
    public TextMeshProUGUI[] rankTexts;
    // Arrastra tus 10 textos de Nombre
    public TextMeshProUGUI[] nameTexts;
    // Arrastra tus 10 textos de Puntaje
    public TextMeshProUGUI[] scoreTexts;

    void OnEnable()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (LeaderboardManager.Instance == null) return;

        // 1. Obtener la tabla de posiciones
        Leaderboard lb = LeaderboardManager.Instance.GetLeaderboard(levelIndexToShow);

        // 2. Llenar los campos de texto
        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (i < lb.entries.Count)
            {
                // Hay una entrada para esta fila
                rankTexts[i].text = (i + 1).ToString() + ".";
                nameTexts[i].text = lb.entries[i].playerName;
                scoreTexts[i].text = lb.entries[i].score.ToString();
            }
            else
            {
                // No hay entrada, mostrar vacío
                rankTexts[i].text = (i + 1).ToString() + ".";
                nameTexts[i].text = "---";
                scoreTexts[i].text = "---";
            }
        }
    }
}