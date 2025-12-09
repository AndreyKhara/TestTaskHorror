using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    
    private Transform _trf;
    private float targetOpenAngle = 90f;

    [Tooltip("Длительность анимации открытия.")]
    public float animationDuration = 1f;

    

    void Awake()
    {
        // Если doorToRotate не назначен в инспекторе, используем сам объект, к которому прикреплен скрипт

        _trf = transform;
        
    }
    public void Open()
    {
        _trf.DOKill(true); 

        _trf.DOLocalRotate(
            new Vector3(_trf.localEulerAngles.x, targetOpenAngle, _trf.localEulerAngles.z),
            animationDuration
        ).SetEase(Ease.OutQuad);
    }
    void OnDestroy()
    {
        if (_trf != null) // Проверяем, что объект еще существует
        {
            _trf.DOKill(true);
        }
    }
}
