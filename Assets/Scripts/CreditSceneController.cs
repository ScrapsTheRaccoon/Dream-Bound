using UnityEngine;
using System.Collections;

public class CreditSceneController : MonoBehaviour
{
    [SerializeField] private float creditsLength = 45f;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private GameObject fadeOut;

    private void Start()
    {
        fadeOut.SetActive(false);

        StartCoroutine(CreditsRoutine());
    }

    private IEnumerator CreditsRoutine()
    {
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(2f);

        credits.SetActive(true);
        yield return new WaitForSeconds(creditsLength);

        fadeOut.SetActive(true);
        LevelManager.Instance.LoadMainMenu();
    }
}
