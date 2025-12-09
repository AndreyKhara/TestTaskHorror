using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

public class CupOfSticks : MonoBehaviour, IQuestObject
{
    [SerializeField] private Transform _coffeePlace;
   public async UniTaskVoid Activate(GameObject item)
    {
        Coffee coffee = item.GetComponent<Coffee>();
        if (coffee != null && coffee.stateCoffe == Coffee.State.WithCoffee)
            {
                coffee.UpdateState();
            item.transform.position = _coffeePlace.position;
            }
    }
}
