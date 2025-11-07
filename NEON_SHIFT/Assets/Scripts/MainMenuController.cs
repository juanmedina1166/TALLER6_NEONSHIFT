using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Botones")]
    public Button jugarButton;
    public Button nuevaPartidaButton;
    public Button salirDeNivelesButton;
    public Button confirmNameButton;
    public Button puntajesButton;
    public Button cerrarPuntajesButton;

    [Header("Paneles")]
    public GameObject mainMenuPanel;
    public GameObject escogerNivelPanel;
    public GameObject nameInputPanel;
    public GameObject leaderboardPanel;

    [Header("Entrada de Nombre")]
    public TMP_InputField nameInputField;

    // --- ¡NUEVO! ---
    [Header("Confirmación Nueva Partida")]
    [Tooltip("Arrastra el panel que pregunta '¿Estás seguro?'")]
    public GameObject newGameConfirmPanel;
    [Tooltip("El botón 'SÍ' dentro del panel de confirmación")]
    public Button confirmNewGameButton;
    [Tooltip("El botón 'NO' dentro del panel de confirmación")]
    public Button cancelNewGameButton;
    // --- FIN DE LO NUEVO ---

    void Start()
    {
        // --- Asignación de Listeners ---
        jugarButton.onClick.AddListener(OnJugarClicked);
        nuevaPartidaButton.onClick.AddListener(OnNuevaPartidaClicked);
        salirDeNivelesButton.onClick.AddListener(OnSalirDeNivelesClicked);
        confirmNameButton.onClick.AddListener(OnConfirmNameClicked);
        puntajesButton.onClick.AddListener(OnPuntajesClicked);
        cerrarPuntajesButton.onClick.AddListener(OnCerrarPuntajesClicked);

        // --- ¡NUEVO! Listeners para el panel de confirmación ---
        confirmNewGameButton.onClick.AddListener(OnConfirmNewGame);
        cancelNewGameButton.onClick.AddListener(OnCancelNewGame);

        // --- Configuración Inicial de Paneles ---
        mainMenuPanel.SetActive(true);
        escogerNivelPanel.SetActive(false);
        nameInputPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        newGameConfirmPanel.SetActive(false); // ¡NUEVO! Ocultar panel al inicio

        // --- ¡NUEVO! Comprobar estado de botones ---
        UpdateMainMenuButtons();
    }

    /// <summary>
    /// ¡NUEVO! Revisa si hay datos guardados y actualiza
    /// la interactividad del botón "Jugar".
    /// </summary>
    void UpdateMainMenuButtons()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager no encontrado. Asegúrate de que esté en la escena.");
            return;
        }

        // Comprobamos si existe un archivo de guardado
        bool hasSaveData = SaveManager.Instance.HasSaveData();

        // El botón "Jugar" (Continuar) solo se puede presionar si hay datos guardados.
        // Poner 'interactable' en 'false' lo hará opaco automáticamente.
        jugarButton.interactable = hasSaveData;
    }

    void OnJugarClicked()
    {
        mainMenuPanel.SetActive(false);
        escogerNivelPanel.SetActive(true);
    }

    // --- ¡MÉTODO MODIFICADO! ---
    void OnNuevaPartidaClicked()
    {
        if (SaveManager.Instance.HasSaveData())
        {
            // GOAL 3: Si SÍ hay partida, mostrar panel de confirmación
            mainMenuPanel.SetActive(false);
            newGameConfirmPanel.SetActive(true);
        }
        else
        {
            // GOAL 4: Si NO hay partida, ir directo a poner el nombre
            mainMenuPanel.SetActive(false);
            nameInputPanel.SetActive(true);
        }
    }

    void OnConfirmNameClicked()
    {
        // 1. Guardar el nombre
        string playerName = nameInputField.text;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Player"; // Nombre por defecto
        }

        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.SetCurrentPlayerName(playerName);
        }

        // 2. Resetear el progreso del jugador (monedas/niveles)
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ResetProgress(); // Esto crea el Nivel 1
        }

        // 3. Actualizar estado de botones (ahora sí se puede "Jugar")
        UpdateMainMenuButtons();

        // 4. Ir a la selección de nivel
        nameInputPanel.SetActive(false);
        escogerNivelPanel.SetActive(true);
    }

    void OnPuntajesClicked()
    {
        mainMenuPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    void OnCerrarPuntajesClicked()
    {
        leaderboardPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void OnSalirDeNivelesClicked()
    {
        escogerNivelPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        // ¡NUEVO! Actualizar botones por si acaso
        UpdateMainMenuButtons();
    }

    // --- ¡NUEVOS MÉTODOS PARA EL PANEL DE CONFIRMACIÓN! ---

    /// <summary>
    /// Se llama al presionar "SÍ" en el panel de confirmación de nueva partida.
    /// </summary>
    void OnConfirmNewGame()
    {
        // El usuario está seguro. Lo mandamos a la pantalla de nombre.
        // El borrado real ocurrirá en 'OnConfirmNameClicked'.
        newGameConfirmPanel.SetActive(false);
        nameInputPanel.SetActive(true);
    }

    /// <summary>
    /// Se llama al presionar "NO" en el panel de confirmación.
    /// </summary>
    void OnCancelNewGame()
    {
        // El usuario canceló. Volvemos al menú principal.
        newGameConfirmPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}