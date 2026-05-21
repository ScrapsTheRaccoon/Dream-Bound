using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private GameObject timeBox;
    private LevelManager levelManager;
    private int maxTime;

    private float timer;
    private UIManager uiManager;

    private void Awake()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    private void Start()
    {
        maxTime = levelManager.MaxTime;
        timer = maxTime;
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timer = Mathf.Max(timer, 0f); // prevent negatives

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timeBox.GetComponent<TMPro.TMP_Text>().text = $"{minutes:00}:{seconds:00}";
        

        if (timer <= 0)
        {
            AudioManager.Instance.PlayTimeUp();
            GameManager.Instance.TimeUp();
            uiManager.TimeUp();
            enabled = false;
        }
    }
}
