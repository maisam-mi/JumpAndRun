using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject door;

    private void Update()
    {
        if (GameManager.Instance.IsDoorOpened)
        {
            door.SetActive(false);
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            door.SetActive(true);
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
