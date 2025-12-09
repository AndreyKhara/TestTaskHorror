using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

public class Monster : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 7f;
    public float rotateSpeed = 720f;
    public float stopDistance = 1.5f;
    private Transform _selfTrf;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _idleClip;
    [SerializeField] private AudioClip _runnngClip;
    [SerializeField] private AudioClip _eventClip;
    private bool _isRunning = false;

    Rigidbody rb;

    private async UniTaskVoid Start()
    {
        GameObject player_gbj = GameObject.FindGameObjectWithTag("Player");
        player = player_gbj.GetComponent<Transform>();
        
        rb = GetComponent<Rigidbody>();
        _selfTrf = GetComponent<Transform>();

        _audioSource.clip = _idleClip;
        _audioSource.Play();
        await UniTask.WaitUntil(() => !_audioSource.isPlaying, PlayerLoopTiming.Update);
        _audioSource.clip = _runnngClip;
        _audioSource.Play();
        _audioSource.loop = true;
        AudioSource playerAudio = player.GetComponent<AudioSource>();
        playerAudio.clip = _eventClip;
        playerAudio.Play();


        _isRunning = true;

    }

    void FixedUpdate()
    {
        if (_isRunning)
        {
            Vector3 dir = player.position - _selfTrf.position;
            dir.y = 0f;
            float dist = dir.magnitude;

            if (dist > stopDistance)
            {
                // поворот
                if (dir.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
                    _selfTrf.rotation = Quaternion.RotateTowards(_selfTrf.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime);
                }

                // движение через Rigidbody для корректных столкновений
                Vector3 newPos = rb.position + _selfTrf.forward * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(newPos);

            }
        }
    }



}
