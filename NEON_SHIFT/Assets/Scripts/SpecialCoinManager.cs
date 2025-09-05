using UnityEngine;
using UnityEngine.UI;

public class SpecialCoinManager : MonoBehaviour
{
    public static SpecialCoinManager Instance;

    [Header("UI de Monedas Especiales")]
    public Image[] coinIcons; // Asigna las 3 imágenes desde el inspector
    public Sprite filledSprite; // Sprite lleno que se muestra al recoger

    private bool[] collected = new bool[3];

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectCoin(int index)
    {
        if (index < 0 || index >= coinIcons.Length) return;
        if (collected[index]) return; // ya fue recogida

        collected[index] = true;
        coinIcons[index].sprite = filledSprite;
        coinIcons[index].color = Color.white; // ponerlo brillante
    }

    public int GetCollectedCount()
    {
        int count = 0;
        foreach (var c in collected)
            if (c) count++;
        return count;
    }
}
