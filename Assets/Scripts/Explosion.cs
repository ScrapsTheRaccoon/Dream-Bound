using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float destructionTime = 3f;

    private void Start()
    {
        Destroy(this.gameObject, destructionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControls player = other.gameObject.GetComponent<PlayerControls>();
            player.KnockBack(transform.position);
        }
    }
}
