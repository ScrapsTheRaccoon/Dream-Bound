using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject timeUpText;
    [SerializeField] private GameObject levelClearText;
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private GameObject stripe;
    [SerializeField] private Animator pauseMenuAnim;

    void Start()
    {
        fadeOut.SetActive(false);
        FadeIn();

        timeUpText.SetActive(false);
        levelClearText.SetActive(false);
        stripe.SetActive(false);

        pauseMenuAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void OnEnable()
    {
        StartCoroutine(Subscribe());
    }

    private IEnumerator Subscribe()
    {
        while (GameManager.Instance == null)
            yield return null;

        GameManager.Instance.OnGameStateChanged += HandleGameState;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged -= HandleGameState;
    }

    private void HandleGameState(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.LevelFinished:
                LevelClear();
                break;

            case GameManager.GameState.TimeUp:
                TimeUp();
                break;

            case GameManager.GameState.FellOff:
                FellOff();
                break;

            case GameManager.GameState.Paused:
                pauseMenuAnim.SetTrigger("OpenPanel");
                break;

            case GameManager.GameState.Playing:
                pauseMenuAnim.SetTrigger("ClosePanel");
                break;
        }
    }

    public void TimeUp()
    {
        fadeOut.SetActive(true);
        timeUpText.SetActive(true);
    }

    public void FellOff()
    {
        fadeOut.SetActive(true);
    }

    public void LevelClear()
    {
        StartCoroutine(LevelClearSequence());
    }

    private IEnumerator LevelClearSequence()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);

        levelClearText.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);

        stripe.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuAnim.SetTrigger("ClosePanel");
        GameManager.Instance.Resume();
    }

    public void Restart()
    {
        LevelManager.Instance.ReloadLevel();
        GameManager.Instance.Resume();
    }

    public void ReturnToMainMenu()
    {
        LevelManager.Instance.LoadMainMenu();
        GameManager.Instance.Resume();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        fadeIn.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        fadeIn.SetActive(false);
    }
}
