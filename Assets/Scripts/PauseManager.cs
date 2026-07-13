using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Paneli")]
    public GameObject pausePanel;

    // Oyunun durdurulup durdurulmadýðýný takip eden anahtar
    private bool isPaused = false;

    void Update()
    {
        // Klavyeden ESC tuþuna basýldýðýnda tetiklenir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Açýksa kapat
            }
            else
            {
                PauseGame(); // Kapalýysa aç
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Oyunun zaman akýþýný 0'a indirir (Fizik donar)
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Zamanýn akýþýný tekrar normale çevirir
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        //Zamanin akýþýný normale çevirir ve ana menü sahnesine geçiþ yapar
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan çýkýlýyor...");
        Application.Quit();
    }
}