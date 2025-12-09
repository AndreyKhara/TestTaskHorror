using UnityEngine;
using DG.Tweening; // Обязательно добавьте это для использования DOTween

[RequireComponent(typeof(Light))] // Гарантирует, что на объекте есть компонент Light
public class BlinkingLamp : MonoBehaviour
{
    [Header("Intensity Settings")]
    public float minOnIntensity = 3f;  // Минимальная интенсивность, когда свет "включен"
    public float maxOnIntensity = 5f;  // Максимальная интенсивность, когда свет "включен"
    public float minOffIntensity = 0f; // Минимальная интенсивность, когда свет "выключен" (может быть 0 для полного выключения)
    public float maxOffIntensity = 0.5f; // Максимальная интенсивность, когда свет "выключен" (для легкого свечения)

    [Header("Duration Settings")]
    public float minOnDuration = 0.05f; // Минимальная длительность "включенного" состояния
    public float maxOnDuration = 0.15f; // Максимальная длительность "включенного" состояния
    public float minOffDuration = 0.05f; // Минимальная длительность "выключенного" состояния
    public float maxOffDuration = 0.15f; // Максимальная длительность "выключенного" состояния

    [Header("Flicker Control")]
    [Range(0.1f, 5f)]
    public float flickerSpeedMultiplier = 1f; // Множитель скорости мерцания (больше -> быстрее)

    private Light targetLight;
    private Sequence flickerSequence; // Ссылка на нашу последовательность

    void Awake()
    {
        targetLight = GetComponent<Light>();
    }

    void OnEnable()
    {
        StartFlickering();
    }

    void OnDisable()
    {
        StopFlickering();
    }

    void OnDestroy()
    {
        StopFlickering();
    }

    void StartFlickering()
    {
        // Останавливаем любую предыдущую последовательность, чтобы избежать дублирования
        StopFlickering();

        // Создаем новую последовательность
        flickerSequence = DOTween.Sequence();

        // --- Шаг 1: Случайное "включение" света ---
        float currentOnIntensity = Random.Range(minOnIntensity, maxOnIntensity);
        float currentOnDuration = Random.Range(minOnDuration, maxOnDuration) / flickerSpeedMultiplier;

        // Устанавливаем интенсивность мгновенно (для резкого мерцания)
        flickerSequence.AppendCallback(() => targetLight.intensity = currentOnIntensity);
        // Ждем случайное время, пока свет "включен"
        flickerSequence.AppendInterval(currentOnDuration);
          float currentOffIntensity = Random.Range(minOffIntensity, maxOffIntensity);
        float currentOffDuration = Random.Range(minOffDuration, maxOffDuration) / flickerSpeedMultiplier;

        // Устанавливаем интенсивность мгновенно (для резкого мерцания)
        flickerSequence.AppendCallback(() => targetLight.intensity = currentOffIntensity);
        // Ждем случайное время, пока свет "выключен"
        flickerSequence.AppendInterval(currentOffDuration);

        // --- Зацикливание ---
        // Когда эта последовательность из одного цикла завершится,
        // мы вызываем StartFlickering() снова, чтобы создать новую последовательность
        // с новыми случайными значениями. Это создает бесконечное, непредсказуемое мерцание.
        flickerSequence.OnComplete(StartFlickering);

        // Запускаем последовательность
        flickerSequence.Play();
    }

    void StopFlickering()
    {
        if (flickerSequence != null && flickerSequence.IsActive())
        {
            flickerSequence.Kill(); // Останавливаем и уничтожаем последовательность
            flickerSequence = null; // Обнуляем ссылку
        }
        // Убедимся, что свет выключен или установлен в дефолтное состояние после остановки
        if (targetLight != null)
        {
            targetLight.intensity = 0f;
        }
    }

}