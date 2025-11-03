using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Botones")]
    public Button jugarButton;
    public Button nuevaPartidaButton;
    public Button salirDeNivelesButton;
    // (Añade aquí tus botones de Creditos, Salir, etc. si quieres)

    [Header("Paneles")]
    public GameObject mainMenuPanel; // El panel con "Jugar", "Creditos", etc.
    public GameObject escogerNivelPanel; // El panel "Escoger Nivel pro"


    void Start()
    {
        // Asignamos las funciones a los clics de los botones
        jugarButton.onClick.AddListener(OnJugarClicked);
        nuevaPartidaButton.onClick.AddListener(OnNuevaPartidaClicked);
        salirDeNivelesButton.onClick.AddListener(OnSalirDeNivelesClicked);

        // Asegurarnos de que empezamos en el menú principal
        mainMenuPanel.SetActive(true);
        escogerNivelPanel.SetActive(false);
    }

    // Esta función es para el botón "JUGAR"
    void OnJugarClicked()
    {
        // NO borramos nada.
        // Simplemente mostramos el panel de niveles.
        mainMenuPanel.SetActive(false);
        escogerNivelPanel.SetActive(true);

        // Los scripts "LevelButton" se ejecutarán y mostrarán el progreso actual
    }

    // Esta función es para el botón "NUEVA PARTIDA"
    void OnNuevaPartidaClicked()
    {
        // 1. Borramos el progreso llamando a la función del SaveManager
        if (SaveManager.Instance != null)
        {
            // ¡Usamos ResetProgress, que NO carga una escena!
            SaveManager.Instance.ResetProgress();
        }

        // 2. Ahora, mostramos el panel de niveles
        mainMenuPanel.SetActive(false);
        escogerNivelPanel.SetActive(true);

        // Los scripts "LevelButton" se ejecutarán y verán que el progreso
        // se acaba de reiniciar, por lo que solo mostrarán el Tutorial.
    }
    void OnSalirDeNivelesClicked()
    {
        // Ocultamos el panel de selección de niveles
        escogerNivelPanel.SetActive(false);

        // Mostramos el menú principal
        mainMenuPanel.SetActive(true);
    }
}