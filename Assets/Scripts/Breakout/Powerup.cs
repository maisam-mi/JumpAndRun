using UnityEngine;

public class Powerup : MonoBehaviour
{
    #region Private Fields

    private Player player;

    #endregion

    private void Start()
    {
        this.player = FindFirstObjectByType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
