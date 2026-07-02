using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasBeenPlaced = false;
    private bool isDragging = false;

    [Header("Hassas Döndürme Ayarlarý")]
    [Tooltip("A/D veya Ok tuþlarýna basýlý tutarken saniyede kaç derece döneceði")]
    public float continuousRotationSpeed = 150f;

    [Tooltip("Fare tekerleði ile her týkta kaç derece döneceði")]
    public float scrollRotationStep = 5f;

    [Header("Efektler")]
    public GameObject dustParticlePrefab;
    private TrailRenderer tr;
    private SquashAndStretch squashAnimator;
    private bool hasPlayedEffects = false;

    private Vector3 offset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        tr = GetComponent<TrailRenderer>();
        if (tr != null) tr.enabled = false;

        squashAnimator = GetComponent<SquashAndStretch>();
    }

    void Update()
    {
        if (hasBeenPlaced) return;

        // Blok fareyle taþýnýrken hassas döndürme girdilerini dinle
        if (isDragging)
        {
            HandlePrecisionRotation();
        }
    }

    void OnMouseDown()
    {
        if (hasBeenPlaced) return;

        if (GameManager.Instance.currentState == GameState.WaitingForP1 ||
            GameManager.Instance.currentState == GameState.WaitingForP2)
        {
            isDragging = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
    }

    void OnMouseDrag()
    {
        if (!isDragging || hasBeenPlaced) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, transform.position.z);
    }

    void OnMouseUp()
    {
        if (!isDragging || hasBeenPlaced) return;

        isDragging = false;
        DropBlock();
    }

    // KONTROL PANELÝ AYRILDI: Oyuncu durumuna göre kilit açma mantýðý
    void HandlePrecisionRotation()
    {
        // GameManager'dan o an sýranýn hangi oyuncuda olduðunu çekiyoruz
        GameState currentState = GameManager.Instance.currentState;

        // 1. OYUNCU 1'ÝN SIRASIYSA: Sadece A ve D tuþlarý çalýþýr, Ok tuþlarý kilitlenir!
        if (currentState == GameState.WaitingForP1)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, 0, continuousRotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, 0, -continuousRotationSpeed * Time.deltaTime);
            }
        }
        // 2. OYUNCU 2'NÝN SIRASIYSA: Sadece Sol ve Sað Ok tuþlarý çalýþýr, A ve D kilitlenir!
        else if (currentState == GameState.WaitingForP2)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, 0, continuousRotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, 0, -continuousRotationSpeed * Time.deltaTime);
            }
        }

        // 3. FARE TEKERLEÐÝ (SCROLL): Ortak bir fiziksel kontrolcü olduðu için sýradan baðýmsýz býraktým.
        // Kim bloðu sürüklüyorsa tekerlekle ince ayarýný yine 5'er derece yapabilir.
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll > 0)
        {
            transform.Rotate(0, 0, scrollRotationStep);
        }
        else if (mouseScroll < 0)
        {
            transform.Rotate(0, 0, -scrollRotationStep);
        }
    }

    void DropBlock()
    {
        hasBeenPlaced = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        gameObject.tag = "Block";

        if (tr != null) tr.enabled = true;

        GameManager.Instance.ChangeState(GameState.CheckingBalance);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            if (GameManager.Instance.currentState != GameState.GameOver)
            {
                Debug.Log("Kule Yýkýldý!");
                GameManager.Instance.ChangeState(GameState.GameOver);
            }
        }

        if (hasBeenPlaced && !hasPlayedEffects && collision.relativeVelocity.magnitude > 1.5f)
        {
            hasPlayedEffects = true;
            GameManager.TriggerShake();

            if (dustParticlePrefab != null)
            {
                Vector2 temasNoktasi = collision.GetContact(0).point;
                GameObject toz = Instantiate(dustParticlePrefab, temasNoktasi, Quaternion.identity);
                Destroy(toz, 1f);
            }

            if (squashAnimator != null)
            {
                squashAnimator.PlaySquash();
            }
        }

        if (hasBeenPlaced && tr != null)
        {
            tr.enabled = false;
        }
    }
}