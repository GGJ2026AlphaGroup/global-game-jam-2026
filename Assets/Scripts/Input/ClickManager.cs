using UnityEngine;

public class ClickManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlayAudio($"audio_click_{Random.Range(1, 4)}");
        }
    }
}
