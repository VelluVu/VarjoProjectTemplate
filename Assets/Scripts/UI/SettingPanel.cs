using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class presents the setting panel.
/// </summary>
public class SettingPanel : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Button changeLeftButton;
    public Button changeRightButton;

    private void Awake()
    {
        changeLeftButton = transform.GetChild(0).GetComponent<Button>();
        changeRightButton = transform.GetChild(1).GetComponent<Button>();
        textMesh = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Changes the text for textmesh component.
    /// </summary>
    /// <param name="text">new text string</param>
    public void SetText(string text)
    {
        textMesh.text = text;
    }

}
