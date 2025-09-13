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

    [Header("Objetos destruidos")]
    public HashSet<string> destroyedObjects = new HashSet<string>();

    [Header("Objetos recogidos/destruidos desde el último checkpoint")]
    public List<GameObject> collectedSinceCheckpoint = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
