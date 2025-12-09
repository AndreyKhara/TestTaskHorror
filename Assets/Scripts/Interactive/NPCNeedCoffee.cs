using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;


public class NPCNeedCoffee : MonoBehaviour, IQuestObject
{
    [SerializeField] private GameObject _monster;
    [SerializeField] private Door _door;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClipsToPlay;
    public async UniTaskVoid Activate(GameObject item)
    {
        Coffee coffee = item.GetComponent<Coffee>();
        if (coffee != null)
        {
            if (coffee.stateCoffe == Coffee.State.WithStick)
            {
                Destroy(item);
                await PlayAudioSequence();
                _door.Open();
                Instantiate(_monster, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

    }
    

    private async UniTask PlayAudioSequence()
    {
        foreach (AudioClip clip in _audioClipsToPlay)
        {
            _audioSource.clip = clip;
            _audioSource.Play();

            await UniTask.WaitUntil(() => !_audioSource.isPlaying, PlayerLoopTiming.Update);
        }
    }
}
