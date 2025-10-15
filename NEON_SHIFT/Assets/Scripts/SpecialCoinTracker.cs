using UnityEngine;

public class SpecialCoinTracker : MonoBehaviour
{
    public static SpecialCoinTracker Instance;

    public bool[] collectedCoins = new bool[3]; // cambia el 3 si hay más monedas

    private void Awake()
    {
        Instance = this;
    }

    public void MarkCoinAsCollected(int index)
    {
        if (index >= 0 && index < collectedCoins.Length)
            collectedCoins[index] = true;
    }

    public bool IsCoinCollected(int index)
    {
        if (index >= 0 && index < collectedCoins.Length)
            return collectedCoins[index];
        return false;
    }
}
