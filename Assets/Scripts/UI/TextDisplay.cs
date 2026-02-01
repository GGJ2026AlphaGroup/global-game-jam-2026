using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextDisplay : Singleton<TextDisplay>
{
    public TextMeshProUGUI display;

    public static void DisplayText(string text, float duration, Action OnFinished = null)
    {
        Instance.display.text = text;
        Instance.StartCoroutine(Instance.Display(duration, OnFinished));
    }

    IEnumerator Display(float duration, Action OnFinished)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;

        display.gameObject.SetActive(true);

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime);

            t = Mathf.Min(t, duration - t) / 0.25f;

            t = Mathf.Clamp01(t);

            display.transform.localScale = new Vector3(t, t, t);

            display.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 90f), Quaternion.Euler(0f, 0f, 0f), t);

            yield return new WaitForEndOfFrame();
        }

        display.gameObject.SetActive(false);

        if (OnFinished != null) OnFinished();
    }
}
