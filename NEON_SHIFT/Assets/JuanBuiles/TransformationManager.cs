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

    [Header("Fast Settings")]
    public float fastDuration = 4f;
    public float fastSpeedMultiplier = 2.5f; // multiplica la velocidad del player
    private bool hasFastPowerUp = false; // si ya recogió el objeto

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
        if (fastModel != null) fastModel.SetActive(false);

        // Ocultar el botón rápido hasta que se consiga el objeto
        if (fastButton != null) fastButton.gameObject.SetActive(false);
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

        yield return new WaitForSeconds(strongCooldown);
        isOnCooldownStrong = false;

        if (strongButton != null) strongButton.interactable = true;
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

        // Consumido el power-up, ocultar botón
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

    // ---------------- POWER UP FAST ----------------
    public void UnlockFast()
    {
        hasFastPowerUp = true;
        if (fastButton != null) fastButton.gameObject.SetActive(true);
    }
}
