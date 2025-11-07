using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICoinManager : MonoBehaviour
{
    public static UICoinManager Instance; // Singleton simple
    public TextMeshProUGUI Moneda;   // Referencia al texto en el Canvas

    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateCoins(int coins)
    {
        Moneda.text = coins.ToString();
    }
}
