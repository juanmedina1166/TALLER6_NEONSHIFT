using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransformationManager : MonoBehaviour
{
    private PlayerController player;

    [Header("Fly Settings")]
    public float flyDuration = 2f;
    public float flyHeight = 5f;
    public float flyCooldown = 5f;

    [Header("UI")]
    public Button flyButton;

    [Header("Models")]
    public GameObject defaultModel; // cápsula
    public GameObject flyModel;     // cubo u otra forma

    private bool isFlying = false;
    private bool isOnCooldown = false;

    void Start()
    {
        player = GetComponent<PlayerController>();

        // Asegurarnos de que solo el modelo por defecto esté activo al inicio
        if (defaultModel != null) defaultModel.SetActive(true);
        if (flyModel != null) flyModel.SetActive(false);
    }

    public void ActivateFly()
    {
        if (!isFlying && !isOnCooldown)
        {
            StartCoroutine(FlyRoutine());
        }
    }

    private IEnumerator FlyRoutine()
    {
        isFlying = true;
        isOnCooldown = true;

        if (flyButton != null)
            flyButton.interactable = false;

        // Cambiar modelo
        if (defaultModel != null) defaultModel.SetActive(false);
        if (flyModel != null) flyModel.SetActive(true);

        // Desactivar gravedad
        player.allowCustomY = true;

        float timer = 0f;
        while (timer < flyDuration)
        {
            Vector3 pos = player.transform.position;
            pos.y = flyHeight;
            player.transform.position = pos;

            timer += Time.deltaTime;
            yield return null;
        }

        // Restaurar modelo
        if (defaultModel != null) defaultModel.SetActive(true);
        if (flyModel != null) flyModel.SetActive(false);

        // Restaurar físicas
        player.allowCustomY = false;
        isFlying = false;

        // Cooldown
        yield return new WaitForSeconds(flyCooldown);
        isOnCooldown = false;

        if (flyButton != null)
            flyButton.interactable = true;
    }
}
