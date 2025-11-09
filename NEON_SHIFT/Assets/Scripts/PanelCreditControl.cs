using UnityEngine;

public class PanelCreditControl : MonoBehaviour
{
    // Arrastra aquí el objeto 'ContenedorCreditos' de la jerarquía
    public Animator creditsAnimator;

    // Nombre del Trigger que creaste en el Animator (¡Debe coincidir!)
    private const string StartTriggerName = "StartCredits";

    void OnEnable()
    {
        // Esta función se llama justo en el momento en que el objeto (el panel) se activa (se hace visible).

        if (creditsAnimator != null)
        {
            creditsAnimator.Rebind();
            creditsAnimator.Update(0f);
            // Reinicia la posición del contenedor a la inicial
            // (Esto asume que el contenedor está en un objeto padre con un RectTransform)
            // Esto es crucial si vas a abrir los créditos más de una vez.
            creditsAnimator.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -900); // Ajusta la Y a tu posición inicial

            // 1. Vuelve al estado Idle
            // Esto es importante para que puedas activar el trigger nuevamente si cierras y abres el panel.
            creditsAnimator.Play("Idle", 0, 0f);

            // 2. Activa el Trigger, iniciando la animación de scroll
            creditsAnimator.SetTrigger(StartTriggerName);
        }
    }

    // Opcional: Para volver al menú cuando la animación termina
    // Puedes usar un Animation Event en tu clip de créditos para llamar a una función aquí.
    public void ReturnToMainMenu()
    {
        // ... Código para volver a cargar la escena del menú principal ...
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

        // Cierra el panel si no vas a cargar una escena
        gameObject.SetActive(false);
    }
}