using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
