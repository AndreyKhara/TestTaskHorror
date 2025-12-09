using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : MonoBehaviour
{
    [SerializeField] private GameObject _water;
    [SerializeField] private GameObject _stick;
    public enum State
    {
        Empty,
        WithCoffee,
        WithStick
    }

    public State stateCoffe = State.Empty;

    public void UpdateState()
    {
        switch (stateCoffe){
            case State.Empty:
                stateCoffe = State.WithCoffee;
                _water.SetActive(true);
                break;
            case State.WithCoffee:
                stateCoffe = State.WithStick;
                _stick.SetActive(true);
                break;
        }
    }

}
