using UnityEngine;

public class SpecialCoin : MonoBehaviour
{
    public int coinIndex; // 0, 1, 2 según el orden de la moneda en el nivel
    public AudioClip pickupSound;
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Actualizar UI
            SpecialCoinManager.Instance.CollectCoin(coinIndex);

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
