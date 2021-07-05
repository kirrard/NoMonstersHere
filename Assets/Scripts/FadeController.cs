using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    #region Singleton
    public static FadeController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public SpriteRenderer blackImg;
    public SpriteRenderer whiteImg;

    public IEnumerator FadeInCoroutine(float fadeTime)
    {
        blackImg.gameObject.SetActive(true);
        Color color = blackImg.color;

        while (color.a >= 0)
        {
            color.a -= Time.deltaTime / fadeTime;
            blackImg.color = color;

            yield return null;
        }

        blackImg.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float fadeTime)
    {
        blackImg.gameObject.SetActive(true);
        Color color = blackImg.color;

        while (color.a <= 1)
        {
            color.a += Time.deltaTime / fadeTime;
            blackImg.color = color;

            yield return null;
        }

        blackImg.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float fadeTime, Image image)
    {
        Color color = image.color;

        while (color.a >= 0)
        {
            color.a -= Time.deltaTime / fadeTime;
            image.color = color;

            yield return null;
        }
    }

    public IEnumerator FadeInCoroutine(float fadeTime, Image image)
    {
        Color color = image.color;

        while (color.a <= 1)
        {
            color.a += Time.deltaTime / fadeTime;
            image.color = color;

            yield return null;
        }
    }
}
