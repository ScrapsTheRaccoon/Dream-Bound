using UnityEngine;

public class GemControl : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private int scoreValue = 100;

    private ScoreControl scoreControl;

    private void Awake()
    {
        scoreControl = GameObject.Find("Canvas").GetComponent<ScoreControl>();
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlayGem();
        Destroy(this.gameObject);
        scoreControl.AddScore(scoreValue);
    }
}
