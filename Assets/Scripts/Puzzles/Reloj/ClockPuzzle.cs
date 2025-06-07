using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzle : MonoBehaviour
{
    [Header("Referencias")]
    public ClockHandleDragger hourHandle;
    public ClockHandleDragger minuteHandle;

    [Header("Horas Objetivo (HH, MM)")]
    public List<Vector2Int> targetTimes = new List<Vector2Int>()
    {
        new Vector2Int(3, 45),
        new Vector2Int(6, 15),
        new Vector2Int(9, 0)
    };

    [Header("Márgenes de Tolerancia")]
    public int allowedHourMargin = 1;     // 1 hora
    public int allowedMinuteMargin = 10;  // 10 minutos

    private int currentStep = 0;
    public SFXPlayer sfxPlayer;

    [Header("Cambio de interactuables")]
    public GameObject interactuableAntes;   // El objeto que se desactivará
    public GameObject interactuableDespues; // El objeto que se activará

    [Header("Interfaz de usuario")]
    public GameObject panelParaActivar; // Este panel será toggled (activado/desactivado)

    [Header("Puzzle Manager")]
    public PuzzleManager puzzleManager;

    void Update()
    {
        
    }

    public void CheckTargetTime()
    {
        if (currentStep >= targetTimes.Count)
        {
            Debug.Log("Puzzle ya completado.");
            return;
        }

        GetCurrentTime(out int currentHour, out int currentMinute);
        Vector2Int target = targetTimes[currentStep];

        bool hourOK = Mathf.Abs(Mathf.DeltaAngle(currentHour * 30f, target.x * 30f)) <= allowedHourMargin * 30f;
        bool minuteOK = Mathf.Abs(Mathf.DeltaAngle(currentMinute * 6f, target.y * 6f)) <= allowedMinuteMargin * 6f;

        Debug.Log($"Intento {currentStep + 1} | Hora actual: {currentHour:00}:{currentMinute:00} | Objetivo: {target.x:00}:{target.y:00}");

        if (hourOK && minuteOK)
        {
            Debug.Log("Hora correcta!");
            sfxPlayer?.PlayClick();
            currentStep++;

            if (currentStep >= targetTimes.Count)
            {
                Debug.Log("Puzzle completado correctamente.");

                puzzleManager.TogglePuzzlePanel(panelParaActivar);
                // Cambiar interactuables para dar la llave y activar la animacion del reloj abriendose
                if (interactuableAntes != null)
                    interactuableAntes.SetActive(false);

                if (interactuableDespues != null)
                    interactuableDespues.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Hora incorrecta. Reiniciando progreso.");
            currentStep = 0;
        }
    }

    private void GetCurrentTime(out int hour, out int minute)
    {
        float minuteAngle = GetAdjustedAngle(minuteHandle);
        float hourAngle = GetAdjustedAngle(hourHandle);

        minute = Mathf.RoundToInt(minuteAngle / 6f) % 60;
        float fullHour = (hourAngle / 30f + (minute / 60f)) % 12f;
        hour = Mathf.FloorToInt(fullHour);
    }

    private float GetAdjustedAngle(ClockHandleDragger handle)
    {
        Vector2 dir = (handle.transform.position - handle.centerPoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        // Inversión del sentido y compensación de ángulo inicial para que coincidan las manecillas con la hora marcada
        angle = (-angle - handle.initialAngleOffset + 360f) % 360f;
        return angle;
    }
}
