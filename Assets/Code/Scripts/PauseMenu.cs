using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused = false;

    public GameObject pauseMenuUi;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1;
        IsGamePaused = false;
    }

    private void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0;
        IsGamePaused = true;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}