using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject dropLocation;
    [SerializeField] private GameObject landingIndicatorPrefab;
    [SerializeField] private float spawnHeight = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Vector3 groundPoint = dropLocation.transform.position;
        groundPoint.y = transform.position.y;

        // indicator
        Instantiate(landingIndicatorPrefab, groundPoint, Quaternion.identity);

        // bomb above
        Vector3 spawnPos = groundPoint + Vector3.up * spawnHeight;
        Instantiate(bombPrefab, spawnPos, Quaternion.identity);

        Destroy(gameObject);
    }
}
