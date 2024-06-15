using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(buildIndex + 1);
        SceneManager.LoadSceneAsync(buildIndex + 2, LoadSceneMode.Additive);
    }

    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}