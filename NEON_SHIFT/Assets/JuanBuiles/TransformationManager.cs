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

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip flySound;
    public AudioClip strongSound;
    public AudioClip fastSound;
    public AudioClip wallBreakSound;

    // Estados
    private bool isFlying = false;
    private bool isStrong = false;
    private bool isFast = false;

    private bool isTransforming = false;

    void Start()
    {
        player = GetComponent<PlayerController>();

        ActivateModel(defaultModel);

        if (flyButton != null) flyButton.gameObject.SetActive(false);
        if (strongButton != null) strongButton.gameObject.SetActive(false);
        if (fastButton != null) fastButton.gameObject.SetActive(false);
    }

    private void ActivateModel(GameObject modelToActivate)
    {
        if (defaultModel != null) defaultModel.SetActive(false);
        if (flyModel != null) flyModel.SetActive(false);
        if (strongModel != null) strongModel.SetActive(false);
        if (fastModel != null) fastModel.SetActive(false);

        if (modelToActivate != null)
        {
            modelToActivate.SetActive(true);

            Animator anim = modelToActivate.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("IsRunning", true);
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // ---------------- FLY ----------------
    public void ActivateFly()
    {
        if (hasFlyPowerUp && !isFlying && !isTransforming)
        {
            PlaySound(flySound);
            StartCoroutine(FlyRoutine());
        }
    }

    private IEnumerator FlyRoutine()
    {
        isFlying = true;
        isTransforming = true;

        ActivateModel(flyModel);
        player.allowCustomY = true;

        float timer = 0f;
        Image fill = flyButton?.GetComponent<Image>();

        while (timer < flyDuration)
        {
            if (fill != null)
                fill.fillAmount = 1 - (timer / flyDuration);

            Vector3 pos = player.transform.position;
            pos.y = flyHeight;
            player.transform.position = pos;

            timer += Time.deltaTime;
            yield return null;
        }

        ActivateModel(defaultModel);
        player.allowCustomY = false;

        isFlying = false;
        isTransforming = false;

        hasFlyPowerUp = false;
        if (flyButton != null) flyButton.gameObject.SetActive(false);
    }

    // ---------------- STRONG ----------------
    public void ActivateStrong()
    {
        if (hasStrongPowerUp && !isStrong && !isTransforming)
        {
            PlaySound(strongSound);
            StartCoroutine(StrongRoutine());
        }
    }

    private IEnumerator StrongRoutine()
    {
        isStrong = true;
        isTransforming = true;

        ActivateModel(strongModel);

        float timer = 0f;
        Image fill = strongButton?.GetComponent<Image>();

        while (timer < strongDuration)
        {
            if (fill != null)
                fill.fillAmount = 1 - (timer / strongDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        ActivateModel(defaultModel);

        isStrong = false;
        isTransforming = false;

        hasStrongPowerUp = false;
        if (strongButton != null) strongButton.gameObject.SetActive(false);
    }

    // ---------------- FAST ----------------
    public void ActivateFast()
    {
        if (hasFastPowerUp && !isFast && !isTransforming)
        {
            PlaySound(fastSound);
            StartCoroutine(FastRoutine());
        }
    }

    private IEnumerator FastRoutine()
    {
        isFast = true;
        isTransforming = true;

        ActivateModel(fastModel);

        float originalSpeed = player.forwardSpeed;
        player.forwardSpeed *= fastSpeedMultiplier;

        float timer = 0f;
        Image fill = fastButton?.GetComponent<Image>();

        while (timer < fastDuration)
        {
            if (fill != null)
                fill.fillAmount = 1 - (timer / fastDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        player.forwardSpeed = originalSpeed;
        ActivateModel(defaultModel);

        isFast = false;
        isTransforming = false;

        hasFastPowerUp = false;
        if (fastButton != null) fastButton.gameObject.SetActive(false);
    }

    // ---------------- COLISION DESTRUIR PAREDES ----------------
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject target = hit.collider.gameObject;

        if ((isStrong || isFast) && target.CompareTag("Wall"))
        {
            if (wallBreakSound != null && audioSource != null)
                audioSource.PlayOneShot(wallBreakSound);

            Destroy(target);
        }
    }

    // ---------------- DESBLOQUEOS ----------------
    public void UnlockFast()
    {
        hasFastPowerUp = true;
        if (fastButton != null)
        {
            fastButton.gameObject.SetActive(true);
            fastButton.GetComponent<Image>().fillAmount = 1f; // reset
        }
    }

    public void UnlockFly()
    {
        hasFlyPowerUp = true;
        if (flyButton != null)
        {
            flyButton.gameObject.SetActive(true);
            flyButton.GetComponent<Image>().fillAmount = 1f; // reset
        }
    }

    public void UnlockStrong()
    {
        hasStrongPowerUp = true;
        if (strongButton != null)
        {
            strongButton.gameObject.SetActive(true);
            strongButton.GetComponent<Image>().fillAmount = 1f; // reset
        }
    }

    public bool IsFast() => isFast;
    public bool IsStrong() => isStrong;
}
