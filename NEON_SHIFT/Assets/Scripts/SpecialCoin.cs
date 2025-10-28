using UnityEngine;

public class SpecialCoin : MonoBehaviour
{
    public int coinIndex; // 0, 1, 2 según el orden de la moneda en el nivel
    public AudioClip pickupSound;
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que colisionó (o su padre) tiene el script PlayerController
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            if (SpecialCoinTracker.Instance != null)
                SpecialCoinTracker.Instance.MarkCoinAsCollected(coinIndex);

            // Mostrar popup visual
            if (SpecialCoinTracker.Instance != null && SpecialCoinPopup.Instance != null)
            {
                int collectedCount = 0;
                foreach (bool collected in SpecialCoinTracker.Instance.collectedCoins)
                    if (collected) collectedCount++;

                SpecialCoinPopup.Instance.ShowPopup(collectedCount);
            }

            // Reproducir sonido
            if (pickupSound != null)
            {
                GameObject tempAudio = new GameObject("TempSpecialCoinAudio");
                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();

                tempSource.clip = pickupSound;
                tempSource.volume = volume;
                tempSource.spatialBlend = 0f;
                tempSource.loop = false;
                tempSource.Play();

                Destroy(tempAudio, pickupSound.length);
            }

            Destroy(gameObject);
        }
    }
}
