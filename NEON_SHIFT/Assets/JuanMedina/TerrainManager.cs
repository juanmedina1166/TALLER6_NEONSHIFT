using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Transform player;              // Jugador o cámara
    public GameObject[] terrains;         // Todos los terrenos en orden (Terrain1, Terrain2, ..., TerrainN)
    public float terrainLength = 30f;     // Largo de cada bloque

    private int currentIndex = 0;         // El primer terreno activo
    private int nextToActivate = 4;       // El próximo terreno que debe aparecer

    void Start()
    {
        // Activar solo los 3 primeros
        for (int i = 0; i < terrains.Length; i++)
        {
            terrains[i].SetActive(i < 4);
        }
    }

    void Update()
    {
        if (currentIndex >= terrains.Length) return; // ya no quedan más

        Transform currentTerrain = terrains[currentIndex].transform;

        // Cuando el jugador pasa la mitad del terreno actual
        if (player.position.z - currentTerrain.position.z > terrainLength)
        {
            // Desactivar el terreno actual
            terrains[currentIndex].SetActive(false);

            // Activar el siguiente si existe
            if (nextToActivate < terrains.Length)
            {
                terrains[nextToActivate].SetActive(true);
                nextToActivate++;
            }

            // Avanzar al siguiente índice
            currentIndex++;
        }
    }
}
