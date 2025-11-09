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
    public PanelAnimator mainMenuPanel;        
    public PanelAnimator escogerNivelPanel;    
    public PanelAnimator nameInputPanel;       
    public PanelAnimator leaderboardPanel;

    [Header("Entrada de Nombre")]
    public TMP_InputField nameInputField;

    
    [Header("Confirmación Nueva Partida")]
    [Tooltip("Arrastra el panel que pregunta '¿Estás seguro?'")]
    public PanelAnimator newGameConfirmPanel;
    [Tooltip("El botón 'SÍ' dentro del panel de confirmación")]
    public Button confirmNewGameButton;
    [Tooltip("El botón 'NO' dentro del panel de confirmación")]
    public Button cancelNewGameButton;
   

    void Start()
    {
        // --- Asignación de Listeners ---
        jugarButton.onClick.AddListener(OnJugarClicked);
        nuevaPartidaButton.onClick.AddListener(OnNuevaPartidaClicked);
        salirDeNivelesButton.onClick.AddListener(OnSalirDeNivelesClicked);
        confirmNameButton.onClick.AddListener(OnConfirmNameClicked);
        puntajesButton.onClick.AddListener(OnPuntajesClicked);
        cerrarPuntajesButton.onClick.AddListener(OnCerrarPuntajesClicked);
        confirmNewGameButton.onClick.AddListener(OnConfirmNewGame);
        cancelNewGameButton.onClick.AddListener(OnCancelNewGame);

       
        mainMenuPanel.ShowPanel();
        

        
        UpdateMainMenuButtons();
    }

   
    void UpdateMainMenuButtons()
    {
        if (SaveManager.Instance == null)
        {
            
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
        mainMenuPanel.HidePanel();        
        escogerNivelPanel.ShowPanel();
    }

    // --- ¡MÉTODO MODIFICADO! ---
    void OnNuevaPartidaClicked()
    {
        mainMenuPanel.HidePanel(); 

        if (SaveManager.Instance.HasSaveData())
        {
            newGameConfirmPanel.ShowPanel();
        }
        else
        {
            nameInputPanel.ShowPanel(); 
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
        nameInputPanel.HidePanel();      
        escogerNivelPanel.ShowPanel();
    }

    void OnPuntajesClicked()
    {
        mainMenuPanel.HidePanel();      
        leaderboardPanel.ShowPanel();
    }

    void OnCerrarPuntajesClicked()
    {
        leaderboardPanel.HidePanel(); 
        mainMenuPanel.ShowPanel();
    }

    void OnSalirDeNivelesClicked()
    {
        escogerNivelPanel.HidePanel(); 
        mainMenuPanel.ShowPanel();       
        UpdateMainMenuButtons();
    }

   
    void OnConfirmNewGame()
    {

        newGameConfirmPanel.HidePanel(); 
        nameInputPanel.ShowPanel();
    }

   
    void OnCancelNewGame()
    {
        
        newGameConfirmPanel.HidePanel(); 
        mainMenuPanel.ShowPanel();
    }
}