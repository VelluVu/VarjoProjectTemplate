using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void SetText(string text)
    {
        textMesh.text = text;
    }

}
