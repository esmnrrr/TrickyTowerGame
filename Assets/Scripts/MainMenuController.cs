using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçiþi için

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Kendi oyun sahnenin adýný buraya tam olarak yaz (Örn: "SampleScene")
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        // Oyundan çýkýþ yapar (Unity Editor içinde çalýþmaz, build alýnca çalýþýr)
        Debug.Log("Oyundan Çýkýlýyor...");
        Application.Quit();
    }
}