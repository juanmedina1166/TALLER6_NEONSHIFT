using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CheckpointUI : MonoBehaviour
{
    public static CheckpointUI Instance; 

    [Header("UI Elements")]
    [SerializeField] private Image checkpointIcon;
    [SerializeField] private TextMeshProUGUI checkpointText;

    private void Awake()
    {
        Instance = this;
        if (checkpointIcon != null)
            checkpointIcon.gameObject.SetActive(false);
    }

    public void ShowCheckpointIcon(float duration = 2f)
    {
        if (checkpointIcon && checkpointText == null) return;
        StartCoroutine(ShowIconRoutine(duration));
        
    }

    private IEnumerator ShowIconRoutine(float time)
    {
        checkpointIcon.gameObject.SetActive(true);
        checkpointText.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        checkpointIcon.gameObject.SetActive(false);
        checkpointText.gameObject.SetActive(false);

    }
}
