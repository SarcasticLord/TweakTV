using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PreStreamScript : MonoBehaviour
{
    [SerializeField] private string selectScene = "SelectScene";

    private UIDocument _document;

    private Button _back;
    private Button _start;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _back = _document.rootVisualElement.Q("BackButton") as Button;
        _back.RegisterCallback<ClickEvent>(OnBackClick);

    }

    private void OnDisable()
    {
        _back.UnregisterCallback<ClickEvent>(OnBackClick);
    }

    private void OnBackClick(ClickEvent evt)
    {
        Debug.Log("Returning to title");
        SceneManager.LoadScene(selectScene);
    }
}
