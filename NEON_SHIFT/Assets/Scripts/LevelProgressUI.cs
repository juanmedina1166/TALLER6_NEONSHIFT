using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class LevelProgressUI : MonoBehaviour
{
    public static LevelProgressUI Instance;

    [Header("Referencias UI")]
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public RectTransform playerIcon;
    public GameObject checkpointIconPrefab;
    public RectTransform checkpointContainer; // Contenedor vertical (misma altura que la barra)

    [Header("Parámetros del nivel")]
    public Transform player;
    public Transform startPoint;
    public Transform endPoint;

    [Header("Checkpoints del nivel (en orden de abajo hacia arriba)")]
    public Transform[] checkpoints;

    [Header("Colores")]
    public Color reachedColor = Color.white;
    public Color unreachedColor = new Color(1, 1, 1, 0.3f);

    private float totalDistance;
    private List<Image> checkpointIcons = new List<Image>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        if (startPoint != null && endPoint != null)
            totalDistance = Vector3.Distance(startPoint.position, endPoint.position);

        StartCoroutine(DelayedCreateCheckpointIcons());
        StartCoroutine(InitializePlayerIcon());
    }

    void Update()
    {
        if (player == null || totalDistance <= 0f || progressBar == null)
            return;

        float distanceCovered = Vector3.Distance(startPoint.position, player.position);
        float progress = Mathf.Clamp01(distanceCovered / totalDistance);

        progressBar.value = progress;
        if (progressText != null)
            progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";

        // --- ?? Movimiento vertical del icono del jugador corregido ---
        if (playerIcon != null && checkpointContainer != null)
        {
            float containerHeight = checkpointContainer.rect.height;
            float pivotOffset = (0.5f - checkpointContainer.pivot.y) * containerHeight;

            // Desde abajo (0) hasta arriba (1)
            Vector2 iconPos = playerIcon.anchoredPosition;
            iconPos.y = (progress * containerHeight) - (containerHeight * 0.5f) + pivotOffset;
            playerIcon.anchoredPosition = iconPos;
        }
    }
    private IEnumerator InitializePlayerIcon()
    {
        yield return null; // espera un frame
        UpdatePlayerIconPosition(0f); // lo pone al inicio
    }
    private IEnumerator DelayedCreateCheckpointIcons()
    {
        yield return new WaitForEndOfFrame(); // esperar un frame para layout
        CreateCheckpointIcons();
    }

    private void CreateCheckpointIcons()
    {
        if (checkpointIconPrefab == null || checkpointContainer == null || checkpoints == null || checkpoints.Length == 0)
            return;

        foreach (Transform child in checkpointContainer)
            Destroy(child.gameObject);

        checkpointIcons.Clear();

        if (startPoint == null || endPoint == null)
            return;

        float containerHeight = checkpointContainer.rect.height;
        float pivotOffset = (0.5f - checkpointContainer.pivot.y) * containerHeight;
        float levelLength = Vector3.Distance(startPoint.position, endPoint.position);

        for (int i = 0; i < checkpoints.Length; i++)
        {
            Transform checkpoint = checkpoints[i];
            GameObject iconObj = Instantiate(checkpointIconPrefab, checkpointContainer);
            Image iconImg = iconObj.GetComponent<Image>();

            if (iconImg != null)
                iconImg.color = unreachedColor;

            float distFromStart = Vector3.Distance(startPoint.position, checkpoint.position);
            float normalizedPos = Mathf.Clamp01(distFromStart / levelLength);

            RectTransform iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0.5f, 0.5f);
            iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.pivot = new Vector2(0.5f, 0.5f);

            iconRect.anchoredPosition = new Vector2(
                0,
                (normalizedPos * containerHeight) - (containerHeight * 0.5f) + pivotOffset
            );

            checkpointIcons.Add(iconImg);
        }
    }

    public void UpdateCheckpointIcon(int index)
    {
        if (index < 0 || index >= checkpointIcons.Count) return;
        if (checkpointIcons[index] != null)
            checkpointIcons[index].color = reachedColor;
    }
    private void UpdatePlayerIconPosition(float progress)
    {
        if (playerIcon == null || checkpointContainer == null) return;

        float height = checkpointContainer.rect.height;
        Vector2 pos = playerIcon.anchoredPosition;
        pos.y = (progress * height) - (height * 0.5f);
        playerIcon.anchoredPosition = pos;
    }

    public void ResetAllCheckpointIcons()
    {
        foreach (var icon in checkpointIcons)
        {
            if (icon != null)
                icon.color = unreachedColor;
        }
    }
}
