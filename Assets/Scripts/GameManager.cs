using UnityEngine;
using System.Collections; // Coroutine (bekleme süresi) kullanabilmek için bunu ekledik

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

    private int currentPlayer = 1;    // Sýranýn kimde olduðunu tutar (1 veya 2)

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
                Debug.Log("OYUN BÝTTÝ! Kaybeden: Oyuncu " + currentPlayer);
                break;
        }
    }

    // Rastgele bir blok seçip havada belirlemesini saðlayan fonksiyon
    void SpawnBlock()
    {
        if (blockPrefabs.Length > 0 && spawnPoint != null)
        {
            int randomIndex = Random.Range(0, blockPrefabs.Length);
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
}