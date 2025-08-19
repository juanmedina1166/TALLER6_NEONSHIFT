
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class SceneChanger : MonoBehaviour
{
    // Método para cargar una escena por nombre
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Método para cargar una escena por índice (en el Build Settings)
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Método para salir del juego (solo funciona en build, no en el editor)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("El juego se cerró."); // Esto lo verás solo en el editor
    }
}
