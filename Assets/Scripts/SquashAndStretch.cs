using UnityEngine;
using System.Collections;

public class SquashAndStretch : MonoBehaviour
{
    [Header("Esneme Ayarlarý")]
    public float duration = 0.15f; // Animasyonun ne kadar süreceði (çok hýzlý olmalý)

    // Hacim Koruma Kuralý: Y ekseni 0.8'e eziliyorsa, X ekseni 1.2'ye þiþmeli!
    public Vector3 squashScale = new Vector3(1.3f, 0.7f, 1f);

    private Vector3 originalScale;
    private Coroutine squashCoroutine;

    void Start()
    {
        // Objenin sahnede doðduðu anki orijinal boyutunu hafýzaya al
        originalScale = transform.localScale;
    }

    // Dýþarýdan (örneðin BlockController'dan) çaðrýlacak tetikleyici fonksiyon
    public void PlaySquash()
    {
        // Eðer zaten esniyorsa animasyonu sýfýrla ki üst üste binip bozulmasýn
        if (squashCoroutine != null)
        {
            StopCoroutine(squashCoroutine);
            transform.localScale = originalScale;
        }

        squashCoroutine = StartCoroutine(SquashRoutine());
    }

    private IEnumerator SquashRoutine()
    {
        float elapsed = 0f;
        // Animasyonun yarýsý ezilme, yarýsý eski haline dönme olacak
        float halfDuration = duration / 2f;

        // 1. AÞAMA: Yere çarpma ve ezilme
        while (elapsed < halfDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, squashScale, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 2. AÞAMA: Orijinal þekle (eski haline) geri zýplama
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            transform.localScale = Vector3.Lerp(squashScale, originalScale, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Animasyon bittiðinde boyutun milimetrik olarak orijinal kalmasýný garantiye al
        transform.localScale = originalScale;
    }
}