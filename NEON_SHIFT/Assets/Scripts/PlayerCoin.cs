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
        // ?? Ya NO actualizamos GameState aquí
        UICoinManager.Instance.UpdateCoins(coins);
    }

    public void SaveCheckpointCoins()
    {
        GameState.Instance.coins = coins;
    }

    public void RestoreCheckpointCoins()
    {
        coins = GameState.Instance.coins;
        UICoinManager.Instance.UpdateCoins(coins);
    }
}
