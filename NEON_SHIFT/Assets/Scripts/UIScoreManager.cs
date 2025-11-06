using UnityEngine;
using TMPro; 


public class UIScoreManager : MonoBehaviour
{
   
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
       
       
    }

    private void Start()
    {
        
        if (PlayerScore.Instance != null)
        {
            
            UpdateScoreText(PlayerScore.Instance.currentScore);
        }
        else
        {
           
            UpdateScoreText(0);
            Debug.LogWarning("UIScoreManager: PlayerScore.Instance no estaba listo en Start.");
        }
    }

    
    private void OnEnable()
    {
        
        PlayerScore.OnScoreChanged += UpdateScoreText;
    }

    
    private void OnDisable()
    {
        PlayerScore.OnScoreChanged -= UpdateScoreText;
    }

    
    private void UpdateScoreText(int newScore)
    {
        if (scoreText != null)
        {
            
            scoreText.text = "Puntaje: " + newScore;
        }
    }
}