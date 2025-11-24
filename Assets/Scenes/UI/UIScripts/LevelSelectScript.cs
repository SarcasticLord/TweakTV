using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectScript : MonoBehaviour
{
    public GameController controller;

    public GameObject AsylumSelectedIcon;
    public GameObject SubwaySelectedIcon;
    public GameObject TweakHQSelectedIcon;


    [SerializeField] private string TitleScreen = "TitleScreen";
    [SerializeField] private string PreStreamScene = "PreStreamScene";

    private UIDocument _document;

    private Button _back;
    private Button _start;

    private Button _AsylumSelectButton;
    private Button _SubwaySelectButton;
    private Button _TweakHQSelectButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _back = _document.rootVisualElement.Q("BackButton") as Button;
        _back.RegisterCallback<ClickEvent>(OnBackClick);

        _start = _document.rootVisualElement.Q("PrepareStreamButton") as Button;
        _start.RegisterCallback<ClickEvent>(OnPrepStreamClick);

        _AsylumSelectButton = _document.rootVisualElement.Q("AsylumLevel") as Button;
        _AsylumSelectButton.RegisterCallback<ClickEvent>(OnAsylumSelected);

        _SubwaySelectButton = _document.rootVisualElement.Q("SubwayLevel") as Button;
        _SubwaySelectButton.RegisterCallback<ClickEvent>(OnSubwaySelected);

        _TweakHQSelectButton = _document.rootVisualElement.Q("BackroomLevel") as Button;
        _TweakHQSelectButton.RegisterCallback<ClickEvent>(OnTweakHQSelected);
    }

    private void Start()
    {
        AsylumSelectedIcon.SetActive(true);
        SubwaySelectedIcon.SetActive(false);
        TweakHQSelectedIcon.SetActive(false);
    }

    private void OnDisable()
    {
        _back.UnregisterCallback<ClickEvent>(OnBackClick);
        _start.UnregisterCallback<ClickEvent>(OnPrepStreamClick);
    }

    private void OnBackClick(ClickEvent evt)
    {
        Debug.Log("Returning to title");
        SceneManager.LoadScene(TitleScreen);
    }

    private void OnPrepStreamClick(ClickEvent evt)
    {
        Debug.Log("Preparing Stream!");
        SceneManager.LoadScene(PreStreamScene);
    }

    private void OnAsylumSelected(ClickEvent evt)
    {
        Debug.Log("Asylum Selected");
        AsylumSelectedIcon.SetActive(true);
        SubwaySelectedIcon.SetActive(false);
        TweakHQSelectedIcon.SetActive(false);
        controller.SelectLevel("AsylumLevel2");
    }

    private void OnSubwaySelected(ClickEvent evt)
    {
        Debug.Log("Subway Selected");
        AsylumSelectedIcon.SetActive(false);
        SubwaySelectedIcon.SetActive(true);
        TweakHQSelectedIcon.SetActive(false);
        controller.SelectLevel("subway");
    }

    private void OnTweakHQSelected(ClickEvent evt)
    {
        Debug.Log("Tweak HQ Selected");
        AsylumSelectedIcon.SetActive(false);
        SubwaySelectedIcon.SetActive(false);
        TweakHQSelectedIcon.SetActive(true);
        controller.SelectLevel("TweakHQ");
    }

}
