using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [Header("Sarsýntý Ayarlarý")]
    public float shakeDuration = 0.15f;  // Ne kadar süre sallanacak?
    public float shakeMagnitude = 0.1f;  // Ne kadar þiddetli sallanacak?

    // KURAL 1: Obje aktifleþtiðinde sinyale (radyo frekansýna) abone ol
    private void OnEnable()
    {
        GameManager.OnScreenShake += StartShake;
    }

    // KURAL 2: Obje kapanýrsa veya silinirse abonelikten çýk (Hata almamak için çok önemli!)
    private void OnDisable()
    {
        GameManager.OnScreenShake -= StartShake;
    }

    // Sinyal geldiðinde tetiklenecek fonksiyon
    private void StartShake()
    {
        // Eðer zaten sallanýyorsa iptal et ve baþtan baþla (Üst üste binmeleri engeller)
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

    // Gerçek sarsýntýyý yapan zamanlayýcý fonksiyon
    private IEnumerator ShakeRoutine()
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Rastgele X ve Y yönlerinde minik kaymalar oluþtur
            float xOffset = Random.Range(-1f, 1f) * shakeMagnitude;
            float yOffset = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = new Vector3(originalPos.x + xOffset, originalPos.y + yOffset, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null; // Bir sonraki frame'e (kareye) kadar bekle
        }

        // Sarsýntý bitince kamerayý tam olarak eski mükemmel yerine oturt
        transform.position = originalPos;
    }
}