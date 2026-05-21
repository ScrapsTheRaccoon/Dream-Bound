using UnityEngine;

public class CameraFlipTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CameraController cam = other.GetComponentInChildren<CameraController>();

        if (cam != null)
        {
            cam.flipCamera();
        }
    }
}
