using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Necesario para el hover

// Asegura que este script esté en un objeto con un Button
[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        // Añade un "listener" al botón.
        // Esto es lo mismo que configurar el "OnClick()" en el Inspector,
        // pero lo hacemos por código.
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Si el SoundManager existe, reproduce el sonido
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayClickSound();
        }
    }

    
   
}