using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // valor de la moneda
    [Header("Audio")]
    public AudioClip pickupSound; // Sonido de recoger moneda
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCoins player = other.GetComponentInParent<PlayerCoins>();
            if (player != null)
            {
                player.AddCoins(coinValue);
            }

            // ?? Reproducir sonido desde el AudioSource del Player
            AudioSource audioSource = other.GetComponentInParent<AudioSource>();

            if (pickupSound != null)
            {
                GameObject tempAudio = new GameObject("TempCoinAudio");
                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();

                tempSource.clip = pickupSound;
                tempSource.volume = volume;
                tempSource.spatialBlend = 0f; // 2D, siempre suena igual
                tempSource.loop = false;
                tempSource.Play();

                Destroy(tempAudio, pickupSound.length); // destruir cuando acabe
            }


            // Destruir moneda
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject);
        }
    }
}
