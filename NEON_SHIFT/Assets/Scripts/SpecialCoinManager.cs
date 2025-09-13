using UnityEngine;
using UnityEngine.UI;

public class SpecialCoinManager : MonoBehaviour
{
    public static SpecialCoinManager Instance;

    [Header("UI")]
    public Image[] coinIcons;
    public Sprite filledSprite;

    private bool[] collected;

    private void Awake()
    {
        Instance = this;
        collected = new bool[coinIcons.Length];
    }

    public void CollectCoin(int index)
    {
        if (index < 0 || index >= coinIcons.Length) return;
        if (collected[index]) return;

        collected[index] = true;
        GameState.Instance.specialCoins[index] = true; // ? guardar en GameState

        coinIcons[index].sprite = filledSprite;
        coinIcons[index].color = Color.white;
    }
}
