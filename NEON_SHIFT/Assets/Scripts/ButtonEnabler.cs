using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonEnabler : MonoBehaviour
{
    public TMP_InputField nameField;
    public Button submitButton;

    void Start()
    {
        // 1. Deshabilitamos el botón al empezar
        submitButton.interactable = false;

        // 2. Añadimos un "listener" al InputField que se activa CADA VEZ que el texto cambia
        nameField.onValueChanged.AddListener(ValidateInput);
    }

    // Esta función se llamará cada vez que el usuario escriba una letra
    public void ValidateInput(string newText)
    {
        // Comprobamos el texto actual
        if (string.IsNullOrWhiteSpace(newText))
        {
            // Si está vacío, el botón NO es interactuable
            submitButton.interactable = false;
        }
        else
        {
            // Si tiene texto, el botón SÍ es interactuable
            submitButton.interactable = true;
        }
    }
}