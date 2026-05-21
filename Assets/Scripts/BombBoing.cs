using UnityEngine;

public class BombBoing : MonoBehaviour
{
    [SerializeField] private float minImpactVelocity = 2f;

    private Rigidbody rb;
    private bool landed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
            return;

        if (!landed && collision.relativeVelocity.magnitude >= minImpactVelocity)
        {
            AudioManager.Instance.PlayBoing();
        }

        if (!landed)
        {
            landed = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.constraints = RigidbodyConstraints.FreezePosition |
                             RigidbodyConstraints.FreezeRotation;
        }
    }
}
