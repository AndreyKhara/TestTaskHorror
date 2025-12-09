using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;


public interface IQuestObject
{
    UniTaskVoid Activate(GameObject item);
}
