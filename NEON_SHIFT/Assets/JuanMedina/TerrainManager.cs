using UnityEngine;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour
{
    // Hacemos que sea un Singleton para accederlo fácilmente desde otros scripts.
    public static TerrainManager Instance;

    public Transform player;              // Jugador o cámara
    public GameObject[] terrains;         // Todos los terrenos en orden (Terrain1, Terrain2, ..., TerrainN)
    public List<float> terrainLengths = new List<float>();

    [Header("Configuración de la Ventana")]
    public int chunksBehind = 1; // Cuántos terrenos mantener activos DETRÁS del jugador.
    public int chunksAhead = 2;  // Cuántos terrenos mantener activos DELANTE del jugador.

    private int lastPlayerChunk = -1; // Para no recalcular en cada frame.
    private List<float> chunkEndPositions = new List<float>();

    void Awake()
    {
        if (Instance == null)
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Pre-calculamos las posiciones donde termina cada chunk
        float cumulativePosition = 0;
        for (int i = 0; i < terrainLengths.Count; i++)
        {
            cumulativePosition += terrainLengths[i];
            chunkEndPositions.Add(cumulativePosition);
        }

        UpdateVisibleChunksBasedOnPosition(player.position);
    }

    void Update()
    {
        int currentPlayerChunk = GetPlayerCurrentChunk();

        if (currentPlayerChunk != lastPlayerChunk)
        {
            lastPlayerChunk = currentPlayerChunk;
            UpdateVisibleChunks();
        }
    }
    // Nueva función para encontrar el chunk actual
    private int GetPlayerCurrentChunk()
    {
        float playerZ = player.position.z;
        // Buscamos en qué rango de posiciones se encuentra el jugador
        for (int i = 0; i < chunkEndPositions.Count; i++)
        {
            if (playerZ < chunkEndPositions[i])
            {
                return i;
            }
        }
        return chunkEndPositions.Count - 1; // Si está más allá del último
    }

    // --- ESTA ES LA FUNCIÓN CLAVE ---
    // Activa y desactiva los terrenos según la posición actual del jugador.
    private void UpdateVisibleChunks()
    {
        // Calculamos el rango de chunks que deben estar visibles.
        int minChunk = lastPlayerChunk - chunksBehind;
        int maxChunk = lastPlayerChunk + chunksAhead;

        // Recorremos la lista de TODOS los terrenos.
        for (int i = 0; i < terrains.Length; i++)
        {
            // Si el terreno 'i' está dentro de nuestra ventana, lo activamos.
            if (i >= minChunk && i <= maxChunk)
            {
                terrains[i].SetActive(true);
            }
            else // Si está fuera, lo desactivamos.
            {
                terrains[i].SetActive(false);
            }
        }
    }
    // La función de respawn ahora sí puede usar la posición
    public void UpdateTerrainsOnRespawn(Vector3 respawnPosition)
    {
        UpdateVisibleChunksBasedOnPosition(respawnPosition);
    }

    private void UpdateVisibleChunksBasedOnPosition(Vector3 position)
    {
        float playerZ = position.z;
        int respawnChunk = 0;
        for (int i = 0; i < chunkEndPositions.Count; i++)
        {
            if (playerZ < chunkEndPositions[i])
            {
                respawnChunk = i;
                break;
            }
        }

        lastPlayerChunk = respawnChunk;
        UpdateVisibleChunks();
    }
}