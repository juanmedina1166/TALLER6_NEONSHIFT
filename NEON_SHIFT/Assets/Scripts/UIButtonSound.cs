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

       
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayClickSound();
        }
    }

    
   
}