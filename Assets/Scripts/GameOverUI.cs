using UnityEngine;
using System.Collections;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 originalScale;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    // KAZANAN YAZISI ÝÇÝN: Yaylanarak büyüme efekti
    public void PlayWinAnimation()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(WinRoutine());
    }

    // KAYBEDEN YAZISI ÝÇÝN: Titreyip aþaðý düþme efekti
    public void PlayLoseAnimation()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(LoseRoutine());
    }

    private IEnumerator WinRoutine()
    {
        // Yazýyý önce tamamen küçültüyoruz (görünmez yapýyoruz)
        rectTransform.localScale = Vector3.zero;
        float elapsed = 0f;
        float duration = 0.4f;

        // Büyüme ve yaylanma animasyonu (Kurþun geçirmez matematik)
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            // Sinüs dalgasý kullanarak yazýyý hafifçe hedef boyuttan büyük yapýp sonra oturtuyoruz
            float scaleModifier = Mathf.Sin(percent * Mathf.PI * 1.5f) * 0.3f;
            rectTransform.localScale = originalScale * (percent + scaleModifier);

            yield return null;
        }
        rectTransform.localScale = originalScale;
    }

    //private IEnumerator LoseRoutine()
    //{
    //    Vector3 originalPos = rectTransform.anchoredPosition;
    //    float elapsed = 0f;
    //    float shakeDuration = 0.5f;

    //    // 1. Aþama: Korkudan titreme efekti
    //    while (elapsed < shakeDuration)
    //    {
    //        elapsed += Time.deltaTime;
    //        float randomX = Random.Range(-8f, 8f);
    //        float randomY = Random.Range(-8f, 8f);
    //        rectTransform.anchoredPosition = new Vector3(originalPos.x + randomX, originalPos.y + randomY, 0);
    //        yield return null;
    //    }

    //    // 2. Aþama: Hýzla aþaðý düþüp ekrandan çýkma
    //    elapsed = 0f;
    //    Vector3 dropTarget = originalPos + Vector3.down * 800f; // Ekranýn altýna fýrlat

    //    while (elapsed < 0.5f)
    //    {
    //        elapsed += Time.deltaTime;
    //        rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, dropTarget, elapsed / 0.5f);
    //        rectTransform.Rotate(0, 0, 50f * Time.deltaTime); // Düþerken kendi etrafýnda dönsün
    //        yield return null;
    //    }

    private IEnumerator LoseRoutine()
    {
        Vector3 originalPos = rectTransform.anchoredPosition;
        float elapsed = 0f;
        float shakeDuration = 0.5f;

        // 1. AÞAMA: Korku Titremesi (Ayný kalýyor)
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            rectTransform.anchoredPosition = originalPos + new Vector3(Random.Range(-8f, 8f), Random.Range(-8f, 8f), 0);
            yield return null;
        }

        // 2. AÞAMA: Serbest Düþüþ
        elapsed = 0f;
        float fallDuration = 0.4f;
        // Yazýnýn düþeceði yer: Ekranýn alt sýnýrýna yakýn bir nokta (-350 gibi, ekranýna göre ayarla)
        Vector3 floorPos = new Vector3(originalPos.x, -350f, 0);
        Vector3 startPos = rectTransform.anchoredPosition;

        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;
            // Yerçekimi hissi için kavisli (EaseIn) düþüþ
            t = t * t;
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, floorPos, t);
            yield return null;
        }

        // 3. AÞAMA: Yere Çarpma ve Ezilme (Impact & Squash)
        elapsed = 0f;
        float impactDuration = 0.1f;
        Vector3 squashScale = new Vector3(1.4f, 0.6f, 1f); // Yere çarpýnca yassýlaþsýn

        while (elapsed < impactDuration)
        {
            elapsed += Time.deltaTime;
            rectTransform.localScale = Vector3.Lerp(originalScale, squashScale, elapsed / impactDuration);
            yield return null;
        }

        // 4. AÞAMA: Boynu Bükük Devrilme (Shatter/Tip Over)
        elapsed = 0f;
        float settleDuration = 0.2f;
        // Yana doðru devrilmiþ ve hafif zýplamýþ hali
        Quaternion targetRotation = Quaternion.Euler(0, 0, Random.Range(15f, 30f) * (Random.value > 0.5f ? 1 : -1));

        while (elapsed < settleDuration)
        {
            elapsed += Time.deltaTime;
            rectTransform.localScale = Vector3.Lerp(squashScale, originalScale, elapsed / settleDuration);
            rectTransform.localRotation = Quaternion.Lerp(Quaternion.identity, targetRotation, elapsed / settleDuration);
            yield return null;
        }
    }
}