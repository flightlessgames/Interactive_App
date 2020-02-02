using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyLevelLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("loading" + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
