using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string XREssentialsScene = "XREssentials";
    public string menuScene = "MenuScene";
    public string previewScene = "PreviewScene";
    private void Awake() 
    {
        LoadSceneAdditive(XREssentialsScene);
        LoadSceneAdditive(menuScene);
    }

    public void UnloadScene(string sceneName)
    {
        if(SceneManager.GetSceneByName(sceneName).isLoaded)
            StartCoroutine(UnloadYourAsyncScene(sceneName));
    }

    public void LoadSceneAdditive(string sceneName)
    {
        if(!SceneManager.GetSceneByName(sceneName).isLoaded)
            StartCoroutine(LoadYourAsyncSceneAdditive(sceneName));
    }

    IEnumerator LoadYourAsyncSceneAdditive(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
      
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
        
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //Debug.Log(asyncLoad.progress);
            yield return null;
        }
    }

    IEnumerator UnloadYourAsyncScene(string sceneName)
    {
       
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            //Debug.Log(asyncLoad.progress);
            yield return null;
        }
    }
}
