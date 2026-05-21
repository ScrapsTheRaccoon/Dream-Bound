using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Scene References")]
    [SerializeField] private AudioClip levelMusicClip;

    [Header("Level Flow")]
    [SerializeField] private int maxTime = 30;
    public int MaxTime => maxTime;
    [SerializeField] private float transitionDelay = 2f;
    [SerializeField] private int nextSceneIndex = -1;
    [SerializeField] private int finalLevelIndex = 4;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(levelMusicClip, fadeIn: true);
        }

    }

    private void OnEnable()
    {
        StartCoroutine(SubscribeToGameManager());
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameState;
        }
    }

    private IEnumerator SubscribeToGameManager()
    {
        while (GameManager.Instance == null)
            yield return null; // wait until it exists

        GameManager.Instance.OnGameStateChanged += HandleGameState;
    }

    private void HandleGameState(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.LevelFinished:
                StartCoroutine(LevelFinishedSequence());
                break;

            case GameManager.GameState.TimeUp:
            case GameManager.GameState.FellOff:
                StartCoroutine(LevelFailedSequence());
                break;
        }
    }

    private IEnumerator LevelFinishedSequence()
    {
        AudioManager.Instance.FadeOutMusic();
        yield return new WaitForSecondsRealtime(2f);

        AudioManager.Instance.PlayLevelFinish();
        yield return new WaitForSecondsRealtime(AudioManager.Instance.LevelFinishLength);

        LoadNextLevel();
    }

    private IEnumerator LevelFailedSequence()
    {
        yield return new WaitForSecondsRealtime(2f);
        ReloadLevel();
    }

    private void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex >= finalLevelIndex)
        {
            StartCoroutine(FadeAndLoadScene("MainMenu"));
            return;
        }

        if (nextSceneIndex >= 0)
        {
            StartCoroutine(FadeAndLoadScene(nextSceneIndex));
            return;
        }

        StartCoroutine(FadeAndLoadScene(currentIndex + 1));
    }


    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.Resume();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator FadeAndLoadScene(object scene)
    {
        AudioManager.Instance.FadeOutMusic();
        yield return new WaitForSecondsRealtime(transitionDelay); // fadeOutDuration

        if (scene is string sceneName)
            SceneManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene((int)scene);

        GameManager.Instance.Resume();
    }


}
