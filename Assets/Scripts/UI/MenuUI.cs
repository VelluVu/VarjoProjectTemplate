using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    SceneLoader sceneLoader;
    [SerializeField]Button startButton;
    [SerializeField]Button settingsButton;
    [SerializeField]Button exitButton;
    [SerializeField]Button backButton;
    [SerializeField]Button saveButton;

    [SerializeField]GameObject mainMenu;
    [SerializeField]GameObject settingsMenu;
    
    private void Awake() 
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void Start() 
    {
       SetMenuListeners();
    }

    void SetMenuListeners()
    {
        startButton.onClick.AddListener(()=>sceneLoader.LoadSceneAdditive(sceneLoader.previewScene));
        startButton.onClick.AddListener(()=>sceneLoader.UnloadScene(sceneLoader.menuScene));

        settingsButton.onClick.AddListener(()=>settingsMenu.SetActive(true));
        settingsButton.onClick.AddListener(()=>mainMenu.SetActive(false));
        settingsButton.onClick.AddListener(()=>backButton.gameObject.SetActive(true));
        settingsButton.onClick.AddListener(()=>saveButton.gameObject.SetActive(true));

        saveButton.onClick.AddListener(()=>mainMenu.SetActive(true));
        saveButton.onClick.AddListener(()=>XRSettings.Instance.SaveSettings());
        saveButton.onClick.AddListener(()=>settingsMenu.SetActive(false));
        saveButton.onClick.AddListener(()=>backButton.gameObject.SetActive(false));
       
        backButton.onClick.AddListener(()=>mainMenu.SetActive(true));
        backButton.onClick.AddListener(()=>settingsMenu.SetActive(false));
        backButton.onClick.AddListener(()=>saveButton.gameObject.SetActive(false));
        backButton.onClick.AddListener(()=>backButton.gameObject.SetActive(false));

        exitButton.onClick.AddListener(()=>Application.Quit());
    }
}
