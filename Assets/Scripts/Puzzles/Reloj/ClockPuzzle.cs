using UnityEngine;

public class ClockPuzzle : MonoBehaviour
{
    [Header("Referencias")]
    public ClockHandleDragger hourHandle;
    public ClockHandleDragger minuteHandle;

    [Header("Hora Objetivo")]
    [Range(0, 11)] public int targetHour = 3;
    [Range(0, 59)] public int targetMinute = 45;

    [Header("Márgenes de Tolerancia")]
    public int allowedHourMargin = 1;     // 1 hora
    public int allowedMinuteMargin = 10;  // 10 minutos

    void Update()
    {
        CheckTargetTime();
    }

    public void CheckTargetTime()
    {
        GetCurrentTime(out int currentHour, out int currentMinute);

        bool hourOK = Mathf.Abs(Mathf.DeltaAngle(currentHour * 30f, targetHour * 30f)) <= allowedHourMargin * 30f;
        bool minuteOK = Mathf.Abs(Mathf.DeltaAngle(currentMinute * 6f, targetMinute * 6f)) <= allowedMinuteMargin * 6f;

        Debug.Log($"Hora actual: {currentHour:00}:{currentMinute:00} | Objetivo: {targetHour:00}:{targetMinute:00}");

        if (hourOK && minuteOK)
        {
            Debug.Log("¡Hora correcta marcada!");
            // Falta implementar la logica de resolucion de este puzle
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
