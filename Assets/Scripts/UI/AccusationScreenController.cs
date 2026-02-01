using TMPro;
using UnityEngine;

public class AccusationScreenController : MonoSingleton<AccusationScreenController>
{
    public GameObject holder;

    public TextMeshProUGUI guessText;

    public void OpenScreen()
    {
        guessText.text = $"Guesses remaining: {PuzzleManager.Instance.guessesRemaining}/{PuzzleManager.Instance.startGuesses}";

        holder.SetActive(true);
    }

    public void CloseScreen()
    {
        holder.SetActive(false);
    }
}
