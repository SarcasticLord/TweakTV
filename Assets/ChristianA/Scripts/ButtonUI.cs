using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string shopScene = "ShopScene";
    public void StartStreamButton()
    {
        return;
    }

    public void OpenShopButton()
    {
        SceneManager.LoadScene(shopScene);
    }

    public void StatisticsButton()
    {
        return;
    }
}
