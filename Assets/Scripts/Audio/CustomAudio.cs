using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CustomAudio : MonoBehaviour
{
    public AudioSource source { get; private set; }

    private void Awake() =>
        source = GetComponent<AudioSource>();

    public void Fade(float fadeTime = .5f) => StartCoroutine(C_Fade(fadeTime));

    private void Update()
    {
        if (!source.isPlaying) Stop();
    }

    private IEnumerator C_Fade(float fadeTime = 0.5f)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(
                startVolume,
                0f,
                Mathf.SmoothStep(0f, 1f, time / fadeTime)
            );

            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        source.volume = 0.0f;
        Stop();
    }


    public void Stop()
    {
        source.Stop();
        Destroy(gameObject);
    }
}