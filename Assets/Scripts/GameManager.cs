using UnityEngine;
using System.Collections; // Coroutine (bekleme süresi) kullanabilmek için bunu ekledik
using System; // ACTION kullanabilmek için ekledik
using UnityEngine.SceneManagement; // Sahne geçiþleri ve yeniden baþlatma için zorunlu!

public enum GameState
{
    WaitingForP1,
    WaitingForP2,
    Dragging,
    CheckingBalance,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState currentState;

    [Header("Blok ve Spawn Ayarlarý")]
    public GameObject[] blockPrefabs; // Oluþturduðun Tetris prefab'larýný buraya atacaksýn
    public Transform spawnPoint;      // Bloklarýn tepede belireceði nokta

    public int currentPlayer = 1;    // Sýranýn kimde olduðunu tutar (1 veya 2)

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        OnScreenShake = null;
        OnGameStateChanged = null;
    }

    // OBSERVER PATTERN: Oyun durumunun deðiþtiðini diðer sistemlere bildirmek için evrensel sinyal
    public static event Action<GameState> OnGameStateChanged;

    // OBSERVER PATTERN: Diðer sistemlerin dinleyebileceði evrensel sinyal
    public static event Action OnScreenShake;

    // Herhangi bir obje bu fonksiyonu çaðýrarak sinyali ateþleyebilir
    public static void TriggerShake()
    {
        OnScreenShake?.Invoke(); // Eðer bu sinyali dinleyen biri varsa, çalýþtýr!
    }

    private void Start()
    {
        // Oyunu P1'in sýrasýyla baþlatýyoruz
        ChangeState(GameState.WaitingForP1);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Oyun Durumu: " + currentState);

        // Oyun durumunun deðiþtiðini diðer sistemlere bildirmek için sinyali ateþle
        OnGameStateChanged?.Invoke(currentState);

        switch (currentState)
        {
            case GameState.WaitingForP1:
                currentPlayer = 1;
                SpawnBlock();
                break;
            case GameState.WaitingForP2:
                currentPlayer = 2;
                SpawnBlock();
                break;
            case GameState.CheckingBalance:
                // Blok býrakýldý, sallantýnýn durmasý için 2 saniye beklemeye baþlýyoruz
                StartCoroutine(CheckBalanceRoutine());
                break;
            case GameState.GameOver:
                Debug.Log("GAME OVER! Loser: Player " + currentPlayer);
                break;
        }
    }

    // Rastgele bir blok seçip havada belirlemesini saðlayan fonksiyon
    void SpawnBlock()
    {
        if (blockPrefabs.Length > 0 && spawnPoint != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, blockPrefabs.Length);
            Instantiate(blockPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        }
    }

    // 2 saniyelik zamanlayýcý (Oyunun hata vermeden beklemesini saðlayan sihirli yapý)
    IEnumerator CheckBalanceRoutine()
    {
        yield return new WaitForSeconds(2.5f); // 2.5 saniye kulenin sallantýsýný izle

        // Eðer bu 2.5 saniye içinde blok yere düþüp "GameOver" olmadýysa, sýrayý deðiþtir!
        if (currentState != GameState.GameOver)
        {
            if (currentPlayer == 1) ChangeState(GameState.WaitingForP2);
            else ChangeState(GameState.WaitingForP1);
        }
    }

    // Yeniden Baþla butonu bu fonksiyonu tetikleyecek
    public void RestartGame()
    {
        // Unity'ye diyoruz ki: "Þu an açýk olan sahneyi bul ve onu baþtan yükle!"
        // Bu, tüm deðiþkenleri, kuleyi ve her þeyi kusursuzca sýfýrlar.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Ana Menüye Dön butonu bu fonksiyonu tetikleyecek
    public void LoadMainMenu()
    {
        // Ýleride adýný "MainMenu" yapacaðýmýz sahneyi yükleyecek
        SceneManager.LoadScene("MainMenu");
    }
}