using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Transform[] terrains; // Asigna Terrain1, Terrain2, Terrain3
    public float terrainLength = 30f; // Largo de cada bloque
    public Transform player; // El jugador o la c�mara que avanza

    private int nextTerrainIndex = 0;

    void Update()
    {
        Transform firstTerrain = terrains[nextTerrainIndex];

        // Detecta si el jugador ya pas� el bloque
        if (player.position.z - firstTerrain.position.z > terrainLength)
        {
            // Calcula la nueva posici�n adelante del �ltimo bloque
            int lastTerrainIndex = (nextTerrainIndex + terrains.Length - 1) % terrains.Length;
            float newZ = terrains[lastTerrainIndex].position.z + terrainLength;

            // Mueve el bloque entero con sus hijos (carriles, obst�culos, etc.)
            firstTerrain.position = new Vector3(firstTerrain.position.x, firstTerrain.position.y, newZ);

            // Avanza el �ndice para el pr�ximo reciclaje
            nextTerrainIndex = (nextTerrainIndex + 1) % terrains.Length;
        }
    }
}
