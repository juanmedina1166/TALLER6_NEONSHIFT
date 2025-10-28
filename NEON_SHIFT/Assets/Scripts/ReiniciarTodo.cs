using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReiniciarTodo : MonoBehaviour
{
    public Button resetButton; // Arrastra tu nuevo botón "Reiniciar Partida" aquí

    void Start()
    {
        if (resetButton != null)
        {
            resetButton = GetComponent<Button>();
        }
        resetButton.onClick.AddListener(OnResetClicked);
    }

    void OnResetClicked()
    {
        if (SaveManager.Instance != null)
        {
            // Opcional: Añadir un panel de confirmación aquí

            // Llama a la función que borra el progreso y carga el primer nivel
            SaveManager.Instance.NewGame();
        }
    }
}