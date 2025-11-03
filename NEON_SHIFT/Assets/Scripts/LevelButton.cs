using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Si usas TextMeshPro

public class LevelButton : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public int levelToLoad; // Índice del nivel que este botón carga (ej: 2)
    public string sceneToLoad; // Nombre de la escena (ej: "Level_2")

    [Header("Requisitos de Desbloqueo")]
    public int specialCoinsRequired;
    public int normalCoinsRequired;

    [Header("UI")]
    public Button button;
    public Image lockIcon; // Un icono de candado
    public TextMeshProUGUI requirementsText; // Texto para mostrar requisitos

    // Awake() o Start() se usa para cosas que solo pasan una vez,
    // como asignar el listener del botón.
    void Start()
    {
        // Asignamos el listener del clic SÓLO UNA VEZ
        button.onClick.AddListener(LoadLevel);

        // (Opcional) Hacemos una comprobación inicial
        // aunque OnEnable() lo hará de todas formas.
        UpdateLockStatus();
    }

    // OnEnable() se llama CADA VEZ que el objeto se activa.
    // Perfecto para cuando el panel "EscogerNivel" se muestra.
    void OnEnable()
    {
        // ¡Actualizamos el estado del botón cada vez que se muestra!
        UpdateLockStatus();
    }

    // Creamos una función separada para la lógica de bloqueo
    // para poder llamarla desde Start() y OnEnable() sin repetir código.
    void UpdateLockStatus()
    {
        // Asegurarnos de que el SaveManager existe
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("SaveManager.Instance no encontrado. El botón de nivel no puede actualizarse.");
            return;
        }

        // Requisito 1: ¿Está el nivel "desbloqueado" secuencialmente?
        bool levelUnlocked = (levelToLoad <= SaveManager.Instance.GetHighestUnlockedLevel());

        // Requisito 2: ¿Tiene suficientes monedas especiales?
        // Usamos las variables del SaveManager para los totales
        int totalSpecial = SaveManager.Instance.GetTotalSpecialCoins(
            SaveManager.Instance.totalLevelsInGame,
            SaveManager.Instance.specialCoinsPerLevel);
        bool hasSpecial = (totalSpecial >= specialCoinsRequired);

        // Requisito 3: ¿Tiene suficientes monedas normales?
        int totalNormal = SaveManager.Instance.GetNormalCoins();
        bool hasNormal = (totalNormal >= normalCoinsRequired);

        // Comprobación final
        if (levelUnlocked && hasSpecial && hasNormal)
        {
            // ¡Desbloqueado!
            button.interactable = true;
            if (lockIcon != null) lockIcon.gameObject.SetActive(false);
            if (requirementsText != null) requirementsText.gameObject.SetActive(false);
        }
        else
        {
            // Bloqueado
            button.interactable = false;
            if (lockIcon != null) lockIcon.gameObject.SetActive(true);

            // Mostrar requisitos
            if (requirementsText != null)
            {
                requirementsText.gameObject.SetActive(true);
                requirementsText.text = $"Necesitas:\n{specialCoinsRequired} Monedas Esp.\n{normalCoinsRequired} Monedas";
            }
        }
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}