using UnityEngine;

public class BombLandingIndicator : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float pulseSpeed;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // pulse slowly
    }
}
