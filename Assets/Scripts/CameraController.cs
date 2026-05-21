using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Default Settings")]
    [SerializeField] private Vector3 defaultOffset;
    [SerializeField] private Quaternion defaultRotation;

    [Header("Flipped Settings")]
    [SerializeField] private Vector3 flippedOffset;
    [SerializeField] private Quaternion flippedRotation;

    [Header("Speed")]
    [SerializeField] private float flipSpeed = 5f;

    private bool isFlipped = false;
    private Coroutine flipCoroutine;

    private Vector3 targetOffset;
    private Quaternion targetRotation;

    void Start()
    {
        targetOffset = defaultOffset;
        targetRotation = defaultRotation;

        transform.position = player.position + player.rotation * targetOffset;
        transform.rotation = player.rotation * targetRotation;
    }

    public void flipCamera()
    {
        if (flipCoroutine != null)
            StopCoroutine(flipCoroutine);

        targetOffset = isFlipped ? defaultOffset : flippedOffset;
        targetRotation = isFlipped ? defaultRotation : flippedRotation;

        flipCoroutine = StartCoroutine(FlipRoutine());
        isFlipped = !isFlipped;
    }

    private IEnumerator FlipRoutine()
    {
        while (
            Vector3.Distance(transform.position, player.position + player.rotation * targetOffset) > 0.01f ||
            Quaternion.Angle(transform.rotation, player.rotation * targetRotation) > 0.1f
        )
        {
            transform.position = Vector3.Lerp(
                transform.position,
                player.position + player.rotation * targetOffset,
                flipSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                player.rotation * targetRotation,
                flipSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = player.position + player.rotation * targetOffset;
        transform.rotation = player.rotation * targetRotation;
    }

    private void LateUpdate()
    {
        // Follow player AFTER they move/rotate this frame
        transform.position = Vector3.Lerp(
            transform.position,
            player.position + player.rotation * targetOffset,
            flipSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            player.rotation * targetRotation,
            flipSpeed * Time.deltaTime
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            flipCamera();
    }
}
