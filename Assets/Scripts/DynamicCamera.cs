using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    //[Header("Kamera Sabitleri (Dokunma)")]
    //private float startOrthoSize;
    //private float startY;
    //private Camera cam;
    //private float highestY;

    //[Header("Geri Çekilme (Zoom Out) Ayarlarý")]
    //[Tooltip("Kule bu yüksekliðe gelene kadar kamera KESÝNLÝKLE KIPIRDAMAZ")]
    //public float zoomTetiklenmeYuksekligi = 5f;

    //[Tooltip("Kule sýnýrý geçtikten sonra kameranýn ne kadar hýzlý küçüleceði")]
    //public float zoomCarpani = 0.6f;
    //public float smoothSpeed = 3f;

    //[Header("Zemin ve Spawn Noktasý")]
    //public Transform balanceBase;
    //public Transform spawnPoint;

    //void Start()
    //{
    //    cam = GetComponent<Camera>();

    //    // Sen Unity'de kamerayý nasýl ayarladýysan, o ayarlarý baþlangýç kabul ediyoruz
    //    startOrthoSize = cam.orthographicSize;
    //    startY = transform.position.y;

    //    if (balanceBase != null) highestY = balanceBase.position.y;
    //}

    //void Update()
    //{
    //    if (balanceBase == null) return;

    //    GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
    //    float currentHighestY = balanceBase.position.y;

    //    foreach (GameObject block in blocks)
    //    {
    //        if (block.transform.position.y > currentHighestY)
    //        {
    //            currentHighestY = block.transform.position.y;
    //        }
    //    }

    //    // Hatalý sýçramalarý filtrele
    //    if (currentHighestY > highestY && currentHighestY < highestY + 5f)
    //    {
    //        highestY = currentHighestY;
    //    }

    //    // Kule baþlangýç noktasýndan (zemin) ne kadar yükseldi?
    //    float kuleYuksekligi = highestY - balanceBase.position.y;

    //    // Baþlangýçta hedeflerimiz senin Unity'de ayarladýðýn sabit ilk deðerler
    //    float targetOrthoSize = startOrthoSize;
    //    float targetY = startY;

    //    // EÐER KULE BELÝRLENEN SINIRI GEÇERSE (ÝLK KISIM DOLARSA)
    //    if (kuleYuksekligi > zoomTetiklenmeYuksekligi)
    //    {
    //        // Sýnýrý ne kadar aþtýysa, kamerayý o oranda geriye çek ve biraz yukarý kaydýr
    //        float asimMiktari = kuleYuksekligi - zoomTetiklenmeYuksekligi;
    //        targetOrthoSize = startOrthoSize + (asimMiktari * zoomCarpani);
    //        targetY = startY + (asimMiktari * (zoomCarpani * 0.8f));
    //    }

    //    // HESAPLANAN DEÐERLERE YUMUÞAKÇA GEÇÝÞ YAP
    //    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthoSize, smoothSpeed * Time.deltaTime);
    //    Vector3 targetPos = new Vector3(transform.position.x, targetY, transform.position.z);
    //    transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

    //    // YENÝ DOÐACAK BLOK (SPAWN POINT) HER ZAMAN KAMERANIN ÝÇÝNDE KALSIN
    //    if (spawnPoint != null)
    //    {
    //        // Kameranýn o anki en üst kenarýný buluyoruz
    //        float camTopEdge = transform.position.y + cam.orthographicSize;

    //        // Spawn noktasýný üst kenarýn azýcýk (1 birim) altýna koyuyoruz ki hep gözüksün
    //        float targetSpawnY = camTopEdge - 1f;

    //        // Eðer blok kuleye çok yakýnsa diye güvenlik mesafesi ekliyoruz
    //        targetSpawnY = Mathf.Max(targetSpawnY, highestY + 3f);

    //        Vector3 targetSpawnPos = new Vector3(spawnPoint.position.x, targetSpawnY, spawnPoint.position.z);
    //        spawnPoint.position = Vector3.Lerp(spawnPoint.position, targetSpawnPos, smoothSpeed * Time.deltaTime);
    //    }
    //}
}