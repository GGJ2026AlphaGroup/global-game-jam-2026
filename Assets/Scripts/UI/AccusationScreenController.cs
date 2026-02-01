using TMPro;
using UnityEngine;

public class AccusationScreenController : MonoSingleton<AccusationScreenController>
{
    public GameObject holder;

    public TextMeshProUGUI guessText;

    CustomAudio audio = null;

    public void OpenScreen()
    {
        guessText.text = $"Guesses remaining: {PuzzleManager.Instance.guessesRemaining}/{PuzzleManager.Instance.startGuesses}";

        if (audio == null)
        {
            AudioManager.Instance.StopAllAudio();
            audio = AudioManager.Instance.PlayAudio("audio_accusation", true);
        }

        holder.SetActive(true);
    }

    public void CloseScreen()
    {
        if (audio != null)
        {
            audio.Fade(.5f);
            AudioManager.Instance.PlayAudio("audio_ballroom", true);

            int people = PuzzleManager.Instance.characters.Length;
            if (people <= 5) AudioManager.Instance.PlayAudio("audio_small_crowd", true);            
            else if (people > 5 && people < 10) AudioManager.Instance.PlayAudio("audio_mid_crowd", true);
            else AudioManager.Instance.PlayAudio("audio_large_crowd", true);
        }
        holder.SetActive(false);
    }
}
