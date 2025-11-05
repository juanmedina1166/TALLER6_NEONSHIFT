using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public static PlayerCoins Instance;

    // 'coins' ahora se llama 'sessionCoins' (bolsillo) y SIEMPRE empieza en 0
    public int sessionCoins = 0;

    // Aquí guardamos el total global al inicio del nivel (banco)
    private int startingGlobalCoins = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        // 1. Obtenemos el total global (ej: 700)
        if (SaveManager.Instance != null)
        {
            startingGlobalCoins = SaveManager.Instance.GetNormalCoins();
        }

        // 2. sessionCoins es 0, así que la UI mostrará 700 + 0 = 700
        UpdateDisplay();
    }

    public void AddCoins(int amount)
    {
        // 1. Sumamos solo al bolsillo de la sesión
        sessionCoins += amount;

        // 2. Actualizamos la UI (mostrará 700 + 50 = 750)
        UpdateDisplay();
    }

    // Nueva función para actualizar la UI
    private void UpdateDisplay()
    {
        // El total a MOSTRAR es la suma de lo que tenías + lo que has recogido
        int displayTotal = startingGlobalCoins + sessionCoins;

        // (Asumo que tu script de UI se llama UICoinManager, basado en tu código)
        if (UICoinManager.Instance != null)
        {
            UICoinManager.Instance.UpdateCoins(displayTotal);
        }
    }

    public void SaveCheckpointCoins()
    {
        // El checkpoint solo guarda las monedas de la SESIÓN
        GameState.Instance.coins = sessionCoins;
    }

    public void RestoreCheckpointCoins()
    {
        // Restauramos solo las monedas de la SESIÓN
        sessionCoins = GameState.Instance.coins;
        UpdateDisplay(); // Actualizamos la UI para que muestre el total correcto
    }

    // ¡¡NUEVA FUNCIÓN!! La llamaremos desde los otros scripts
    public void ResetSession()
    {
        sessionCoins = 0;

        // Actualizamos el 'startingGlobalCoins' al valor global más reciente
        if (SaveManager.Instance != null)
        {
            startingGlobalCoins = SaveManager.Instance.GetNormalCoins();
        }

        UpdateDisplay();
    }
}