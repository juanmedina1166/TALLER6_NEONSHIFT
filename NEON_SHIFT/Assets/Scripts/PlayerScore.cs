using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore Instance;
    public static event Action<int> OnScoreChanged;

    [Header("Puntaje")]
    [Tooltip("El puntaje actual en esta sesión/intento")]
    public int currentScore = 0;

    [Tooltip("El puntaje guardado en el último checkpoint")]
    private int checkpointScore = 0;

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
        checkpointScore = 0;

        OnScoreChanged?.Invoke(currentScore);
    }

    public void SaveCheckpointScore()
    {
        checkpointScore = currentScore;
        
    }

    
    /// Restaura el puntaje al valor guardado en el último checkpoint.
    
    public void RestoreCheckpointScore()
    {
        currentScore = checkpointScore;
        OnScoreChanged?.Invoke(currentScore); // Actualiza la UI
       
    }
}