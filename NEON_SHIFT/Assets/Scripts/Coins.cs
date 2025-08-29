using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // valor de la moneda

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí le sumamos la moneda al jugador
            PlayerCoins player = other.GetComponent<PlayerCoins>();
            if (player != null)
            {
                player.AddCoins(coinValue);
            }
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject);
        }
    }
}
