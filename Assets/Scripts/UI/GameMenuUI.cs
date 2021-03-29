using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Veli-Matti Vuoti
/// This class handles the logic of the game menu ui-elements.
/// </summary>
public class GameMenuUI : MonoBehaviour
{
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;
    
    [SerializeField]Button backButton;
    [SerializeField]Button saveButton;

    [SerializeField]GameObject menu;
    [SerializeField]GameObject settingsMenu;

    SceneLoader SceneLoader;

    private void Awake() {
        SceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Start() {
        exitButton.onClick.AddListener(()=>SceneLoader.LoadSceneAdditive(SceneLoader.menuScene));
        exitButton.onClick.AddListener(()=>SceneLoader.UnloadScene(SceneLoader.previewScene));

        settingsButton.onClick.AddListener(()=>settingsMenu.SetActive(true));
        settingsButton.onClick.AddListener(()=>menu.SetActive(false));
        settingsButton.onClick.AddListener(()=>backButton.gameObject.SetActive(true));
        settingsButton.onClick.AddListener(()=>saveButton.gameObject.SetActive(true));

        saveButton.onClick.AddListener(()=>menu.SetActive(true));
        saveButton.onClick.AddListener(()=>XRSettings.Instance.SaveSettings());
        saveButton.onClick.AddListener(()=>settingsMenu.SetActive(false));
        saveButton.onClick.AddListener(()=>backButton.gameObject.SetActive(false));
        
        backButton.onClick.AddListener(()=>menu.SetActive(true));
        backButton.onClick.AddListener(()=>settingsMenu.SetActive(false));
        backButton.onClick.AddListener(()=>saveButton.gameObject.SetActive(false));
        backButton.onClick.AddListener(()=>backButton.gameObject.SetActive(false));

    }
}
