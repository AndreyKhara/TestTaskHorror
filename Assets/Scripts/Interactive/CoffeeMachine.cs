using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;


public class CoffeeMachine : MonoBehaviour, IQuestObject
{
    [SerializeField] private Transform _coffePoint;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private List<AudioClip> _audioClipsToPlay;
    private bool _isActive = false;
    public async UniTaskVoid Activate(GameObject item)
    {
        if (!_isActive)
        {
            Coffee coffee = item.GetComponent<Coffee>();
            if (coffee != null && coffee.stateCoffe == Coffee.State.Empty)
            {
                _isActive = true;
                item.transform.position = _coffePoint.position;
                item.transform.parent = _coffePoint;
                item.GetComponent<BoxCollider>().enabled = false;
                await PlayAudioSequence();
                item.GetComponent<BoxCollider>().enabled = true;
                coffee.UpdateState();
                _isActive = false;
            }
        }
        else
        {
            return;
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
