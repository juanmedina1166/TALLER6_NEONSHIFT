using UnityEngine;
using UnityEngine.UI; // Necesario para los botones

/// <summary>
/// Este script se encarga de mostrar un panel de confirmación
/// y, si se confirma, borrar todas las puntuaciones
/// llamando al LeaderboardManager.
/// </summary>
public class ClearLeaderboardButton : MonoBehaviour
{
    [Header("Referencias de UI")]
    [Tooltip("Arrastra aquí el panel que contiene la pregunta '¿Estás seguro?'")]
    public GameObject confirmationPanel;

    private void Start()
    {
        // Asegurarse de que el panel esté oculto al iniciar
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }
    }

    // --- MÉTODOS PÚBLICOS PARA LOS BOTONES ---

    /// <summary>
    /// 1. Asigna este método al botón principal "Borrar Puntuaciones".
    /// Muestra el panel de confirmación.
    /// </summary>
    public void ShowConfirmation()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("No se ha asignado el panel de confirmación. Borrando directamente (¡peligroso!)");
            // Si no quieres confirmación, puedes llamar a ConfirmClearData() directamente
            ConfirmClearData();
        }
    }

    /// <summary>
    /// 2. Asigna este método al botón "NO" o "Cancelar" del panel.
    /// Oculta el panel de confirmación.
    /// </summary>
    public void HideConfirmation()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 3. Asigna este método al botón "SÍ" o "Confirmar" del panel.
    /// Ejecuta la lógica de borrado.
    /// </summary>
    public void ConfirmClearData()
    {
        Debug.LogWarning("Confirmado: Borrando todas las tablas de puntuación...");

        // Validar que los managers existen
        if (LeaderboardManager.Instance == null)
        {
            Debug.LogError("LeaderboardManager no encontrado. No se pueden borrar los datos.");
            HideConfirmation(); // Ocultar panel aunque falle
            return;
        }

        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager no encontrado. No se puede determinar el total de niveles.");
            HideConfirmation(); // Ocultar panel aunque falle
            return;
        }

        // --- LÓGICA DE BORRADO ---

        // 1. Obtener el total de niveles desde tu SaveManager
        int totalLevels = SaveManager.Instance.totalLevelsInGame;

        // 2. Llamar al método que ya existe en tu LeaderboardManager
        LeaderboardManager.Instance.ClearAllLeaderboards(totalLevels);

        // 3. Ocultar el panel de confirmación
        HideConfirmation();

        // 4. (IMPORTANTE) Forzar la actualización de todas las tablas visibles
        UpdateAllVisibleDisplays();

        Debug.LogWarning("¡Tablas de puntuación borradas exitosamente!");
    }


    /// <summary>
    /// Busca todos los LeaderboardDisplay en la escena y los fuerza a
    /// actualizarse para que muestren los datos vacíos ("---").
    /// </summary>
    private void UpdateAllVisibleDisplays()
    {
        // Buscar todos los componentes LeaderboardDisplay en la escena
        LeaderboardDisplay[] allDisplays = FindObjectsOfType<LeaderboardDisplay>();

        foreach (LeaderboardDisplay display in allDisplays)
        {
            // Opcional: solo actualizar los que estén activos y visibles
            if (display.gameObject.activeInHierarchy)
            {
                display.UpdateDisplay();
            }
        }
    }
}