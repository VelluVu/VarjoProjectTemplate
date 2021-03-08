using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour
{
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;

    SceneLoader SceneLoader;

    private void Awake() {
        SceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Start() {
        exitButton.onClick.AddListener(()=>SceneLoader.LoadSceneAdditive(SceneLoader.menuScene));
        exitButton.onClick.AddListener(()=>SceneLoader.UnloadScene(SceneLoader.previewScene));
    }
}
