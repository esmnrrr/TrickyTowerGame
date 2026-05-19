using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasBeenPlaced = false;

    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 5f;       // Saða sola gitme hýzý
    public float rotationAmount = 90f; // Dönme açýsý

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Havada asýlý baþlasýn
    }

    void Update()
    {
        // Blok zaten býrakýldýysa artýk tuþ basýmlarýný dinleme
        if (hasBeenPlaced) return;

        // --- DEVREYE GÝREN OYUNCU KONTROLLERÝ ---
        // Oyun P1'i bekliyorsa sadece P1'in tuþlarýný dinle
        if (GameManager.Instance.currentState == GameState.WaitingForP1)
        {
            HandlePlayer1Input();
        }
        // Oyun P2'yi bekliyorsa sadece P2'nin tuþlarýný dinle
        else if (GameManager.Instance.currentState == GameState.WaitingForP2)
        {
            HandlePlayer2Input();
        }
    }

    void HandlePlayer1Input()
    {
        // OYUNCU 1 HAREKET (A ve D Tuþlarý)
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);

        // OYUNCU 1 DÖNDÜRME (W ve S Tuþlarý)
        if (Input.GetKeyDown(KeyCode.W)) transform.Rotate(0, 0, rotationAmount);
        if (Input.GetKeyDown(KeyCode.S)) transform.Rotate(0, 0, -rotationAmount);

        // OYUNCU 1 BÝR BIRAKMA (Boþluk - Space Tuþu)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropBlock();
        }
    }

    void HandlePlayer2Input()
    {
        // OYUNCU 2 HAREKET (Sol ve Sað Ok Tuþlarý)
        // (Numpad kapalýyken 4 ve 6 da ok tuþu görevi görür)
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);

        // OYUNCU 2 DÖNDÜRME (Yukarý ve Aþaðý Ok Tuþlarý)
        if (Input.GetKeyDown(KeyCode.UpArrow)) transform.Rotate(0, 0, rotationAmount);
        if (Input.GetKeyDown(KeyCode.DownArrow)) transform.Rotate(0, 0, -rotationAmount);

        // OYUNCU 2 BIRAKMA (Enter veya Numpad Enter Tuþu)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            DropBlock();
        }
    }

    // Ortak Býrakma Fonksiyonu
    void DropBlock()
    {
        hasBeenPlaced = true;

        // Fiziði aç, yerçekimi etki etsin
        rb.bodyType = RigidbodyType2D.Dynamic;

        // KAMERA ÝÇÝN YENÝ SATIR: Obje býrakýldýðý an kameranýn görmesi için etiketini deðiþtiriyoruz!
        gameObject.tag = "Block";

        // GameManager'a sýranýn bittiðini ve dengeyi kontrol etmesini söyle
        GameManager.Instance.ChangeState(GameState.CheckingBalance);
    }

    // Çarpýþma ve Yýkýlma Kontrolü (Burasý ayný kalýyor)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            if (GameManager.Instance.currentState != GameState.GameOver)
            {
                Debug.Log("Kule Yýkýldý! Blok yere deðdi!");
                GameManager.Instance.ChangeState(GameState.GameOver);
            }
        }
    }
}