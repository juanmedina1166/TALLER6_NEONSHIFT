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
    private bool hasFlyPowerUp = false;

    [Header("Strong Settings")]
    public float strongDuration = 3f;
    public float strongCooldown = 6f;
    private bool hasStrongPowerUp = false;

    [Header("Fast Settings")]
    public float fastDuration = 4f;
    public float fastSpeedMultiplier = 2.5f;
    private bool hasFastPowerUp = false;

    [Header("UI Buttons")]
    public Button flyButton;
    public Button strongButton;
    public Button fastButton;

    [Header("Models")]
    public GameObject defaultModel;
    public GameObject flyModel;
    public GameObject strongModel;
    public GameObject fastModel;

    // Estados
    private bool isFlying = false;
    private bool isStrong = false;
    private bool isFast = false;

    // Exclusividad
    private bool isTransforming = false;

    void Start()
    {
        player = GetComponent<PlayerController>();

        // Modelos iniciales
        if (defaultModel != null) defaultModel.SetActive(true);
        if (flyModel != null) flyModel.SetActive(false);
        if (strongModel != null) strongModel.SetActive(false);
        if (fastModel != null) fastModel.SetActive(false);

        // Ocultar botones hasta que se consigan los objetos
        if (flyButton != null) flyButton.gameObject.SetActive(false);
        if (strongButton != null) strongButton.gameObject.SetActive(false);
        if (fastButton != null) fastButton.gameObject.SetActive(false);
    }

    // ---------------- FLY ----------------
    public void ActivateFly()
    {
        if (hasFlyPowerUp && !isFlying && !isTransforming)
        {
            StartCoroutine(FlyRoutine());
        }
    }

    private IEnumerator FlyRoutine()
    {
        isFlying = true;
        isTransforming = true;

        if (flyButton != null) flyButton.interactable = false;

        if (defaultModel != null) defaultModel.SetActive(false);
        if (flyModel != null) flyModel.SetActive(true);

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

        if (defaultModel != null) defaultModel.SetActive(true);
        if (flyModel != null) flyModel.SetActive(false);

        player.allowCustomY = false;
        isFlying = false;
        isTransforming = false;

        // Consumido el power-up
        hasFlyPowerUp = false;
        if (flyButton != null) flyButton.gameObject.SetActive(false);
    }

    // ---------------- STRONG ----------------
    public void ActivateStrong()
    {
        if (hasStrongPowerUp && !isStrong && !isTransforming)
        {
            StartCoroutine(StrongRoutine());
        }
    }

    private IEnumerator StrongRoutine()
    {
        isStrong = true;
        isTransforming = true;

        if (strongButton != null) strongButton.interactable = false;

        if (defaultModel != null) defaultModel.SetActive(false);
        if (strongModel != null) strongModel.SetActive(true);

        float timer = 0f;
        while (timer < strongDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (defaultModel != null) defaultModel.SetActive(true);
        if (strongModel != null) strongModel.SetActive(false);

        isStrong = false;
        isTransforming = false;

        // Consumido el power-up
        hasStrongPowerUp = false;
        if (strongButton != null) strongButton.gameObject.SetActive(false);
    }

    // ---------------- FAST ----------------
    public void ActivateFast()
    {
        if (hasFastPowerUp && !isFast && !isTransforming)
        {
            StartCoroutine(FastRoutine());
        }
    }

    private IEnumerator FastRoutine()
    {
        isFast = true;
        isTransforming = true;

        if (fastButton != null) fastButton.interactable = false;

        if (defaultModel != null) defaultModel.SetActive(false);
        if (fastModel != null) fastModel.SetActive(true);

        float originalSpeed = player.forwardSpeed;
        player.forwardSpeed *= fastSpeedMultiplier;

        float timer = 0f;
        while (timer < fastDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Restaurar
        player.forwardSpeed = originalSpeed;

        if (defaultModel != null) defaultModel.SetActive(true);
        if (fastModel != null) fastModel.SetActive(false);

        isFast = false;
        isTransforming = false;

        // Consumido el power-up
        hasFastPowerUp = false;
        if (fastButton != null) fastButton.gameObject.SetActive(false);
    }

    // ---------------- COLISION DESTRUIR PAREDES ----------------
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject target = hit.collider.gameObject;

        if ((isStrong || isFast) && target.CompareTag("Wall"))
        {
            Destroy(target);
        }
    }

    // ---------------- DESBLOQUEOS ----------------
    public void UnlockFast()
    {
        hasFastPowerUp = true;
        if (fastButton != null) fastButton.gameObject.SetActive(true);
    }

    public void UnlockFly()
    {
        hasFlyPowerUp = true;
        if (flyButton != null) flyButton.gameObject.SetActive(true);
    }

    public void UnlockStrong()
    {
        hasStrongPowerUp = true;
        if (strongButton != null) strongButton.gameObject.SetActive(true);
    }
}

