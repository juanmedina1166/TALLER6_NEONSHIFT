using UnityEngine;
using System.Collections.Generic;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    [Header("Progreso")]
    public int coins = 0;
    public bool[] specialCoins = new bool[3]; // 3 monedas especiales por nivel

    [Header("Checkpoint")]
    public Vector3 lastCheckpoint;
    public bool checkpointReached = false;
    public string lastCheckpointScene = ""; // NUEVO

    [Header("Objetos destruidos")]
    public HashSet<string> destroyedObjects = new HashSet<string>();

    [Header("Objetos recogidos/destruidos desde el último checkpoint")]
    public List<GameObject> collectedSinceCheckpoint = new List<GameObject>();

    [Header("Paneles de instrucciones mostrados")]
    public HashSet<string> shownInstructionTriggers = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ? mantiene el estado entre recargas
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    /// <summary>
    /// Marca un trigger como mostrado para no volver a mostrarlo tras respawn.
    /// </summary>
    public void MarkTriggerShown(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        if (!shownInstructionTriggers.Contains(id))
            shownInstructionTriggers.Add(id);
    }

    /// <summary>
    /// Devuelve true si el trigger ya fue mostrado antes.
    /// </summary>
    public bool IsTriggerShown(string id)
    {
        if (string.IsNullOrEmpty(id)) return true; // si no tiene ID, lo considera mostrado
        return shownInstructionTriggers.Contains(id);
    }
}
