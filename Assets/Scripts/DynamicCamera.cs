using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [Header("Kamera Kaydýrma Ayarlarý")]
    [Tooltip("Kule, kameranýn merkezinden ne kadar yukarý çýkarsa kamera hareket etsin?")]
    public float yThreshold = 2.5f;

    [Tooltip("Sýnýr geçildiðinde kamera tek seferde ne kadar yukarý kaysýn?")]
    public float moveUpAmount = 3.0f;

    [Tooltip("Kameranýn yukarý kayma yumuþaklýðý (Düþük = daha yavaþ/smooth)")]
    public float smoothSpeed = 4f;

    private float targetY; // Kameranýn gitmek istediði yeni Y koordinatý

    void Start()
    {
        // Oyun baþladýðýnda kameranýn ilk yüksekliðini hedef olarak belirliyoruz
        targetY = transform.position.y;
    }

    void Update()
    {
        float highestBlockY = GetHighestBlockY();

        // Eðer en yüksek bloðun Y koordinatý, kameranýn tetiklenme sýnýrýný geçtiyse
        if (highestBlockY > targetY + yThreshold)
        {
            // Kameranýn yeni hedefini moveUpAmount kadar yukarý taþý!
            targetY += moveUpAmount;
        }

        // Kamerayý her frame'de yavaþça yeni hedefine doðru kaydýr (Lerp ile pürüzsüzleþtirme)
        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }

    // Sahnedeki (Block etiketine sahip) en yüksek bloðu bulan yardýmcý fonksiyon
    float GetHighestBlockY()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        float maxY = -100f; // Referans için en dip nokta

        foreach (GameObject block in blocks)
        {
            // Eðer incelediðimiz blok þu anki en yüksekten daha yukarýdaysa, yeni rekor budur
            if (block.transform.position.y > maxY)
            {
                maxY = block.transform.position.y;
            }
        }
        return maxY;
    }
}