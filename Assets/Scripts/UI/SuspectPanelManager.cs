using System.Collections;
using UnityEngine;

public class SuspectPanelManager : MonoBehaviour
{
    public GameObject openButton;
    public GameObject closeButton;

    public void OpenScreen()
    {
        StartCoroutine(SetScreenPivot(0f, 0.5f));
        openButton.SetActive(false);
        closeButton.SetActive(true);
    }

    public void CloseScreen()
    {
        StartCoroutine(SetScreenPivot(1f, 0.5f));
        openButton.SetActive(true);
        closeButton.SetActive(false);
    }

    IEnumerator SetScreenPivot(float newXPivot, float overTime)
    {
        float startTime = Time.time;
        float endTime = startTime + overTime;

        float startXPivot = ((RectTransform)transform).pivot.x;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / overTime;

            t = t * t * t;

            ((RectTransform)transform).pivot = new Vector2(Mathf.Lerp(startXPivot, newXPivot, t), 0.5f);

            yield return new WaitForEndOfFrame();
        }

        ((RectTransform)transform).pivot = new Vector2(newXPivot, 0.5f);
    }
}
