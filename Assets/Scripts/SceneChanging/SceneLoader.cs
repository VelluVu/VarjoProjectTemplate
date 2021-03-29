using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class is used to handle the scene loading and unloading in correct order.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Header("Scenes to load by name")]

    [Tooltip("Scene which contains VR Essential classes and objects, and will be loaded first and never unloaded.")]
    public string XREssentialsScene = "XREssentials";
    [Tooltip("Scene which contains the main menu, and will be loaded after VR Essentials Scene.")]
    public string menuScene = "MenuScene";
    [Tooltip("The Preview Scene of the functionality which this project offers. Loaded on click start button in main menu, and unloaded when exit on click in game menu.")]
    public string previewScene = "PreviewScene";

    private void Awake() 
    {
        LoadSceneAdditive(XREssentialsScene);
        LoadSceneAdditive(menuScene);
    }

    /// <summary>
    /// This Function starts coroutine to unload the scene by name.
    /// </summary>
    /// <param name="sceneName">the scene name you wish to unload.</param>
    public void UnloadScene(string sceneName)
    {
        if(SceneManager.GetSceneByName(sceneName).isLoaded)
            StartCoroutine(UnloadYourAsyncScene(sceneName));
    }

    /// <summary>
    /// This function starts coroutine to load the scene by name and additive.
    /// </summary>
    /// <param name="sceneName">the scene name you wish to load.</param>
    public void LoadSceneAdditive(string sceneName)
    {
        if(!SceneManager.GetSceneByName(sceneName).isLoaded)
            StartCoroutine(LoadYourAsyncSceneAdditive(sceneName));
    }

    /// <summary>
    /// This will do the async loading for scene.
    /// </summary>
    /// <param name="sceneName">scene name to load</param>
    /// <returns>keeps going until async operation is complete</returns>
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

    /// <summary>
    /// This will do the async unloading for scene.
    /// </summary>
    /// <param name="sceneName">scene name to unload</param>
    /// <returns></returns>
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
