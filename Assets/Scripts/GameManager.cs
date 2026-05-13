using UnityEngine;

// --- STATE PATTERN (DURUM KALIBI) ---
// Oyunun anlýk olarak hangi aþamada olduðunu belirlediðimiz liste
public enum GameState
{
    WaitingForP1,   // Oyuncu 1 blok seçmesini/sürüklemesini bekliyor
    WaitingForP2,   // Oyuncu 2 blok seçmesini/sürüklemesini bekliyor
    Dragging,       // Þu an bir blok havada sürükleniyor
    CheckingBalance,// Blok býrakýldý, kule yýkýlacak mý diye izliyoruz
    GameOver        // Kule yýkýldý, oyun bitti
}

public class GameManager : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    public static GameManager Instance { get; private set; }

    [Header("Oyun Durumu")]
    public GameState currentState;

    private void Awake()
    {
        // Singleton Kurulumu
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Oyuna P1'in sýrasýyla baþlýyoruz
        ChangeState(GameState.WaitingForP1);
    }

    // Durumlarý deðiþtireceðimiz ve her durumda ne olacaðýný yöneten ana fonksiyon
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Oyun Durumu Deðiþti: " + currentState);

        switch (currentState)
        {
            case GameState.WaitingForP1:
                // P1 için yeni blok spawn etme veya seçmesine izin verme kodlarý buraya gelecek
                break;
            case GameState.WaitingForP2:
                // P2 için blok spawn etme kodlarý buraya gelecek
                break;
            case GameState.Dragging:
                // Blok sürüklenirken arka planda yapýlacak þeyler (varsa)
                break;
            case GameState.CheckingBalance:
                // Blok býrakýldýðýnda 2-3 saniye kulenin yýkýlýp yýkýlmadýðýný kontrol edeceðiz
                break;
            case GameState.GameOver:
                // Kule yýkýldýysa kaybedeni göster
                break;
        }
    }
}