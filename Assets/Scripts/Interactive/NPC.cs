using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class NPC : MonoBehaviour
{
    [Header("Настройки маршрута")]
    public Transform[] waypoints;
    public float speed = 3.0f; 
    public float rotationSpeed = 5.0f;
    public float stopDistance = 0.5f;
    public NPCNeedCoffee _quest;
    private Animator _animator;


    private int currentWaypointIndex = 0;
    private bool finishedAllWaypoints = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        LookAtTarget(waypoints[currentWaypointIndex].position);
        _quest.enabled = false;
    }

    void Update()
    {
        if (finishedAllWaypoints)
        {
            return;
        }
        // Получаем текущую целевую точку
        Transform currentTarget = waypoints[currentWaypointIndex];

        // Вычисляем направление к цели
        Vector3 direction = currentTarget.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= stopDistance)
        {
            GoToNextWaypoint(); 
        }
        else
        {
            // Двигаемся к цели
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            if (direction.sqrMagnitude > 0.001f)
            {
                LookAtTarget(currentTarget.position);
            }
        }
    }

    void GoToNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
        {
            finishedAllWaypoints = true;
            NeedCoffe();
            return;
            }
    }

    void LookAtTarget(Vector3 targetPos)
    {
        Vector3 lookDirection = targetPos - transform.position;
        lookDirection.y = 0;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void NeedCoffe()
    {
        _animator.SetBool("Stop", true);
        Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
        transform.rotation = targetRotation;
        _quest.enabled = true;
    }
}
