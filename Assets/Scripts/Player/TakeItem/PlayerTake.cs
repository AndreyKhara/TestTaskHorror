using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerTake : MonoBehaviour
{
    [SerializeField] private Transform itemPlace;
    private GameObject _currentGameObject;
    private bool pickup;
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _takeItem;

    private void Awake()
    {
        _takeItem = _playerInput.actions["TakeItem"];
        _takeItem.performed += TakeItem;
    }

    private void TakeItem(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (pickup)
        {
            if (Physics.Raycast(ray, out hit, 1.3f))
            {
                if (hit.collider.tag == "QuestPoint")
                {
                    hit.collider.GetComponent<IQuestObject>().Activate(_currentGameObject);
                    pickup = false;
                }
            }

            itemPlace.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
            itemPlace.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
            itemPlace.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            itemPlace.GetChild(0).parent = null;
            _currentGameObject = null;
            pickup = false;
            return;
        }
        else
        {
            
            if (Physics.Raycast(ray, out hit, 1.3f))
            {
                if(hit.collider.tag == "Item")
                {
				    if (pickup == false)
                    {
                        _currentGameObject = hit.collider.gameObject;
					    hit.collider.transform.parent = itemPlace;
					    hit.collider.GetComponent<Rigidbody>().isKinematic = true;
					    hit.collider.GetComponent<Rigidbody>().useGravity = false;
					    hit.collider.GetComponent<BoxCollider>().enabled = false;
					    hit.collider.transform.position = itemPlace.position;
					    hit.collider.transform.rotation = itemPlace.rotation;
					    pickup = true;
				    }
                }
		    }
        }
    }
}
