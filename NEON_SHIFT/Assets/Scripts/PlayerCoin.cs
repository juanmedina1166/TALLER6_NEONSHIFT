using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public static PlayerCoins Instance;
    public int coins = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Si ya existe una instancia (por ejemplo, en el jugador)
            // y este script está en otro objeto (como RespawnManager),
            // destruye este componente para evitar duplicados.
            Destroy(this);
        }
    }
    private void Start()
    {
        // Restaurar las monedas globales guardadas al inicio del nivel
        if (SaveManager.Instance != null)
        {
            coins = SaveManager.Instance.GetNormalCoins();
            UICoinManager.Instance.UpdateCoins(coins);
            Debug.Log("?? Monedas restauradas al iniciar nivel: " + coins);
        }
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
