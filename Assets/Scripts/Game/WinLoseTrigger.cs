using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseTrigger : MonoBehaviour
{
    public string LoadScene;
   private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger");
            SceneManager.LoadScene(LoadScene);
        }
    }
}
