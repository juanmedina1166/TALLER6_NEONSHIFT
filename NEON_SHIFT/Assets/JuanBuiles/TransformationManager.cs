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

    [Header("Strong Settings")]
    public float strongDuration = 3f;
    public float strongCooldown = 6f;

    [Header("UI Buttons")]
    public Button flyButton;
    public Button strongButton;

    [Header("Models")]
    public GameObject defaultModel;
    public GameObject flyModel;
    public GameObject strongModel;

    // Estados
    private bool isFlying = false;
    private bool isStrong = false;
    private bool isOnCooldownFly = false;
    private bool isOnCooldownStrong = false;

    // Exclusividad
    private bool isTransforming = false;

    void Start()
    {
        player = GetComponent<PlayerController>();

        // Modelos iniciales
        if (defaultModel != null) defaultModel.SetActive(true);
        if (flyModel != null) flyModel.SetActive(false);
        if (strongModel != null) strongModel.SetActive(false);
    }

    // ---------------- FLY ----------------
    public void ActivateFly()
    {
        if (!isFlying && !isOnCooldownFly && !isTransforming)
        {
            StartCoroutine(FlyRoutine());
        }
    }

    private IEnumerator FlyRoutine()
    {
        isFlying = true;
        isTransforming = true;
        isOnCooldownFly = true;

        if (flyButton != null) flyButton.interactable = false;

        // Cambiar modelo
        if (defaultModel != null) defaultModel.SetActive(false);
        if (flyModel != null) flyModel.SetActive(true);

        // Desactiva gravedad
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
        isTransforming = false;

        // Cooldown
        yield return new WaitForSeconds(flyCooldown);
        isOnCooldownFly = false;

        if (flyButton != null) flyButton.interactable = true;
    }

    // ---------------- STRONG ----------------
    public void ActivateStrong()
    {
        if (!isStrong && !isOnCooldownStrong && !isTransforming)
        {
            StartCoroutine(StrongRoutine());
        }
    }

    private IEnumerator StrongRoutine()
    {
        isStrong = true;
        isTransforming = true;
        isOnCooldownStrong = true;

        if (strongButton != null) strongButton.interactable = false;

        // Cambiar modelo
        if (defaultModel != null) defaultModel.SetActive(false);
        if (strongModel != null) strongModel.SetActive(true);

        float timer = 0f;
        while (timer < strongDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Restaurar modelo
        if (defaultModel != null) defaultModel.SetActive(true);
        if (strongModel != null) strongModel.SetActive(false);

        isStrong = false;
        isTransforming = false;

        // Cooldown
        yield return new WaitForSeconds(strongCooldown);
        isOnCooldownStrong = false;

        if (strongButton != null) strongButton.interactable = true;
    }

    // ---------------- COLISION DESTRUIR PAREDES ----------------
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject target = hit.collider.gameObject;

        if (isStrong && target.CompareTag("Wall"))
        {
            Destroy(target);
        }
    }
}
