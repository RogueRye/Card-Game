using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNav : MonoBehaviour {

    [SerializeField]
    GameObject loadingScreen;
    [SerializeField]
    Slider slider;
    [SerializeField]
    TextMeshProUGUI progressText;


    public void ChangeScene(int index)
    {
        if (loadingScreen != null)
            StartCoroutine(LoadAsynchronously(index));
        else
            SceneManager.LoadScene(index);
    }

    public void Exit()
    {
        Application.Quit();
    } 


    IEnumerator LoadAsynchronously(int index)
    {


        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

 
            loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                slider.value = progress;

                if (progressText != null)
                    progressText.text = Mathf.RoundToInt(progress * 100) + "%";

                yield return null;
            }
 
    }

}
