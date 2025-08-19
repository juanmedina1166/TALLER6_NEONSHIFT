
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class SceneChanger : MonoBehaviour
{
    // M�todo para cargar una escena por nombre
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // M�todo para cargar una escena por �ndice (en el Build Settings)
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // M�todo para salir del juego (solo funciona en build, no en el editor)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("El juego se cerr�."); // Esto lo ver�s solo en el editor
    }
}
