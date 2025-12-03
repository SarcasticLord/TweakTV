using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndLevelUIScripts : MonoBehaviour
{
    private UIDocument _document;

    [SerializeField] private string TitleScreen = "TitleScreen";

    private Button _endStreamButton;


    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        _endStreamButton = _document.rootVisualElement.Q("EndStream") as Button;
        _endStreamButton.RegisterCallback<ClickEvent>(OnEndStreamClick);
    }

    private void Start()
    {
        Label time_label= _document.rootVisualElement.Q("TimeElapsed") as Label;
        if (time_label != null )
        {
            time_label.text = Singleton.Instance.TimeAsString;
        }
        
        Label sub_score_label= _document.rootVisualElement.Q("SubScore") as Label;
        if(sub_score_label != null )
        {
            //sub_score_label =
        }

        Label donation_label= _document.rootVisualElement.Q("DonationsGained") as Label;
        if (donation_label != null )
        {
            //donation_label =
        }

        Label revenue_label= _document.rootVisualElement.Q("RevenueGained") as Label;
        if (revenue_label != null)
        {
            //revenue_label =
        }

        Label tweak_cut_label = _document.rootVisualElement.Q("TweaksCutEntry") as Label;
        if (tweak_cut_label != null)
        {
            //tweak_cut_label = 
        }

        Label total_entry_label= _document.rootVisualElement.Q("TotalEntry") as Label;
        if (total_entry_label != null )
        {
            //TODO MATH
        }
    }

    private void OnDisable()
    {
        _endStreamButton.UnregisterCallback<ClickEvent>(OnEndStreamClick);
    }

    private void OnEndStreamClick(ClickEvent evt)
    {
        Debug.Log("Ending Stream");
        SceneManager.LoadScene(TitleScreen);
    }

    
}
