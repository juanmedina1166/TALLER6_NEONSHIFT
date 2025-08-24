using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator; // ?? referencia al Animator del modelo visual

    // Movimiento general
    public float forwardSpeed = 10f;
    public float laneDistance = 2f; // distancia entre carriles
    public float laneChangeSpeed = 10f;

    // Control de carriles
    private int currentLane = 1; // 0 = izquierda, 1 = centro, 2 = derecha

    // Saltar
    public float jumpForce = 10f;
    public float gravity = -20f;
    private float verticalVelocity;

    // Agacharse (slide)
    private bool isSliding = false;
    private float originalHeight;
    private Vector3 originalCenter;

    // Input de swipes
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;

    // Transformaciones (ej: volar)
    [HideInInspector] public bool allowCustomY = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); // busca el Animator en el modelo hijo

        // Guardar valores originales del CharacterController
        originalHeight = controller.height;
        originalCenter = controller.center;

        // Empezar corriendo
        if (animator != null)
            animator.SetBool("IsRunning", true);
    }

    void Update()
    {
        HandleSwipeInput();

        Vector3 move = Vector3.zero;
        move.z = forwardSpeed; // movimiento hacia adelante

        // Movimiento lateral (carriles)
        float targetX = (currentLane - 1) * laneDistance;
        float deltaX = targetX - transform.position.x;
        move.x = deltaX * laneChangeSpeed;

        // Movimiento vertical (saltar y gravedad)
        if (!allowCustomY)
        {
            if (controller.isGrounded)
            {
                verticalVelocity = -1f;

                if (swipeUp) // Salto
                {
                    verticalVelocity = jumpForce;
                    if (animator != null)
                    {
                        animator.SetTrigger("Jump");
                        animator.SetBool("IsRunning", false);
                    }
                }

                if (swipeDown && !isSliding) // Slide
                {
                    StartCoroutine(Slide());
                }
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            move.y = verticalVelocity;
        }

        // Aplicar movimiento
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

        // Reducir tamaño del CharacterController
        controller.height = originalHeight / 2f;
        controller.center = new Vector3(originalCenter.x, originalCenter.y / 2f, originalCenter.z);

        yield return new WaitForSeconds(1.0f); // duración del slide

        // Restaurar CharacterController
        controller.height = originalHeight;
        controller.center = originalCenter;

        if (animator != null)
            animator.SetBool("IsRunning", true);

        isSliding = false;
    }
}
