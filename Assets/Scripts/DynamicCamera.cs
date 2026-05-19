using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [Header("Kamera Ayarlarý")]
    public Transform balanceBase;
    public float smoothSpeed = 3f;
    public float minOrthoSize = 5f;      // Kameranýn en yakýn hali
    public float maxOrthoSize = 12f;     // EKLENDÝ: Kameranýn aþýrý uzaklaþmasýný engelleyen sýnýr

    // DEÐÝÞTÝRÝLDÝ: Üst boþluðu çok daha fazla verdik ki bloklar hep ekranýn içinde doðsun
    public float topPadding = 8f;
    public float bottomPadding = 2f;

    [Header("Spawn Noktasý")]
    public Transform spawnPoint;
    // DEÐÝÞTÝRÝLDÝ: Blok en yüksek noktanýn azýcýk üstünde doðsun ki topPadding'in içinde kalsýn
    public float spawnHeightOffset = 3f;

    private Camera cam;
    private float highestY;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (balanceBase != null) highestY = balanceBase.position.y;
    }

    void Update()
    {
        if (balanceBase == null) return;

        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        float currentHighestY = balanceBase.position.y;

        // Kule yüksekliðini hesapla
        foreach (GameObject block in blocks)
        {
            if (block.transform.position.y > currentHighestY)
            {
                currentHighestY = block.transform.position.y;
            }
        }

        // Eðer sekip uzaya uçan hatalý bir blok varsa kamerayý bozmasýn diye makul bir sýnýr koyuyoruz
        // Sadece yavaþça yükselen makul rekorlarý kabul et
        if (currentHighestY > highestY && currentHighestY < highestY + 5f)
        {
            highestY = currentHighestY;
        }

        // 1. ZOOM OUT HESAPLAMASI (Clamp ile sýnýrlandýrýldý)
        float requiredHeight = (highestY - balanceBase.position.y) + topPadding + bottomPadding;

        // EKLENDÝ: Zoom miktarý minOrthoSize ile maxOrthoSize arasýna sýkýþtýrýldý!
        float targetOrthoSize = Mathf.Clamp(requiredHeight / 2f, minOrthoSize, maxOrthoSize);

        // 2. YUKARI KAYMA HESAPLAMASI
        float targetY = balanceBase.position.y - bottomPadding + targetOrthoSize;

        // HAREKETLERÝ UYGULA
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthoSize, smoothSpeed * Time.deltaTime);
        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 3. SPAWN NOKTASINI GÜNCELLE
        if (spawnPoint != null)
        {
            float spawnY = Mathf.Lerp(spawnPoint.position.y, highestY + spawnHeightOffset, smoothSpeed * Time.deltaTime);
            spawnPoint.position = new Vector3(spawnPoint.position.x, spawnY, spawnPoint.position.z);
        }
    }
}