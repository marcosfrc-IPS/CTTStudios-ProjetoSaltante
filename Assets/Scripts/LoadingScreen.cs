using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel, mainPanel;
    [SerializeField] Slider progressBar;

    public void LoadGame(string sceneName) => StartCoroutine(Load(sceneName));

    IEnumerator Load(string sceneName)
    {
        mainPanel.SetActive(false);
        loadingPanel.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            progressBar.value = Mathf.Clamp01(op.progress / 0.9f); // progress stops at 0.9
            yield return null;
        }
    }
}