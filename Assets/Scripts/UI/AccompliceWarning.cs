using TMPro;
using UnityEngine;

public class AccompliceWarning : MonoBehaviour
{
    public TextMeshProUGUI text;
    public HoverText hoverText;

    private void Start()
    {
        int accomplices = 0;
        foreach(Character character in PuzzleManager.Instance.characters)
        {
            if (character.isAccomplice) accomplices++;
        }

        text.text = $"{accomplices} Accomplice{(accomplices > 1 ? "s" : "")}";

        hoverText.text = $"At this party, there {(accomplices > 1 ? "are" : "is")} {accomplices} accomplice{(accomplices > 1 ? "s" : "")}.\nUnless they have the honest trait, accomplices will always give incorrect information.\nThe killer is always counted as an accomplice.";
    }
}
