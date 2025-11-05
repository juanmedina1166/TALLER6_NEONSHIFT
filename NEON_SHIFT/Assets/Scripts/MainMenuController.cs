using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ¡Asegúrate de tener esto!
using TMPro; // ¡Asegúrate de tener esto!

public class MainMenuController : MonoBehaviour
{
    [Header("Botones")]
    public Button jugarButton;
    public Button nuevaPartidaButton;
    public Button salirDeNivelesButton;
    public Button confirmNameButton; // ¡NUEVO! Arrastra tu botón "Confirmar" aquí
    public Button puntajesButton; // El botón "Puntajes" en el menú principal
    public Button cerrarPuntajesButton; // El botón "Volver" DENTRO del panel de puntajes

    [Header("Paneles")]
    public GameObject mainMenuPanel;
    public GameObject escogerNivelPanel;
    public GameObject nameInputPanel; // ¡NUEVO! Arrastra tu panel de nombre aquí
    public GameObject leaderboardPanel;

    [Header("Entrada de Nombre")]
    public TMP_InputField nameInputField; // ¡NUEVO! Arrastra tu InputField aquí

    void Start()
    {
        jugarButton.onClick.AddListener(OnJugarClicked);
        nuevaPartidaButton.onClick.AddListener(OnNuevaPartidaClicked);
        salirDeNivelesButton.onClick.AddListener(OnSalirDeNivelesClicked);
        confirmNameButton.onClick.AddListener(OnConfirmNameClicked); // ¡NUEVO!

        puntajesButton.onClick.AddListener(OnPuntajesClicked);
        cerrarPuntajesButton.onClick.AddListener(OnCerrarPuntajesClicked);

        mainMenuPanel.SetActive(true);
        escogerNivelPanel.SetActive(false);
        nameInputPanel.SetActive(false); // ¡NUEVO!

        leaderboardPanel.SetActive(false);
    }

    void OnJugarClicked()
    {
        mainMenuPanel.SetActive(false);
        escogerNivelPanel.SetActive(true);
    }

    // ¡MODIFICADO!
    void OnNuevaPartidaClicked()
    {
        // Ya no resetea. Solo abre el panel de nombre.
        mainMenuPanel.SetActive(false);
        nameInputPanel.SetActive(true);
    }

    // ¡NUEVO!
    void OnConfirmNameClicked()
    {
        // 1. Guardar el nombre
        string playerName = nameInputField.text;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Player"; // Nombre por defecto
        }
        LeaderboardManager.Instance.SetCurrentPlayerName(playerName);

        // 2. Resetear el progreso del jugador (monedas/niveles)
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ResetProgress();
        }

        nameInputPanel.SetActive(false);
        escogerNivelPanel.SetActive(true);


    }

    void OnPuntajesClicked()
    {
        // Oculta el menú principal y muestra los puntajes
        mainMenuPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    void OnCerrarPuntajesClicked()
    {
        // Oculta los puntajes y regresa al menú principal
        leaderboardPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void OnSalirDeNivelesClicked()
    {
        escogerNivelPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}