using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string selectScene = "SelectScene";
    [SerializeField] private string shopScene = "ShopScene";
    [SerializeField] private string statisticsScene = "StatScene";
    public void StartStreamButton()
    {
        SceneManager.LoadScene(selectScene);
    }

    public void OpenShopButton()
    {
        SceneManager.LoadScene(shopScene);
    }

    public void StatisticsButton()
    {
        SceneManager.LoadScene(statisticsScene);
    }
}
