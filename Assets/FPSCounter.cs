using UnityEngine;
using TMPro; // Обязательно добавьте эту строку для работы с TextMeshPro

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText; // Ссылка на текстовый компонент UI

    // Переменные для усреднения FPS
    private float deltaTimeSum = 0.0f;
    private int frameCount = 0;
    private float updateInterval = 0.5f; // Обновлять FPS каждые 0.5 секунды

    void Start()
    {
        // Проверяем, назначен ли текстовый компонент
        if (fpsText == null)
        {
            Debug.LogError("FPSDisplay: Компонент TextMeshProUGUI не назначен! Пожалуйста, назначьте его в Инспекторе.");
            enabled = false; // Отключаем скрипт, если нет текстового компонента
            return;
        }

        // Можно настроить выравнивание текста, если нужно
        // fpsText.alignment = TextAlignmentOptions.TopLeft;
    }

    void Update()
    {
        // Добавляем время последнего кадра к сумме
        // Используем Time.unscaledDeltaTime, чтобы FPS не зависел от Time.timeScale
        deltaTimeSum += Time.unscaledDeltaTime;
        frameCount++;

        // Если прошло достаточно времени, обновляем FPS
        if (deltaTimeSum >= updateInterval)
        {
            // Вычисляем средний FPS за интервал
            float averageFps = frameCount / deltaTimeSum;

            // Форматируем строку и выводим на экран
            // Mathf.RoundToInt округляет до ближайшего целого числа
            fpsText.text = $"FPS: {Mathf.RoundToInt(averageFps)}";

            // Сбрасываем счетчики для следующего интервала
            deltaTimeSum = 0.0f;
            frameCount = 0;
        }
    }
}
