using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [Header("Movimiento general")]
    public float forwardSpeed = 10f;
    [HideInInspector] public float defaultForwardSpeed;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 10f;

    private int currentLane = 1;

    [Header("Saltar")]
    public float jumpForce = 10f;
    public float gravity = -20f;
    private float verticalVelocity;

    [Header("Slide")]
    private bool isSliding = false;
    private float originalHeight;
    private Vector3 originalCenter;

    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter = 0f;

    private bool swipeLeft, swipeRight, swipeUp, swipeDown;

    [HideInInspector] public bool allowCustomY = false;
    private bool isDead = false;

    [Header("Audio Clips")]
    public AudioClip jumpClip;
    public AudioClip slideClip;
    public AudioClip wallHitClip;  

    private AudioSource audioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        originalHeight = controller.height;
        originalCenter = controller.center;

        defaultForwardSpeed = forwardSpeed;

        if (animator != null)
            animator.SetBool("IsRunning", true);
    }

    void Update()
    {
        if (Time.timeScale == 0f || isDead) return;

        HandleSwipeInput();

        if (swipeUp) jumpBufferCounter = jumpBufferTime;
        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;

        Vector3 move = Vector3.zero;
        move.z = forwardSpeed;

        float targetX = (currentLane - 1) * laneDistance;
        float deltaX = targetX - transform.position.x;
        move.x = deltaX * laneChangeSpeed;

        if (!allowCustomY)
        {
            if (controller.isGrounded)
            {
                verticalVelocity = -1f;

                if (jumpBufferCounter > 0)
                {
                    verticalVelocity = jumpForce;
                    jumpBufferCounter = 0f;

                    if (animator != null)
                    {
                        animator.SetTrigger("Jump");
                        animator.SetBool("IsRunning", false);
                    }
                    PlaySound(jumpClip);
                }

                if (swipeDown && !isSliding)
                {
                    StartCoroutine(Slide());
                    PlaySound(slideClip);
                }
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
                if (swipeDown)
                    verticalVelocity = gravity * 2f;
            }

            move.y = verticalVelocity;
        }

        controller.Move(move * Time.deltaTime);
    }

    void HandleSwipeInput()
    {
        swipeLeft = SwipeManager.swipeLeft;
        swipeRight = SwipeManager.swipeRight;
        swipeUp = SwipeManager.swipeUp;
        swipeDown = SwipeManager.swipeDown;

        if (swipeRight && currentLane < 2)
            currentLane++;
        else if (swipeLeft && currentLane > 0)
            currentLane--;
    }

    IEnumerator Slide()
    {
        isSliding = true;
        if (animator != null)
        {
            animator.SetTrigger("Slide");
            animator.SetBool("IsRunning", false);
        }

        controller.height = originalHeight / 2f;
        controller.center = new Vector3(originalCenter.x, originalCenter.y / 2f, originalCenter.z);

        yield return new WaitForSeconds(1.0f);

        controller.height = originalHeight;
        controller.center = originalCenter;

        if (animator != null)
            animator.SetBool("IsRunning", true);

        isSliding = false;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    //  Detecta choque con paredes
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //  Detecta si el objeto está en el layer "Wall"
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Reproducir sonido solo si el jugador no está muerto (evita spam tras morir)
            if (!isDead)
                PlaySound(wallHitClip);
        }
    }

    // -------------------- MUERTE --------------------
    public void Die(Action onDeathComplete)
    {
        isDead = true;
        forwardSpeed = 0f;

        if (animator != null)
        {
            animator.SetTrigger("Die");
            StartCoroutine(WaitForDeathAnimation(onDeathComplete));
        }
        else
        {
            onDeathComplete?.Invoke();
        }
    }

    private IEnumerator WaitForDeathAnimation(Action onComplete)
    {
        bool enteredDie = false;
        while (!enteredDie)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Die"))
            {
                enteredDie = true;
                yield return new WaitForSeconds(state.length);
                break;
            }
            yield return null;
        }

        onComplete?.Invoke();
    }

    // -------------------- REAPARECER --------------------
    public void RestoreMovement()
    {
        isDead = false;
        forwardSpeed = defaultForwardSpeed;

        if (animator != null)
            animator.SetBool("IsRunning", true);
    }
}
