using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // �T�O�ޥ� UI �R�W�Ŷ�

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
        Cursor.visible = false; // ���ù���
        Cursor.lockState = CursorLockMode.Locked; // ��w����
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true; // ��ܹ���
        Cursor.lockState = CursorLockMode.None; // ���񹫼�
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // ��_�ɶ��y��
        SceneManager.LoadScene("menu"); // �[���ʭ������A�T�O�ӳ������W�٬� "menu"
    }
}
