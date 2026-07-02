using UnityEngine;
using UnityEngine.UI; // Panellerin arka plan rengini deðiþtirmek için bu þart
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elemanlarý")]
    public TextMeshProUGUI turnText;    // Oyuncu sýrasýný göstermek için

    [Header("Bölünmüþ Game Over Ekraný")]
    public GameObject gameOverPanel;    // Game Over panelini göstermek için

    // Sol Taraf (Oyuncu 1)
    public Image p1Background;          // Arka plan rengini deðiþtirmek için
    public TextMeshProUGUI p1ResultText;// Sonuç metnini göstermek için

    // Sað Taraf (Oyuncu 2)
    public Image p2Background;
    public TextMeshProUGUI p2ResultText;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= UpdateUI;
    }

    private void UpdateUI(GameState newState)
    {
        switch (newState)
        {
            case GameState.WaitingForP1:
                turnText.text = "PLAYER 1 TURN";
                turnText.color = Color.red;
                gameOverPanel.SetActive(false);
                break;

            case GameState.WaitingForP2:
                turnText.text = "PLAYER 2 TURN";
                turnText.color = Color.blue;
                gameOverPanel.SetActive(false);
                break;

            case GameState.CheckingBalance:
                turnText.text = "BALANCE CHECKING...";
                turnText.color = Color.yellow;
                break;

            case GameState.GameOver:
                turnText.gameObject.SetActive(false);
                gameOverPanel.SetActive(true);

                // GameManager'daki currentPlayer o an hamleyi yapaný (yani kuleyi yýkaný) tutar.
                int kuleyiYikan = GameManager.Instance.currentPlayer;

                // Transparan renkler hazýrlýyoruz (Kýrmýzý ve Yeþil)
                Color redColor = new Color(1f, 0.2f, 0.2f, 0.7f);
                Color greenColor = new Color(0.2f, 1f, 0.2f, 0.7f);

                if (kuleyiYikan == 1)
                {
                    // Oyuncu 1 kuleyi yýktý ve KAYBETTÝ
                    p1Background.color = redColor;
                    p1ResultText.text = "LOSE!";
                    
                    p2Background.color = greenColor;
                    p2ResultText.text = "CONGRATULATIONS\nYOU WIN!";
                }
                else
                {
                    // Oyuncu 2 kuleyi yýktý ve KAYBETTÝ
                    p2Background.color = redColor;
                    p2ResultText.text = "LOSE!";
                    
                    p1Background.color = greenColor;
                    p1ResultText.text = "CONGRATULATIONS\nYOU WIN!";
                }
                break;
        }
    }
}