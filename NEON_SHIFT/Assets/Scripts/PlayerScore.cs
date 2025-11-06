using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore Instance;
    public static event Action<int> OnScoreChanged;

    // El puntaje de esta sesión/intento
    public int currentScore = 0;

    // (Opcional) Referencia a la UI de puntaje
    // public TextMeshProUGUI scoreText; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        // Siempre empezamos un nivel con 0 puntos de sesión
        ResetSessionScore();
    }

    // Método para sumar puntos
    public void AddScore(int amount)
    {
        currentScore += amount;
        // if (scoreText != null) scoreText.text = currentScore.ToString();

        OnScoreChanged?.Invoke(currentScore);
    }
   

    // Método para resetear el puntaje de la sesión
    public void ResetSessionScore()
    {
        currentScore = 0;
        // if (scoreText != null) scoreText.text = "0";

        OnScoreChanged?.Invoke(currentScore);
    }
}