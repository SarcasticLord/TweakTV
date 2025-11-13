using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance { get { return _instance; } }

    //private bool AsylumSelected = false;
    //private bool SubwaySelected = false;
    //private bool TweakHQSelected = false;

    private static string SelectedLevel = "AsylumLevel2";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void SelectLevel(string level)
    {
        SelectedLevel = level;
    }

    public string GetSelectedLevel()
    {
        return SelectedLevel;
    }
}
