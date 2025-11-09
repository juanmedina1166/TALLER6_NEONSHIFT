using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // El patrón "Singleton" para que sea fácil de encontrar
    public static SoundManager Instance { get; private set; }

    [Header("Componentes")]
    public AudioSource uiAudioSource; // Arrastra tu AudioSource aquí

    [Header("Clips de UI")]
    public AudioClip uiClickSound;    // Arrastra tu archivo de sonido "click" aquí
    public AudioClip uiHoverSound;    // (Opcional) Sonido al pasar el cursor
    // ... puedes añadir más sonidos de UI aquí (error, success, etc.)

    void Awake()
    {
        // Configuración del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ¡Importante!
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Asegúrate de tener el AudioSource
        if (uiAudioSource == null)
        {
            uiAudioSource = GetComponent<AudioSource>();
        }
    }

    // Función pública que llamarán los botones
    public void PlayClickSound()
    {
        if (uiClickSound != null)
        {
            uiAudioSource.PlayOneShot(uiClickSound);
        }
    }

    
    
}