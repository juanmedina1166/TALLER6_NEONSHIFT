using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckpointUI : MonoBehaviour
{
    public static CheckpointUI Instance; // Singleton simple

    [Header("UI Elements")]
    [SerializeField] private Image checkpointIcon;

    private void Awake()
    {
        Instance = this;
        if (checkpointIcon != null)
            checkpointIcon.gameObject.SetActive(false);
    }

    public void ShowCheckpointIcon(float duration = 2f)
    {
        if (checkpointIcon == null) return;
        StartCoroutine(ShowIconRoutine(duration));
    }

    private IEnumerator ShowIconRoutine(float time)
    {
        checkpointIcon.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        checkpointIcon.gameObject.SetActive(false);
    }
}
