using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance; // Singleton opcional

    [Header("Estado")]
    public bool isAlive = true;
    public bool isInvincible = false;

    [Header("Puntaje")]
    public int score = 0;
    public int coins = 0;

    [Header("Referencias")]
    public PlayerMovement movement;
    //public PlayerCollision collision;
    public PlayerInput input;

    private void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        isAlive = true;
        //score = 0;
        //coins = 0;
        movement.EnableMovement();
    }

    public void Die()
    {
        isAlive = false;
        movement.DisableMovement();

    }

    //public void AddScore(int amount)
    //{
        //score += amount;
        // Actualizar UI
   // }

    //public void AddCoin(int amount)
    //{
        //coins += amount;
        // Actualizar UI
    //}
}
