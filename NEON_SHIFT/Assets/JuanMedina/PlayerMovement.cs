using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidades")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f; // distancia entre carriles
    public float jumpForce = 7f;
    public float gravity = -20f; // gravedad personalizada
    public float slideDuration = 0.5f;

    [Header("Referencias")]
    public Transform groundCheck; // Empty child para detectar suelo
    private Vector3 groundBoxSize = new Vector3(2.5f, 2f, 2.5f);
    public LayerMask groundMask;

    private int currentLane = 0; // -1 izquierda, 0 centro, 1 derecha
    private Vector3 direction; // velocidad vertical
    private bool isSliding = false;

    private bool canMove = false;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove || !PlayerManager.instance.isAlive) return;

        // Movimiento hacia adelante automático
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.deltaTime;

        // Movimiento lateral hacia el carril objetivo
        Vector3 targetPosition = transform.position.z * Vector3.forward + Vector3.up * transform.position.y;
        if (currentLane == -1)
            targetPosition += Vector3.left * laneDistance;
        else if (currentLane == 1)
            targetPosition += Vector3.right * laneDistance;

        // Interpolación hacia el carril
        Vector3 moveToLane = Vector3.MoveTowards(transform.position, targetPosition, 15f * Time.deltaTime);

        // Movimiento vertical (salto / caída)
        if (IsGrounded() && direction.y < 0)
            direction.y = -2f; // mantenerlo pegado al suelo

        direction.y += gravity * Time.deltaTime;

        Vector3 verticalMove = Vector3.up * direction.y * Time.deltaTime;

        // Aplicar movimiento final
        rb.MovePosition(moveToLane + forwardMove + verticalMove);
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void MoveLeft()
    {
        if (currentLane > -1) currentLane--;
    }

    public void MoveRight()
    {
        if (currentLane < 1) currentLane++;
    }

    public void Jump()
    {
        if (IsGrounded())
            direction.y = jumpForce;
    }

    public void Slide()
    {
        if (!isSliding)
            StartCoroutine(SlideRoutine());
    }

    private bool IsGrounded()
    {
        return Physics.CheckBox(groundCheck.position, groundBoxSize / 2f, Quaternion.identity, groundMask);
    }

    System.Collections.IEnumerator SlideRoutine()
    {
        isSliding = true;
        // Reducir el collider aquí si quieres
        yield return new WaitForSeconds(slideDuration);
        // Restaurar el collider
        isSliding = false;
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
    }
}
