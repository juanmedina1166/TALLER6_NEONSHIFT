using UnityEngine;
using UnityEngine.UI;

public class SpecialCoinManager : MonoBehaviour
{
    public static SpecialCoinManager Instance;

    [Header("UI")]
    public Image[] coinIcons;
    public Sprite filledSprite;
    public Sprite emptySprite;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateUI()
    {
        if (SpecialCoinTracker.Instance == null) return;

        for (int i = 0; i < coinIcons.Length; i++)
        {
            bool collected = SpecialCoinTracker.Instance.IsCoinCollected(i);
            coinIcons[i].sprite = collected ? filledSprite : emptySprite;
            coinIcons[i].color = Color.white;
        }
    }
}
