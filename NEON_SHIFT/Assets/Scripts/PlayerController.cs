using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    // Movimiento general
    public float forwardSpeed = 10f;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 10f;

    // Control de carriles
    private int currentLane = 1;

    // Saltar
    public float jumpForce = 10f;
    public float gravity = -20f;
    private float verticalVelocity;

    // Slide
    private bool isSliding = false;
    private float originalHeight;
    private Vector3 originalCenter;

    // Input
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;

    // Transformaciones
    [HideInInspector] public bool allowCustomY = false;

    // ?? Audio
    [Header("Audio Clips")]
    public AudioClip jumpClip;
    public AudioClip slideClip;
    private AudioSource audioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        originalHeight = controller.height;
        originalCenter = controller.center;

        if (animator != null)
            animator.SetBool("IsRunning", true);
    }

    void Update()
    {
        HandleSwipeInput();

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

                if (swipeUp) // ?? Jump
                {
                    verticalVelocity = jumpForce;

                    if (animator != null)
                    {
                        animator.SetTrigger("Jump");
                        animator.SetBool("IsRunning", false);
                    }

                    PlaySound(jumpClip);
                }

                if (swipeDown && !isSliding) // ?? Slide
                {
                    StartCoroutine(Slide());
                    PlaySound(slideClip);
                }
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
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
        {
            currentLane++;
        }
        else if (swipeLeft && currentLane > 0)
        {
            currentLane--;
        }
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

    // ?? Reproducir sonido seguro
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
