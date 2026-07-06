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

    private IEnumerator LoseRoutine()
    {
        Vector3 originalPos = rectTransform.anchoredPosition;
        float elapsed = 0f;
        float shakeDuration = 0.5f;

        // 1. Aþama: Korkudan titreme efekti
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float randomX = Random.Range(-8f, 8f);
            float randomY = Random.Range(-8f, 8f);
            rectTransform.anchoredPosition = new Vector3(originalPos.x + randomX, originalPos.y + randomY, 0);
            yield return null;
        }

        // 2. Aþama: Hýzla aþaðý düþüp ekrandan çýkma
        elapsed = 0f;
        Vector3 dropTarget = originalPos + Vector3.down * 800f; // Ekranýn altýna fýrlat

        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, dropTarget, elapsed / 0.5f);
            rectTransform.Rotate(0, 0, 50f * Time.deltaTime); // Düþerken kendi etrafýnda dönsün
            yield return null;
        }
    }
}