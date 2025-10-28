using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        // Buscamos el script PlayerCoins en el objeto que colisionó O en sus padres.
        PlayerCoins player = other.GetComponentInParent<PlayerCoins>();

        // Si 'player' no es null, significa que lo que tocó la moneda es parte del jugador.
        if (player != null)
        {
            player.AddCoins(coinValue);

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 1f);

            // ? Desactivar en vez de destruir
            gameObject.SetActive(false);

            // ? Registrar en la lista de objetos "recogidos desde checkpoint"
            if (GameState.Instance != null && !GameState.Instance.collectedSinceCheckpoint.Contains(gameObject))
            {
                GameState.Instance.collectedSinceCheckpoint.Add(gameObject);
            }
        }
    }
}
