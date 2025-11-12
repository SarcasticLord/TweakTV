using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenuScript : MonoBehaviour
{

    [SerializeField] private string selectScene = "SelectScene";
    [SerializeField] private string shopScene = "ShopScene";
    [SerializeField] private string statScene = "StatScene";

    private UIDocument _document;

    private Button _start;
    private Button _shop;
    private Button _stats;
    private Button _quit;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _start = _document.rootVisualElement.Q("StartGameButton") as Button;
        _start.RegisterCallback<ClickEvent>(OnPlayGameClick);

        _shop = _document.rootVisualElement.Q("OpenShopButton") as Button;
        _shop.RegisterCallback<ClickEvent>(OnShopClick);

        _stats = _document.rootVisualElement.Q("OpenStatsButton") as Button;
        _stats.RegisterCallback<ClickEvent>(OnStatsClick);

        _quit = _document.rootVisualElement.Q("QuitGameButton") as Button;
        _quit.RegisterCallback<ClickEvent>(OnQuitClick);
    }

    private void OnDisable()
    {
        _start.UnregisterCallback<ClickEvent>(OnPlayGameClick);
        _shop.UnregisterCallback<ClickEvent>(OnShopClick);
        _stats.UnregisterCallback<ClickEvent>(OnStatsClick);
        _quit.UnregisterCallback<ClickEvent>(OnQuitClick);
    }

    private void OnPlayGameClick(ClickEvent evt)
    {
        Debug.Log("Initiate Tweaking");
        SceneManager.LoadScene(selectScene);
    }

    private void OnShopClick(ClickEvent evt)
    {
        Debug.Log("www.Congo.com");
        SceneManager.LoadScene(shopScene);
    }

    private void OnStatsClick(ClickEvent evt)
    {
        Debug.Log("Opening Stream Statistics");
        SceneManager.LoadScene(statScene);
    }

    private void OnQuitClick(ClickEvent evt)
    {
        Debug.Log("Bye bye!");
    }
}
