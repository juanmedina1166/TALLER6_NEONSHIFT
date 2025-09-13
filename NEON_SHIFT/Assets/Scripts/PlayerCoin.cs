using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public static PlayerCoins Instance;
    public int coins = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        GameState.Instance.coins = coins; // ? guardar en GameState

        Debug.Log("Monedas: " + coins);

        // Actualizar UI
        UICoinManager.Instance.UpdateCoins(coins);
    }
}
