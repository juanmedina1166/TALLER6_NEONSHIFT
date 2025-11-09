using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraFlyBob : MonoBehaviour
{
    [Header("Fly Bob Settings")]
    public float amplitude = 0.4f;
    public float frequency = 2.2f;
    public float rollAmount = 4f;
    public float restoreSpeed = 5f;

    [Header("Flight Camera Offset")]
    public Vector3 flyOffset = new Vector3(0, 2f, -5f); // se aleja y sube un poco
    public float offsetLerpSpeed = 2f;

    [Header("Flight Camera Rotation")]
    public Vector3 flyRotation = new Vector3(10f, 0f, 0f); // mira levemente hacia abajo
    public float rotationLerpSpeed = 3f; // velocidad de transición de rotación

    private bool isFlying = false;
    private CinemachineCamera vcam;
    private CinemachineFollow follow;
    private Vector3 baseOffset;
    private Quaternion baseRotation;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        if (vcam != null)
            follow = vcam.GetComponent<CinemachineFollow>();

        if (follow != null)
            baseOffset = follow.FollowOffset;

        baseRotation = transform.localRotation;
    }

    void Update()
    {
        if (follow == null) return;

        if (isFlying)
        {
            // Movimiento de vuelo + alejamiento
            float t = Time.time * frequency;
            float y = Mathf.Sin(t) * amplitude;
            float roll = Mathf.Sin(t * 0.6f) * rollAmount;

            // Offset dinámico
            Vector3 targetOffset = baseOffset + flyOffset + new Vector3(0, y, 0);
            follow.FollowOffset = Vector3.Lerp(follow.FollowOffset, targetOffset, Time.deltaTime * offsetLerpSpeed);

            // Rotación combinada (leve inclinación + rotación de vuelo)
            Quaternion targetRot = Quaternion.Euler(flyRotation.x + roll, flyRotation.y, flyRotation.z);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * rotationLerpSpeed);
        }
        else
        {
            // Restaurar posición y rotación original
            follow.FollowOffset = Vector3.Lerp(follow.FollowOffset, baseOffset, Time.deltaTime * restoreSpeed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, baseRotation, Time.deltaTime * restoreSpeed);
        }
    }

    public void SetFlying(bool value)
    {
        isFlying = value;
        
    }

    // ?? Método opcional para restaurar instantáneamente
    public void ResetCameraInstant()
    {
        if (follow != null)
            follow.FollowOffset = baseOffset;

        transform.localRotation = baseRotation;
        isFlying = false;
    }
}
