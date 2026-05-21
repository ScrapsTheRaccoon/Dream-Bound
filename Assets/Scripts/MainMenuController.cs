using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private AudioClip levelMusicClip;
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private Animator quitPanelAnim;
    [SerializeField] private Animator LevelCompletePanelAnim;

    private Animator currentOpenMenu = null;

    private void Start()
    {
        StartCoroutine(FadeIn());

        if (GameManager.showCompletionPopup)
        {
            OpenPanel(LevelCompletePanelAnim);
            GameManager.showCompletionPopup = false;
        }

        AudioManager.Instance.PlayMusic(levelMusicClip, true);
    }

    public void StartGame()
    {
        StartCoroutine(LoadGame());
    }

    public void StartCreditsScene()
    {
        StartCoroutine(loadCredits());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OpenPanel(Animator menuAnim)
    {
        if (currentOpenMenu != null) return;

        menuAnim.SetTrigger("OpenPanel");
        currentOpenMenu = menuAnim;
    }

    public void CloseCurrentPanel()
    {
        if (currentOpenMenu == null) return;

        currentOpenMenu.SetTrigger("ClosePanel");
        currentOpenMenu = null;
    }

    private IEnumerator LoadGame()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Level01");
    }

    private IEnumerator loadCredits()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Credits");
    }

    IEnumerator FadeIn()
    {
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(2f);
        fadeIn.SetActive(false);
    }

    public void OpenExitConfirmationPanel() => OpenPanel(quitPanelAnim);
}
