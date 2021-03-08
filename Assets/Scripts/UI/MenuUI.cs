using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    SceneLoader sceneLoader;
    [SerializeField]Button startButton;
    [SerializeField]Button settingsButton;
    [SerializeField]Button exitButton;
    
    private void Awake() {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Start() {
        startButton.onClick.AddListener(()=>sceneLoader.LoadSceneAdditive(sceneLoader.previewScene));
        startButton.onClick.AddListener(()=>sceneLoader.UnloadScene(sceneLoader.menuScene));
        //TODO: Settings button add listener open settings menu...
        exitButton.onClick.AddListener(()=>Application.Quit());
    }
}
