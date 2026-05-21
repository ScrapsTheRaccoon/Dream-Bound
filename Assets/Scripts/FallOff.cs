using UnityEngine;

public class FallOff : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerControls player = other.GetComponent<PlayerControls>();

        if (player == null) return;

        player.OnFallOff();
        AudioManager.Instance.PlayFallOff();
        GameManager.Instance.FallDeath();
    }
}
