using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public Slider progressBar;

    private void OnEnable()
    {
        progressBar.value = 0;
    }

    public void LoadScene(string sceneName, ESCREEN afterScreen)
    {
        this.gameObject.SetActive(true);
        progressBar.value = 0;
        StartCoroutine(LoadSceneAsync(sceneName, afterScreen));
    }

    private IEnumerator LoadSceneAsync(string sceneName, ESCREEN afterScreen)
    {
        // Bắt đầu tải scene không đồng bộ
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = true;
        while (!asyncOperation.isDone) {
            // Cập nhật giá trị Slider
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressBar.value = progress;

            // Nếu tải hoàn tất, bật scene
            if (asyncOperation.progress >= 0.9f)
            {
                progressBar.value = 1f;
                asyncOperation.allowSceneActivation = true;
                //Bật Screen lên sau khi tải xong scene
                //GameManager.Instance.UIManager.ShowScreen(afterScreen);
            }
            yield return null;
        }
    }
}
