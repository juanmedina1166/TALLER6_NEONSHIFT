using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCoins player = other.GetComponentInParent<PlayerCoins>();
            if (player != null)
                player.AddCoins(coinValue);

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 1f);

            // ? Guardar como destruida
            GameState.Instance.destroyedObjects.Add(gameObject.name);

            Destroy(gameObject);
        }
    }
}
