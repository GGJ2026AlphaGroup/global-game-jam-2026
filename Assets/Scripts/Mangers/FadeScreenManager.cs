using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreenManager : MonoSingleton<FadeScreenManager>
{
    private const float m_FadeTime = .5f;

    [SerializeField] private Canvas m_LoadScreen;
    [SerializeField] private Image m_Screen;

    public void FadeIn(float fadeTime = m_FadeTime, Action OnComplete = null)
    {
        StartCoroutine(C_Fade(1.0f, 0.0f, fadeTime, OnComplete));
    }

    public void FadeOut(float fadeTime = m_FadeTime, Action OnComplete = null)
    {
        StartCoroutine(C_Fade(0.0f, 1.0f, fadeTime, OnComplete));
    }

    private IEnumerator C_Fade(float start, float end, float fadeTime = m_FadeTime, Action OnComplete = null)
    {
        m_LoadScreen.enabled = true;
        float time = 0.0f;
        Color c = m_Screen.color;
        c.a = start;
        m_Screen.color = c;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float t = time / fadeTime;

            t = 1f - Mathf.Exp(-5f * t);

            c.a = Mathf.Lerp(start, end, t);
            m_Screen.color = c;

            yield return null;
        }

        c.a = end;
        m_Screen.color = c;

        if (c.a == 0.0f)
            m_LoadScreen.enabled = false;
        OnComplete?.Invoke();
    }
}