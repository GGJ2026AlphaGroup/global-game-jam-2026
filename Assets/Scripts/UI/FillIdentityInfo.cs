using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillIdentityInfo : MonoBehaviour
{
    public Character character;

    [Header("Identity Fields")]
    public TextMeshProUGUI maskText;
    public TextMeshProUGUI clothesText;
    public TextMeshProUGUI activityText;
    public TextMeshProUGUI idText;

    public TMP_Dropdown guess;

    void Start()
    {
        RebuildLayout();
    }

    public void BuildLayout()
    {
        if (character.isRevealed)
        {
            guess.options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetNameDisplayName(character.name)) };
            guess.value = 0;
        }
        else
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>() { new TMP_Dropdown.OptionData(Character.GetNameDisplayName(Name.None)) };
            int i = 1;
            int j = 0;
            foreach (Name name in PuzzleManager.Instance.GetAllActiveNames())
            {
                options.Add(new TMP_Dropdown.OptionData(Character.GetNameDisplayName(name)));
                if (name == character.guessedName) j = i;
                i++;
            }
            guess.options = options;
            guess.value = j;
        }
    }

    public void RebuildLayout()
    {
        if (character == null) return;

        maskText.text = Character.GetMaskDisplayName(character.mask);
        clothesText.text = Character.GetClothingDisplayName(character.clothing);
        activityText.text = Character.GetActivityDisplayName(character.activity);
        idText.text = character.isKiller ? "The Killer" : $"Guest #{character.id}";

        BuildLayout();
    }

    public void SetGuess()
    {
        if (guess.value == 0)
        {
            character.guessedName = Name.None;
        }
        else
        {
            character.guessedName = PuzzleManager.Instance.GetAllActiveNames()[guess.value - 1];

            foreach (Character c in PuzzleManager.Instance.characters)
            {
                if (c.name == character.guessedName)
                {
                    c.guessedMask = character.mask;
                    c.guessedClothing = character.clothing;
                    c.guessedActivity = character.activity;
                    c.RegisterChange();
                    return;
                }
            }
        }
    }
}
