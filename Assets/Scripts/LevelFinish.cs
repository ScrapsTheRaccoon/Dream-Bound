using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControls player = other.GetComponent<PlayerControls>();
        if (player == null) return;

        player.OnLevelFinished();

        GameManager.Instance.FinishLevel();
    }
}
