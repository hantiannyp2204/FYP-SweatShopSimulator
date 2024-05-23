using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionmanager : MonoBehaviour
{
    public FadeScreen fadeScreen;

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex));
        
    }
    
    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        if(fadeScreen.gameObject.activeSelf == false)
        {
            fadeScreen.gameObject.SetActive(true);
        }
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        //Launch New Scene
        SceneManager.LoadScene(sceneIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GoToScene(1);
        }
    }

    public void GoToSceneAsy(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutineAsy(sceneIndex));
    }

    IEnumerator GoToSceneRoutineAsy(int sceneIndex)
    {
        if (fadeScreen.gameObject.activeSelf == false)
        {
            fadeScreen.gameObject.SetActive(true);
        }
        fadeScreen.FadeOut();

        //Launch New Scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
        while (timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
