using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 確保引用 UI 命名空間

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false; // 隱藏鼠標
        Cursor.lockState = CursorLockMode.Locked; // 鎖定鼠標
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true; // 顯示鼠標
        Cursor.lockState = CursorLockMode.None; // 釋放鼠標
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // 恢復時間流動
        SceneManager.LoadScene("menu"); // 加載封面場景，確保該場景的名稱為 "menu"
    }
}
