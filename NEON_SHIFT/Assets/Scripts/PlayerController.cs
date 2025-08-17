using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

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

    // Agacharse
    private bool isSliding = false;
    private float originalHeight;
    private Vector3 originalCenter;

    // Input de swipes
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;

    // Transformaciones
    [HideInInspector] public bool allowCustomY = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Guardar valores originales del CharacterController
        originalHeight = controller.height;
        originalCenter = controller.center;
    }

    void Update()
    {
        HandleSwipeInput();

        Vector3 move = Vector3.zero;

        // Movimiento hacia adelante
        move.z = forwardSpeed;

        // Movimiento entre carriles (suavizado con Lerp)
        float targetX = (currentLane - 1) * laneDistance;
        float deltaX = targetX - transform.position.x;
        move.x = deltaX * laneChangeSpeed;

        // Movimiento vertical (saltar y gravedad)
        if (!allowCustomY)
        {
            if (controller.isGrounded)
            {
                verticalVelocity = -1f;

                if (swipeUp)
                {
                    verticalVelocity = jumpForce;
                }

                if (swipeDown && !isSliding)
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

        // Reducir la altura a la mitad
        controller.height = originalHeight / 2f;
        controller.center = new Vector3(originalCenter.x, originalCenter.y / 2f, originalCenter.z);

        yield return new WaitForSeconds(1.0f); // duración del slide

        // Restaurar los valores originales
        controller.height = originalHeight;
        controller.center = originalCenter;

        isSliding = false;
    }
}
