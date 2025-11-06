
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
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveAllData();
        }
        SceneManager.LoadScene(sceneIndex);
    }

    // Método para salir del juego (solo funciona en build, no en el editor)
    public void QuitGame()
    {
        Application.Quit();
    }
}
