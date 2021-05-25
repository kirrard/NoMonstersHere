using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void FadeIn(float fadeTime)
    {
        StartCoroutine(FadeInCoroutine(fadeTime));
    }

    public void FadeOut(float fadeTime)
    {
        StartCoroutine(FadeOutCoroutine(fadeTime));
    }

    public IEnumerator FadeInCoroutine(float fadeTime)
    {
        Color color = blackImg.color;

        while (color.a >= 0)
        {
            color.a -= Time.deltaTime / fadeTime;
            blackImg.color = color;

            yield return null;
        }
    }

    public IEnumerator FadeOutCoroutine(float fadeTime)
    {
        Color color = blackImg.color;

        while (color.a <= 1)
        {
            color.a += Time.deltaTime / fadeTime;
            blackImg.color = color;

            yield return null;
        }
    }
}
